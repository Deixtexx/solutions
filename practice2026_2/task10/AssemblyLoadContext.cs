using System.IO;
using System.Runtime.Loader;
using System.Reflection;

namespace task10
{
    public class PluginLoadContext : AssemblyLoadContext
    {
        private readonly string _pluginFolder;

        public PluginLoadContext(string pluginFolder) : base(isCollectible: true)
        {
            _pluginFolder = pluginFolder;
        }

        protected override Assembly? Load(AssemblyName assemblyName)
        {
            // Проверяем, есть ли зависимая сборка в папке плагинов
            string assemblyPath = Path.Combine(_pluginFolder, $"{assemblyName.Name}.dll");
            if (File.Exists(assemblyPath))
            {
                return LoadFromAssemblyPath(assemblyPath);
            }
            return null;
        }
    }

}
