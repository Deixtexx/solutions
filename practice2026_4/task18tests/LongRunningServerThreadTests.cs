using System;
using System.Collections.Generic;
using System.Threading;
using Xunit;
using task17;
using task18;

namespace task18tests
{
    public class LongRunningServerThreadTests
    {
        public class ActionCommand : ICommand
        {
            private readonly Action _action;
            public ActionCommand(Action action) => _action = action;
            public void Execute() => _action();
        }
        // имитация длинной задачи (в несколько этапов)
        public class LongRunningCommand : ICommand
        {
            private readonly LongRunningServerThread _thread;
            private readonly string _name;
            private int _currentStep = 0;
            private readonly int _totalSteps;
            private readonly List<string> _order;

            public LongRunningCommand(LongRunningServerThread thread, string name, int totalSteps, List<string> order)
            {
                _thread = thread;
                _name = name;
                _totalSteps = totalSteps;
                _order = order;
            }

            public void Execute()
            {
                _currentStep++;
                _order.Add($"{_name}: шаг {_currentStep}");

                Thread.Sleep(50);

                if (_currentStep < _totalSteps) _thread.AddLongRunningCommand(this);
            }
        }

        public static ExceptionHandler handler = new ExceptionHandler();

        [Fact]
        public void RoundScheduler_ShouldSwitchLongTasks()
        {
            var scheduler = new RoundScheduler();
            var serverThread = new LongRunningServerThread(scheduler, handler);

            var executionOrder = new List<string>();

            var taskA = new ActionCommand(() => executionOrder.Add("A"));
            var taskB = new ActionCommand(() => executionOrder.Add("B"));

            serverThread.Start();

            // напрямую закидываем в планировщик, симулируя их многоэтапность
            serverThread.AddLongRunningCommand(taskA);
            serverThread.AddLongRunningCommand(taskB);

            serverThread.AddLongRunningCommand(taskA);
            serverThread.AddLongRunningCommand(taskB);

            Thread.Sleep(100);
            serverThread.Stop();
            serverThread.Join();

            // проверяем чередование
            Assert.Equal(4, executionOrder.Count);
            Assert.Equal("A", executionOrder[0]);
            Assert.Equal("B", executionOrder[1]);
            Assert.Equal("A", executionOrder[2]);
            Assert.Equal("B", executionOrder[3]);
        }

        [Fact]
        public void TestNewCommands_ShouldHavePriorityOverScheduler_WithSingleTask()
        {
            var scheduler = new RoundScheduler();
            var serverThread = new LongRunningServerThread(scheduler, handler);
            var order = new List<string>();

            var longTask = new LongRunningCommand(serverThread, "игра", 2, order);

            serverThread.AddLongRunningCommand(longTask);
            serverThread.Start();

            // даем время, чтобы шаг 1 успел пройти
            Thread.Sleep(5);

            // эта команда обязана вклиниться
            serverThread.AddCommand(new ActionCommand(() => order.Add("быстрая_команда")));

            Thread.Sleep(100);
            serverThread.Stop();
            serverThread.Join();

            Assert.Equal(3, order.Count);
            Assert.Equal("игра: шаг 1", order[0]);
            Assert.Equal("быстрая_команда", order[1]);
            Assert.Equal("игра: шаг 2", order[2]);
        }

        [Fact]
        public void ThreadSleepsWhenIdle_NoBusyWait()
        {
            var scheduler = new RoundScheduler();
            var serverThread = new LongRunningServerThread(scheduler, handler);

            serverThread.Start();
            Thread.Sleep(50);

            var wasExecutedAfterWakeup = false;
            serverThread.AddCommand(new ActionCommand(() => wasExecutedAfterWakeup = true));

            Thread.Sleep(50);
            serverThread.Stop();
            serverThread.Join();

            Assert.True(wasExecutedAfterWakeup);
        }
    }

}
