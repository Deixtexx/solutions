using System.Collections.Concurrent;
using task17;

namespace task18
{
    public class RoundScheduler : IScheduler 
    {
        private readonly ConcurrentQueue<ICommand> _tasks = new();

        public bool HasCommand() => !_tasks.IsEmpty;

        public ICommand Select()
        {
            if (_tasks.TryDequeue(out ICommand command)) return command;
            return null;
        }

        public void Add(ICommand command)
        {
            if (command != null) _tasks.Enqueue(command);
        }
    }
}
