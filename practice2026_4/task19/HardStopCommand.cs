using task17;
using task18;

namespace task19
{
    public class HardStopCommand : ICommand
    {
        private readonly LongRunningServerThread _thread;

        public HardStopCommand(LongRunningServerThread thread) => _thread = thread;

        public void Execute()
        {
            _thread.TriggerHardStop();
        }
    }
}
