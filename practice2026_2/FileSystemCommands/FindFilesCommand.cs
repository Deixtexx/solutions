using System.Text;
using CommandLib;

namespace FileSystemCommands
{
    public class FindFilesCommand : ICommand
    {
        private string _path { get; }
        private string _mask { get; }
        public Action<string>? callback;

        public IEnumerable<string> files { get; private set; }
        public FindFilesCommand(string path, string mask, Action<string>? cb)
        {
            _path = path;
            _mask = mask;
            callback = cb;
        }

        public void Execute()
        {
            files = Directory.EnumerateFiles(_path, _mask, SearchOption.AllDirectories)
                             .Select(p => Path.GetFileName(p));
            StringBuilder sb = new StringBuilder();
            foreach (var file in files) sb.Append(file + " ");
            callback?.Invoke(sb.ToString());
        }
    }
}
