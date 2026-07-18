using task17;
using task18;

namespace task19
{
    public class TestCommand : ICommand
    {
        private readonly int _id;
        private readonly LongRunningServerThread _thread;
        private int counter = 0;

        public TestCommand(int id, LongRunningServerThread thread)
        {
            _id = id;
            _thread = thread;
        }

        public void Execute()
        {
            Console.WriteLine($"Поток {_id} вызов {++counter}");

            if (counter < 3)
            {
                _thread.AddLongRunningCommand(this);
            }
        }
    }
}
