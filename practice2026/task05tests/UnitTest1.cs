using task05;
namespace task05tests
{
    public class TestClass
    {
        public int PublicField;
        private string _privateField;
        public int Property { get; set; }

        public void Method() { }
    }

    [Serializable]
    public class AttributedClass { }

    public class ClassWithMethods
    {
        public string Method1(int first, float second) => "a";
        public void Method2() { }
    }

    public class ClassAnalyzerTests
    {
        [Fact]
        public void GetPublicMethods_ReturnsCorrectMethods()
        {
            var analyzer = new ClassAnalyzer(typeof(TestClass));
            var methods = analyzer.GetPublicMethods();

            Assert.Contains("Method", methods);
        }

        [Fact]
        public void GetAllFields_IncludesPrivateFields()
        {
            var analyzer = new ClassAnalyzer(typeof(TestClass));
            var fields = analyzer.GetAllFields();

            Assert.Contains("_privateField", fields);
        }

        [Fact]
        public void GetProperties_ReturnsCorrectProperties()
        {
            var analyzer = new ClassAnalyzer(typeof(TestClass));
            var properties = analyzer.GetProperties();

            Assert.Contains("Property", properties);
        }

        [Fact]
        public void HasAttribute_ReturnsCorrectAnswer()
        {
            var analyzer = new ClassAnalyzer(typeof(AttributedClass));
            bool answer = analyzer.HasAttribute<SerializableAttribute>();

            Assert.True(answer);
        }

        [Fact]
        public void GetMethodParams_ReturnsCorrectParams()
        {
            var analyzer = new ClassAnalyzer(typeof(ClassWithMethods));
            var params1 = analyzer.GetMethodParams("Method1");
            var params2 = analyzer.GetMethodParams("Method2");

            Assert.Equal(3, params1.Count());
            Assert.Contains("first", params1);
            Assert.Contains("second", params1);
            Assert.Contains("String", params1);

            Assert.Single(params2);
            Assert.Contains("Void", params2);
        }
    }
}
