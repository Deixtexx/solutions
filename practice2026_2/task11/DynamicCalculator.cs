using System;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace task11
{
    public static class DynamicCalculator
    {
        public static ICalculator CompileCalculator(string sourceCode)
        {
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);

            var references = new MetadataReference[]
            {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(ICalculator).Assembly.Location), // ICalculator
            MetadataReference.CreateFromFile(Path.Combine(Path.GetDirectoryName(typeof(object).Assembly.Location)!, "System.Runtime.dll"))
            };

            CSharpCompilation compilation = CSharpCompilation.Create(
                "TestAssembly_" + Guid.NewGuid().ToString("N"),
                [ syntaxTree ],
                references,
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
            );

            using var ms = new MemoryStream();
            var result = compilation.Emit(ms);

            if (!result.Success)
            {
                string errors = string.Join(Environment.NewLine, result.Diagnostics.Select(d => d.GetMessage()));
                throw new InvalidOperationException($"Ошибка компиляции: {errors}");
            }

            ms.Seek(0, SeekOrigin.Begin);
            Assembly assembly = AssemblyLoadContext.Default.LoadFromStream(ms);

            Type? calculatorType = assembly.GetType("Calculator");
            if (calculatorType == null)
                throw new TypeLoadException("Класс 'Calculator' не найден в скомпилированной сборке");

            object? instance = Activator.CreateInstance(calculatorType);
            if (instance is ICalculator calculator)
                return calculator;

            throw new InvalidCastException("Скомпилированный класс не реализует интерфейс ICalculator");
        }
    }

}
