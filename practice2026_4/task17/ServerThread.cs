using System;
using System.Collections.Concurrent;
using System.Threading;
using task17;

public class ServerThread
{
    private readonly BlockingCollection<ICommand> _queue = new();
    private readonly ExceptionHandler _exceptionHandler;
    private readonly Thread _thread;

    private readonly CancellationTokenSource _cts = new();
    private volatile bool _isHardStop = false;

    public ServerThread(ExceptionHandler exceptionHandler)
    {
        _exceptionHandler = exceptionHandler;
        _thread = new Thread(RunLoop);
    }

    public void Start() => _thread.Start();
    public void Join() => _thread.Join();
    public bool IsAlive => _thread.IsAlive;

    public void AddCommand(ICommand command)
    {
        if (!_cts.IsCancellationRequested) _queue.Add(command);
    }

    private void RunLoop()
    {
        while (!_isHardStop)
        {
            ICommand command;
            try
            {
                if (!_queue.TryTake(out command, Timeout.Infinite, _cts.Token)) continue;
            }
            catch (OperationCanceledException)
            {
                if (_isHardStop) break;
                while (_queue.TryTake(out command)) ExecuteSingleCommand(command);

                break;
            }

            ExecuteSingleCommand(command);
        }
    }

    private void ExecuteSingleCommand(ICommand command)
    {
        try
        {
            command?.Execute();
        }
        catch (Exception ex)
        {
            _exceptionHandler?.Handle(command, ex);
        }
    }

    public void TriggerHardStop()
    {
        VerifyCurrentThread();
        _isHardStop = true;
        _cts.Cancel(); // Мгновенно будит поток из TryTake
    }

    public void TriggerSoftStop()
    {
        VerifyCurrentThread();
        _cts.Cancel(); // Будит поток. Код в блоке catch довыполнит оставшиеся команды.
    }

    private void VerifyCurrentThread()
    {
        if (Thread.CurrentThread != _thread)
        {
            throw new InvalidOperationException("Команду остановки можно вызвать только внутри целевого потока!");
        }
    }
}

