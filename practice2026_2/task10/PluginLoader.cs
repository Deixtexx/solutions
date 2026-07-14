using System;
using System.IO;
using System.Reflection;

namespace task10
{
    public class PluginLoaderApp
    {
        public static void RunPlugins(string pluginsFolderPath)
        {
            if (!Directory.Exists(pluginsFolderPath))
            {
                Console.WriteLine("папка с плагинами не найдена");
                return;
            }

            var sortedPluginPaths = PluginResolver.GetLoadOrder(pluginsFolderPath);
            var loadContext = new PluginLoadContext(pluginsFolderPath);

            // загружаем сборки в память строго по порядку
            foreach (var path in sortedPluginPaths)
            {
                try
                {
                    Assembly assembly = loadContext.LoadFromAssemblyPath(path);

                    foreach (Type type in assembly.GetTypes())
                    {
                        if (type.IsClass && !type.IsAbstract && type.GetCustomAttribute<PluginLoadAttribute>() != null)
                        {
                            try
                            {
                                object? instance = Activator.CreateInstance(type);

                                if (instance != null)
                                {
                                    MethodInfo? executeMethod = type.GetMethod("Execute");

                                    if (executeMethod != null) executeMethod.Invoke(instance, null);
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"не удалось запустить команду {type.FullName}: {ex.Message}");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ошибка при загрузке плагина {Path.GetFileName(path)}: {ex.Message}");
                }
            }
        }
    }

}
