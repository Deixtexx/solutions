using CommandLib;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
class Program
{
    public static void Main(string[] args)
    {
        string baseDir = AppDomain.CurrentDomain.BaseDirectory;

        // cклеиваем путь, выходя на уровень Solution и заходя в папку соседнего проекта
        string dllPath = Path.GetFullPath(Path.Combine(baseDir, @"..\..\..\..\FileSystemCommands\bin\Debug\net10.0\FileSystemCommands.dll"));
        if (!File.Exists(dllPath)) return;

        Assembly assembly = Assembly.LoadFrom(dllPath);
        var commandTypes = assembly.GetTypes()
                                   .Where(t => typeof(ICommand).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

        foreach(var t in commandTypes)
        {
            object[] arg;
            var testDir = Path.Combine(Path.GetTempPath(), "TestDir2");

            if (Directory.Exists(testDir)) Directory.Delete(testDir, true);
            Directory.CreateDirectory(testDir);

            File.WriteAllText(Path.Combine(testDir, "test1.txt"), "Hello"); // 5 байт
            File.WriteAllText(Path.Combine(testDir, "test2.txt"), "World");

            Action<string> callback = (x) => { Console.WriteLine(x); };

            if (t.Name.Equals("DirectorySizeCommand")) arg = [testDir, callback];
            else arg = [testDir, "*.txt", callback];

            ICommand? command = (ICommand?) Activator.CreateInstance(t, arg);
            command?.Execute();
        }
    }
}
