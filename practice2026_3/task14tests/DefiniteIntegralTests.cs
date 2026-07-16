using Xunit;
using task14;

namespace task14tests
{
    public class DefiniteIntegralTests
    {
        [Fact]
        public void DefiniteIntegral_CorrectValueOnLinearFunction()
        {
            var X = (double x) => x;
            Assert.Equal(0, DefiniteIntegral.Solve(-1, 1, X, 1e-4, 2), 1e-4);
        }

        [Fact]
        public void DefiniteIntegral_CorrectValueOnSin()
        {
            var SIN = (double x) => Math.Sin(x);
            Assert.Equal(0, DefiniteIntegral.Solve(-1, 1, SIN, 1e-5, 8), 1e-4);
        }

        [Fact]
        public void DefiniteIntegral_CorrectValueOnXInPositiveRange()
        {
            var X = (double x) => x;
            Assert.Equal(12.5, DefiniteIntegral.Solve(0, 5, X, 1e-6, 8), 1e-5);
        }
    }
}
