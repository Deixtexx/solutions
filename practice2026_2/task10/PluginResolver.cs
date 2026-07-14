using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace task10
{
    public class PluginResolver
    {
        public static List<string> GetLoadOrder(string folderPath)
        {
            var dllFiles = Directory.GetFiles(folderPath, "*.dll");
            var fileMap = dllFiles.ToDictionary(f => Path.GetFileNameWithoutExtension(f), f => f, StringComparer.OrdinalIgnoreCase);

            var adjacencyList = new Dictionary<string, List<string>>();

            foreach (var file in dllFiles)
            {
                var assemblyName = AssemblyName.GetAssemblyName(file);
                var dependencies = new List<string>();

                try
                {
                    var asm = Assembly.LoadFile(file);
                    foreach (var referencedAsm in asm.GetReferencedAssemblies())
                    {
                        if (fileMap.ContainsKey(referencedAsm.Name ?? string.Empty))
                        {
                            dependencies.Add(fileMap[referencedAsm.Name!]);
                        }
                    }
                }
                catch { }

                adjacencyList[file] = dependencies;
            }

            return TopologicalSort(adjacencyList);
        }

        public static List<string> TopologicalSort(Dictionary<string, List<string>> graph)
        {
            var result = new List<string>();
            var visited = new Dictionary<string, bool>(); // false = в процессе (цикл), true = посещен

            foreach (var node in graph.Keys)
            {
                if (!visited.ContainsKey(node))
                {
                    DepthSearch(node, graph, visited, result);
                }
            }

            return result;
        }

        private static void DepthSearch(string node, Dictionary<string, List<string>> graph, Dictionary<string, bool> visited, List<string> result)
        {
            if (!graph.ContainsKey(node)) return;

            visited[node] = false;

            foreach (var neighbor in graph[node])
            {
                if (string.Equals(node, neighbor, StringComparison.OrdinalIgnoreCase))
                    throw new InvalidOperationException($"обнаружена самоссылка: плагин {node} ссылается сам на себя");

                if (!visited.TryGetValue(neighbor, out bool isVisited))
                    DepthSearch(neighbor, graph, visited, result);

                else if (!isVisited)
                    throw new InvalidOperationException("обнаружена циклическая зависимость между плагинами");
            }

            visited[node] = true;
            result.Add(node);
        }
    }

}
