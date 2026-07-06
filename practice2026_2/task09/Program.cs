using System;
using System.Reflection;

namespace task09
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length == 0) return;

            try
            {
                // Вызываем метод анализа
                string result = MetadataAnalyzer.AnalyzeAssembly(args[0]);
                Console.WriteLine(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке библиотеки: {ex.Message}");
            }
        }
    }

}

