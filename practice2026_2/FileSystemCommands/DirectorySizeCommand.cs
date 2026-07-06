using CommandLib;
using System.IO;
using System.Text;

namespace FileSystemCommands
{
    public class DirectorySizeCommand : ICommand
    {
        private string _path { get; } 
        public long Size { get; private set; }
        public Action<string>? callback;

        public DirectorySizeCommand(string path, Action<string>? cb)
        {
            _path = path;
            callback = cb;
        }
        public void Execute()
        {
            Size = new DirectoryInfo(_path)
                        .GetFiles("*.*", SearchOption.AllDirectories)
                        .Sum(file => file.Length);
            callback?.Invoke(Size.ToString());
        }
    }
}
