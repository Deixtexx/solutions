using System;
using task11;
using Xunit;

namespace task11tests
{
    public class DynamicCalculatorTests
    {
        // Исходный корректный код для успешных тестов
        private const string ValidCalculatorCode = @"
        public class Calculator : task11.ICalculator
        {
            public int Add(int a, int b) => a + b;
            public int Minus(int a, int b) => a - b;
            public int Mul(int a, int b) => a * b;
            public int Div(int a, int b) => a / b;
        }";

        [Fact]
        public void CompileCalculator_AllMethods_ExecuteCorrectly()
        {
            ICalculator calculator = DynamicCalculator.CompileCalculator(ValidCalculatorCode);

            Assert.Equal(0, calculator.Add(-3, 3));
            Assert.Equal(5, calculator.Minus(10, 5));
            Assert.Equal(50, calculator.Mul(10, 5));
            Assert.Equal(2, calculator.Div(10, 5));
        }

        [Fact]
        public void CompileCalculator_DivideByZero_ThrowsDivideByZeroException()
        {
            ICalculator calculator = DynamicCalculator.CompileCalculator(ValidCalculatorCode);

            Assert.Throws<DivideByZeroException>(() => calculator.Div(10, 0));
        }

        [Fact]
        public void CompileCalculator_SyntaxErrorInCode_ThrowsInvalidOperationException()
        {
            // код со сломанным синтаксисом (пропущена точка с запятой и закрывающая скобка)
            string brokenCode = @"
            public class Calculator : task11.ICalculator
            {
                public int Add(int a, int b) => a + b
            ";

            var exception = Assert.Throws<InvalidOperationException>(() =>
                DynamicCalculator.CompileCalculator(brokenCode)
            );

            Assert.Contains("Ошибка компиляции", exception.Message);
        }
    }
}
