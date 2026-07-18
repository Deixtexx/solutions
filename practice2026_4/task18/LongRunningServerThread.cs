using System;
using System.Collections.Concurrent;
using System.Threading;
using task17;

namespace task18
{
    public class LongRunningServerThread
    {
        private readonly ConcurrentQueue<ICommand> _queue = new();
        private readonly IScheduler _scheduler;
        private readonly AutoResetEvent _signal = new(false);
        private readonly Thread _thread;
        private readonly ExceptionHandler _exceptionHandler;

        private volatile bool _isRunning = true;
        private volatile bool _isHardStop = false;

        public LongRunningServerThread(IScheduler scheduler, ExceptionHandler exceptionHandler)
        {
            _scheduler = scheduler;
            _exceptionHandler = exceptionHandler;
            _thread = new Thread(RunLoop);
        }

        public void Start() => _thread.Start();
        public void Join() => _thread.Join();
        public bool IsAlive() => _thread.IsAlive;
        public void Stop()
        {
            _isRunning = false;
            _signal.Set();
        }

        public void AddCommand(ICommand command)
        {
            _queue.Enqueue(command);
            _signal.Set();
        }
        public void AddLongRunningCommand(ICommand command)
        {
            _scheduler.Add(command);
            _signal.Set();
        }
        public void TriggerHardStop()
        {
            if (Thread.CurrentThread != _thread)
                throw new InvalidOperationException("HardStop можно вызвать только изнутри целевого потока");
            _isHardStop = true;
            _isRunning = false;
        }
        private void RunLoop()
        {
            while(_isRunning)
            {
                if (_isHardStop) break;

                bool processedWork = false;

                if (_queue.TryDequeue(out ICommand task))
                {
                    ExecuteSingleCommand(task);
                    processedWork = true;
                }
                else if (_scheduler.HasCommand())
                {
                    var longTask = _scheduler.Select();
                    if (longTask != null)
                    {
                        ExecuteSingleCommand(longTask);
                        processedWork = true;
                    }
                }

                if (!processedWork && _isRunning) _signal.WaitOne();
            }
        }
        
        private void ExecuteSingleCommand(ICommand command)
        {
            try
            {
                command?.Execute();
            }
            catch(Exception ex)
            {
                _exceptionHandler.Handle(command, ex);
            }
        }
    }
}
