using task07;
using System.ComponentModel;
using System.Reflection;
using DisplayNameAttribute = task07.DisplayNameAttribute;

namespace task07tests
{
    [DisplayName("Пример класса")]
    [Version(1, 0)]
    public class SampleClass
    {
        [DisplayName("Тестовый метод")]
        public void TestMethod() { }
        [DisplayName("Числовое свойство")]
        public int Number { get; set; }
    }
    public class AttributeReflectionTests
    {
        [Fact]
        public void Class_HasDisplayNameAttribute()
        {
            var type = typeof(SampleClass);
            var attribute = type.GetCustomAttribute<DisplayNameAttribute>();
            Assert.NotNull(attribute);
            Assert.Equal("Пример класса", attribute.DisplayName);
        }

        [Fact]
        public void Method_HasDisplayNameAttribute()
        {
            var method = typeof(SampleClass).GetMethod("TestMethod");
            var attribute = method.GetCustomAttribute<DisplayNameAttribute>();
            Assert.NotNull(attribute);
            Assert.Equal("Тестовый метод", attribute.DisplayName);
        }

        [Fact]
        public void Property_HasDisplayNameAttribute()
        {
            var prop = typeof(SampleClass).GetProperty("Number");
            var attribute = prop.GetCustomAttribute<DisplayNameAttribute>();
            Assert.NotNull(attribute);
            Assert.Equal("Числовое свойство", attribute.DisplayName);
        }

        [Fact]
        public void Class_HasVersionAttribute()
        {
            var type = typeof(SampleClass);
            var attribute = type.GetCustomAttribute<VersionAttribute>();
            Assert.NotNull(attribute);
            Assert.Equal(1, attribute.Major);
            Assert.Equal(0, attribute.Minor);
        }
        [Fact]
        public void ReflectionHelper_OutputsCorrectValues()
        {
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);

                ReflectionHelper.PrintTypeInfo(typeof(SampleClass));

                var result = sw.ToString().Split("\r\n");
                Assert.Equal(5, result.Length); //в конце добавляется "", поэтому 5 вместо 4
                Assert.Contains("Пример класса", result);
                Assert.Contains("1.0", result);
                Assert.Contains("TestMethod - Тестовый метод", result);
                Assert.Contains("Number - Числовое свойство", result);
            }
        }
    }
}
