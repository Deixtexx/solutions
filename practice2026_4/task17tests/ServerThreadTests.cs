using System;
using System.Threading;
using Xunit;
using task17;

namespace task17tests
{
    public class ActionCommand : ICommand
    {
        private readonly Action _action;
        public ActionCommand(Action action) => _action = action;
        public void Execute() => _action();
    }

    public class ServerThreadTests
    {
        [Fact]
        public void TestHardStop_ShouldStopImmediately_AndLeaveRemainingCommands()
        {
            var handler = new ExceptionHandler();
            var serverThread = new ServerThread(handler);
            var wasThirdCommandExecuted = false;

            serverThread.Start();

            serverThread.AddCommand(new ActionCommand(() => Thread.Sleep(20)));
            serverThread.AddCommand(new HardStopCommand(serverThread));
            // эта команда добавлена, но из-за HardStop поток должен мгновенно выйти, не выполнив её
            serverThread.AddCommand(new ActionCommand(() => wasThirdCommandExecuted = true));

            serverThread.Join();

            Assert.False(serverThread.IsAlive);
            Assert.False(wasThirdCommandExecuted);
        }

        [Fact]
        public void TestSoftStop_ShouldExecuteAllRemainingCommands_ThenStop()
        {
            var handler = new ExceptionHandler();
            var serverThread = new ServerThread(handler);
            var wasThirdCommandExecuted = false;

            serverThread.Start();

            serverThread.AddCommand(new ActionCommand(() => Thread.Sleep(20)));
            serverThread.AddCommand(new SoftStopCommand(serverThread));
            serverThread.AddCommand(new ActionCommand(() => wasThirdCommandExecuted = true));

            serverThread.Join();

            Assert.False(serverThread.IsAlive);
            Assert.True(wasThirdCommandExecuted);
        }
    }

}
