using Frontend.Helpers;

namespace FrontendxUnitTest
{
    public class HelperTests
    {
        [Fact]
        public void IsValidMACAddress_ShouldReturnFalse_WhenStrIsNullOrEmpty()
        {
            bool result1 = Helper.IsValidMACAddress("");
            bool result2 = Helper.IsValidMACAddress(null!);

            Assert.False(result1);
            Assert.False(result2);
        }

        [Fact]
        public void IsValidMACAddress_ShouldReturnFalse_WhenStrIsTooShort()
        {
            string str = "AA-BB-CC-DD";
            bool result = Helper.IsValidMACAddress(str);

            Assert.False(result);
        }

        [Fact]
        public void IsValidMACAddress_ShouldReturnFalse_WhenStrHasInvalideCharacters()
        {
            string str = "AA-BB-CC-DD-EE-HH";
            bool result = Helper.IsValidMACAddress(str);

            Assert.False(result);
        }

        [Fact]
        public void IsValidMACAddress_ShouldReturnTrue_WhenStrIsValidMacAddress()
        {
            string str1 = "AA-BB-CC-DD-EE-FF";
            string str2 = "AA:BB:CC:DD:EE:FF";
            bool result1 = Helper.IsValidMACAddress(str1);
            bool result2 = Helper.IsValidMACAddress(str2);

            Assert.True(result1);
            Assert.True(result2);
        }
    }
}
