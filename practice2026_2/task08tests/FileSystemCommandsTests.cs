using FileSystemCommands;
namespace task08tests
{
    public class FileSystemCommandsTests
    {
        [Fact]
        public void DirectorySizeCommand_ShouldCalculateSize()
        {
            var testDir = Path.Combine(Path.GetTempPath(), "TestDir");

            if (Directory.Exists(testDir)) Directory.Delete(testDir, true);
            Directory.CreateDirectory(testDir);

            File.WriteAllText(Path.Combine(testDir, "test1.txt"), "Hello"); // 5 байт
            File.WriteAllText(Path.Combine(testDir, "test2.txt"), "World"); // 5 байт

            var command = new DirectorySizeCommand(testDir, null);
            command.Execute(); // Проверяем, что не возникает исключений

            Assert.Equal(10, command.Size);

            Directory.Delete(testDir, true);
        }

        [Fact]
        public void FindFilesCommand_ShouldFindMatchingFiles()
        {
            var testDir = Path.Combine(Path.GetTempPath(), "TestDir3");

            if (Directory.Exists(testDir)) Directory.Delete(testDir, true);
            Directory.CreateDirectory(testDir);

            File.WriteAllText(Path.Combine(testDir, "file1.txt"), "Text");
            File.WriteAllText(Path.Combine(testDir, "file2.log"), "Log");

            var command = new FindFilesCommand(testDir, "*.txt", null);
            command.Execute(); // Должен найти 1 файл

            Assert.Single(command.files);
            Assert.Contains("file1.txt", command.files);

            Directory.Delete(testDir, true);
        }
    }
}
