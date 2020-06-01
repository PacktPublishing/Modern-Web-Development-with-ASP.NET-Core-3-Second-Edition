using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace UnitTests
{
    public class CalculatorTest
    {
        [Theory]
        [InlineData(1, 2, 3)]
        [InlineData(1, 1, 2)]
        public void Calculate(int a, int b, int c)
        {
            Assert.Equal(c, a + b);
        }
    }
}
