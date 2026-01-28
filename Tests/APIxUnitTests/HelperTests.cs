using API.Helpers;
using System.Text.Json;

namespace TestProject1
{
    public class HelperTests
    {
        [Fact]
        public void SafeGet_ShouldReturn0_WhenInputIsNull()
        {
            int result = Helper.SafeGet<int>(null!);
            Assert.Equal(0, result);
        }

        [Fact]
        public void SafeGet_ShouldReturnTheInputValue_WhenInputIsNotAJsonElement()
        {
            List<dynamic> inputs = new List<dynamic>() { "I am a string", 55, 'c', true };

            foreach (dynamic input in inputs)
            {
                dynamic result = Helper.SafeGet<dynamic>(input);
                Assert.Equal(input, result);
            }
        }

        [Fact]
        public void SafeGet_ShouldReturnDefaultValue_WhenInputIsJsonElementNull()
        {
            var input = JsonDocument.Parse("null").RootElement;
            int result = Helper.SafeGet<int>(input);

            Assert.Equal(0, result);
        }

        [Fact]
        public void SafeGet_ShouldReturnIntWithCorrectValues_WhenInputIsJsonElementInt()
        {
            var input = JsonDocument.Parse("55").RootElement;

            var result = Helper.SafeGet<int>(input);

            Assert.Equal(55, result);
        }
        [Fact]
        public void SafeGet_ShouldReturnStringWithCorrectValues_WhenInputIsJsonElementString()
        {
            var input = JsonDocument.Parse("\"I am a string\"").RootElement;

            var result = Helper.SafeGet<string>(input);

            Assert.Equal("I am a string", result);
        }
        [Fact]
        public void SafeGet_ShouldReturnArrayWithCorrectValues_WhenInputIsJsonElementArray()
        {
            var input = JsonDocument.Parse("[1,2,3,4,5,6,7,8,9,0]").RootElement;

            var result = Helper.SafeGet<int[]>(input);

            Assert.Equal([1, 2, 3, 4, 5, 6, 7, 8, 9, 0], result);
        }
        [Fact]
        public void SafeGet_ShouldReturnBoolWithCorrectValues_WhenInputIsJsonElementBool()
        {
            var input = JsonDocument.Parse("true").RootElement;

            var result = Helper.SafeGet<bool>(input);

            Assert.True(result);
        }
    }
}
