using System.Threading;
using task17;
using task18;
using task19;
using Xunit;

namespace task19tests
{
    public class ActionCommand : ICommand
    {
        private readonly Action _action;
        public ActionCommand(Action action) => _action = action;
        public void Execute() => _action();
    }

    public class HardStop_WithLongCommands_WorksAsExpected
    {
        public static ExceptionHandler handler = new ExceptionHandler();
        [Fact]
        public void FiveCommands_ThenHardStop()
        {
            var scheduler = new RoundScheduler();
            var serverThread = new LongRunningServerThread(scheduler, handler);

            // 5 экземпляров
            for (int i = 1; i <= 5; i++)
            {
                serverThread.AddLongRunningCommand(new TestCommand(id: i, serverThread));
            }

            serverThread.Start();
            Thread.Sleep(100);

            serverThread.AddCommand(new task19.HardStopCommand(serverThread));

            serverThread.Join();

            // проверяем, что поток действительно успешно завершил свою работу
            Assert.False(serverThread.IsAlive());
        }

        [Fact]
        public void HardStopFromWrongThread_ThrowsException()
        {
            var scheduler = new RoundScheduler();
            var serverThread = new LongRunningServerThread(scheduler, handler);

            bool wasNextCommandExecuted = false;

            var hardStop = new task19.HardStopCommand(serverThread);

            var exception = Assert.Throws<InvalidOperationException>(() => hardStop.Execute());
            Assert.Equal("HardStop можно вызвать только изнутри целевого потока", exception.Message);


            serverThread.Start();

            serverThread.AddCommand(hardStop);
            serverThread.AddCommand(new ActionCommand(() => wasNextCommandExecuted = true));

            serverThread.Join();

            // поток умер
            Assert.False(serverThread.IsAlive());

            // следующая команда не была выполнена
            Assert.False(wasNextCommandExecuted);
        }
    }
}
