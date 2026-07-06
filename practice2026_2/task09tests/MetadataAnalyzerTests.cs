using task09;

namespace task09tests
{
    public class MetadataAnalyzerTests
    {
        [Fact]
        public void AnalyzeAssembly_ShouldThrowException_WhenPathDoesNotExist()
        {
            // задаем несуществующий путь к файлу
            string invalidPath = "non_existent_file.dll";

            Assert.Throws<FileNotFoundException>(() => MetadataAnalyzer.AnalyzeAssembly(invalidPath));
        }

        [Fact]
        public void AnalyzeAssembly_ShouldContainClassAndMethodsInfo()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string dllPath = Path.GetFullPath(Path.Combine(baseDir, "TestLib.dll"));

            string output = MetadataAnalyzer.AnalyzeAssembly(dllPath);

            Assert.Contains("SampleClass - 2.10", output);
            Assert.Contains("CalculateSum - тестовое вычисление", output);
            Assert.Contains("Класс: TestLib.SampleClass", output);
            Assert.Contains("Int32 _secretId", output);
            Assert.Contains("String Name", output);
            Assert.Contains("SampleClass(String initialName, Int32 id)", output);
            Assert.Contains("Int32 CalculateSum(Int32 a, Int32 b)", output);
        }
    }
}

