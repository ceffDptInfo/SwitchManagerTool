using System.Text.RegularExpressions;

namespace Frontend.Helpers
{
    public static class Helper
    {
        public static bool IsValidMACAddress(string str)
        {
            // Regex to check valid
            // MAC address
            string regex = "^([0-9A-Fa-f]{2}[:-])"
                        + "{5}([0-9A-Fa-f]{2})|"
                        + "([0-9a-fA-F]{4}\\."
                        + "[0-9a-fA-F]{4}\\."
                        + "[0-9a-fA-F]{4})$";

            // Compile the ReGex
            Regex p = new Regex(regex);

            // If the string is empty
            // return false
            if (str == null)
            {
                return false;
            }

            // Find match between given string
            // and regular expression
            // uSing Pattern.matcher()

            Match m = p.Match(str);

            // Return if the string
            // matched the ReGex
            return m.Success;
        }
    }
}
