using System;
using System.Collections.Generic;
using System.Text;

namespace task17
{
    public class HardStopCommand : ICommand
    {
        private readonly ServerThread _serverThread;

        public HardStopCommand(ServerThread serverThread) => _serverThread = serverThread;

        public void Execute() => _serverThread.TriggerHardStop();
    }

    public class SoftStopCommand : ICommand
    {
        private readonly ServerThread _serverThread;

        public SoftStopCommand(ServerThread serverThread) => _serverThread = serverThread;

        public void Execute() => _serverThread.TriggerSoftStop();
    }
}
