using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using task07;

namespace task09
{
    public static class MetadataAnalyzer
    {
        public static string AnalyzeAssembly(string path)
        {
            var writer = new StringBuilder();

            try
            {
                if (!File.Exists(path)) throw new FileNotFoundException($"Файл по пути '{path}' не найден.");

                Assembly assembly = Assembly.LoadFrom(path);
                Type[] types;

                try
                {
                    types = assembly.GetTypes();
                }
                catch (ReflectionTypeLoadException rtle)
                {
                    writer.AppendLine("некоторые типы не удалось загрузить.");
                    writer.AppendLine("нричины (ошибки загрузчика):");
                    foreach (var loaderEx in rtle.LoaderExceptions)
                    {
                        if (loaderEx != null)
                            writer.AppendLine($"  - {loaderEx.Message}");
                    }

                    // извлекаем только те типы, которые успешно загрузились, чтобы проанализировать хотя бы их
                    var validTypes = new List<Type>();
                    foreach (var t in rtle.Types)
                    {
                        if (t != null) validTypes.Add(t);
                    }
                    types = validTypes.ToArray();
                }

                foreach (Type type in types)
                {
                    try
                    {
                        AnalyzeSingleType(type, writer);
                    }
                    catch (Exception ex)
                    {
                        writer.AppendLine($"ошибка: не удалось проанализировать тип {type.FullName}. причина: {ex.Message}");
                    }
                }
            }
            catch (FileNotFoundException ex)
            {
                writer.AppendLine($"ошибка: {ex.Message}");
                throw; // пробрасываем дальше для тестов, если нужно
            }
            catch (Exception ex)
            {
                writer.AppendLine($"непредвиденная ошибка при анализе сборки: {ex.Message}");
                throw;
            }

            return writer.ToString();
        }
        private static void AnalyzeSingleType(Type type, StringBuilder writer)
        {
            // Атрибуты
            AppendCustomAttributes(writer, type);
            writer.AppendLine($"Класс: {type.FullName}");

            // Поля
            writer.AppendLine("\n[Поля:]");
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            if (fields.Length == 0) writer.AppendLine("  (нет полей)");
            else
            {
                foreach (var field in fields)
                    writer.AppendLine($"  {field.FieldType.Name} {field.Name}");
            }

            // Конструкторы
            writer.AppendLine("\n[Конструкторы:]");
            var constructors = type.GetConstructors();
            if (constructors.Length == 0) writer.AppendLine("  (нет конструкторов)");
            else
            {
                foreach (var ctor in constructors)
                {
                    writer.Append($"  {type.Name}(");
                    AppendParameters(writer, ctor.GetParameters());
                    writer.AppendLine(")");
                }
            }

            // Методы
            writer.AppendLine("\n[Методы:]");
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            if (methods.Length == 0) writer.AppendLine("  (нет методов)");
            else
            {
                foreach (var method in methods)
                {
                    if (method.IsSpecialName) continue;
                    writer.Append($"  {method.ReturnType.Name} {method.Name}(");
                    AppendParameters(writer, method.GetParameters());
                    writer.AppendLine(")");
                }
            }
            writer.AppendLine("\n");
        }
        private static void AppendCustomAttributes(StringBuilder writer, MemberInfo member)
        {
            using (var sw = new StringWriter(writer)) {
                Console.SetOut(sw);
                ReflectionHelper.PrintTypeInfo((Type) member);
            }
        }
        private static void AppendParameters(StringBuilder writer, ParameterInfo[] parameters)
        {
            for (int i = 0; i < parameters.Length; i++)
            {
                writer.Append($"{parameters[i].ParameterType.Name} {parameters[i].Name}");
                if (i < parameters.Length - 1) writer.Append(", ");
            }
        }
    }
}
