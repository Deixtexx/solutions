using System;
using System.Collections.Generic;
using Xunit;
using task10;

namespace task10tests
{
    public class PluginResolverTests
    {
        [Fact]
        public void TopologicalSort_NoDependencies_ReturnsAllPlugins()
        {
            //3 плагина, которые вообще не зависят друг от друга
            var graph = new Dictionary<string, List<string>>
            {
                { "PluginA.dll", new List<string>() },
                { "PluginB.dll", new List<string>() },
                { "PluginC.dll", new List<string>() }
            };

            var result = PluginResolver.TopologicalSort(graph);

            Assert.Equal(3, result.Count);
            Assert.Contains("PluginA.dll", result);
            Assert.Contains("PluginB.dll", result);
            Assert.Contains("PluginC.dll", result);
        }

        [Fact]
        public void TopologicalSort_SimpleDependency_LoadsDependencyFirst()
        {
            // PluginB зависит от PluginA (PluginA должен загрузиться раньше)
            // граф: B -> A
            var graph = new Dictionary<string, List<string>>
            {
                { "PluginB.dll", new List<string> { "PluginA.dll" } },
                { "PluginA.dll", new List<string>() }
            };

            var result = PluginResolver.TopologicalSort(graph);

            int indexA = result.IndexOf("PluginA.dll");
            int indexB = result.IndexOf("PluginB.dll");

            Assert.True(indexA < indexB, "PluginA должен быть загружен раньше, чем PluginB");
        }

        [Fact]
        public void TopologicalSort_ComplexChain_ReturnsCorrectOrder()
        {
            var graph = new Dictionary<string, List<string>>
            {
                { "MainPlugin.dll", new List<string> { "Database.dll" } },
                { "Database.dll", new List<string> { "CoreLib.dll", "Logger.dll" } },
                { "Logger.dll", new List<string> { "CoreLib.dll" } },
                { "CoreLib.dll", new List<string>() }
            };

            var result = PluginResolver.TopologicalSort(graph);

            int coreIndex = result.IndexOf("CoreLib.dll");
            int loggerIndex = result.IndexOf("Logger.dll");
            int dbIndex = result.IndexOf("Database.dll");
            int mainIndex = result.IndexOf("MainPlugin.dll");

            Assert.True(coreIndex < loggerIndex, "CoreLib перед Logger");
            Assert.True(loggerIndex < dbIndex, "Logger перед Database");
            Assert.True(coreIndex < dbIndex, "CoreLib перед Database");
            Assert.True(dbIndex < mainIndex, "Database перед MainPlugin");
        }

        [Fact]
        public void TopologicalSort_CircularDependency_ThrowsInvalidOperationException()
        {
            // циклическая зависимость
            var graph = new Dictionary<string, List<string>>
            {
                { "PluginA.dll", new List<string> { "PluginB.dll" } },
                { "PluginB.dll", new List<string> { "PluginC.dll" } },
                { "PluginC.dll", new List<string> { "PluginA.dll" } }
            };

            var exception = Assert.Throws<InvalidOperationException>(() =>
                PluginResolver.TopologicalSort(graph)
            );

            Assert.Equal("обнаружена циклическая зависимость между плагинами", exception.Message);
        }

        [Fact]
        public void TopologicalSort_SelfDependency_ThrowsInvalidOperationException()
        {
            // плагин ошибочно ссылается сам на себя
            var graph = new Dictionary<string, List<string>>
            {
                { "PluginA.dll", new List<string> { "PluginA.dll" } }
            };

            var exception = Assert.Throws<InvalidOperationException>(() =>
                PluginResolver.TopologicalSort(graph)
            );

            Assert.Equal("обнаружена самоссылка: плагин PluginA.dll ссылается сам на себя", exception.Message);
        }
    }

}
