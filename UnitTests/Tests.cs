using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace UnitTests
{
    public class Tests
    {
        [Fact]
        public void IndexTest()
        {
            // Arrange
            int a = 5;

            // Act
            a += 10;

            // Assert
            Assert.Equal(15, a);
            //Assert.NotNull(result);
            //Assert.Equal("Index", result?.ViewName);
        }
    }
}
