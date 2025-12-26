namespace IntroductionToWebAPIs.Tests
{
    public class UnitTest2
    {
        [Fact]
        public void Test2()
        {
            // Arrange
            string str1 = "Hello, ";
            string str2 = "World!";
            // Act
            string result = str1 + str2;
            // Assert
            Assert.Equal("Hello, World!", result);
        }
    }
}
