using task07;
namespace TestLib
{
    [Version(2, 10)]
    public class SampleClass
    {
        private int _secretId;
        public string Name;

        public SampleClass(string initialName, int id)
        {
            Name = initialName;
            _secretId = id;
        }

        [DisplayName("тестовое вычисление")]
        public int CalculateSum(int a, int b)
        {
            return a + b;
        }
    }
}
