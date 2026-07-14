using System;
using System.Collections.Generic;
using System.Text;

namespace task10
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class PluginLoadAttribute : Attribute { }

    public interface ICommand
    {
        void Execute();
    }

}
