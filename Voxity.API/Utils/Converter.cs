using System.Text.RegularExpressions;

namespace Voxity.API.Utils
{
    /// <summary/>
    public class Converter
    {
        private static Regex phoneInt1 = new Regex(@"^(\+\d{0,15})$");

        /// <summary/>
        public static string ConvertPhone(string number)
        {
            string modNum = number;
            char[] ignoreChar = { ' ', '\'', '/', '(', ')', '.', ':', '-' };
            foreach (char c in ignoreChar)
                modNum = modNum.Replace(c.ToString(), string.Empty);

            if (!modNum.StartsWith("+"))
                modNum = "+" + modNum;

            if (modNum.StartsWith("+0"))
                modNum = modNum.Remove(1, 1);

            if (!modNum.StartsWith("+33"))
                modNum = modNum.Insert(1, "33");


            if (phoneInt1.IsMatch(modNum))
                return modNum;

            return number;
        }
    }
}
