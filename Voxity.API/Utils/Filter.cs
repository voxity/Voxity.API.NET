using System.Text.RegularExpressions;

namespace Voxity.API.Utils
{
    /// <summary/>
    public class Filter
    {
        private static Regex phoneInt1 = new Regex(@"^(\+\d{0,15})$");
        private static Regex phoneInt2 = new Regex(@"^(00\d{0,15})$");
        private static Regex phoneFr = new Regex(@"^(0[1-9]\d{8})$");
        private static Regex phoneSpecial = new Regex(@"^([1-9]\d{3,5})$");

        private static Regex phoneRac = new Regex(@"^(\*[0-9]{1,4})$");
        private static Regex filterMail = new Regex(@"^([a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$)|(^$)");

        /// <summary/>
        public static bool ValidPhone(string number)
        {
            char[] ignoreChar = { ' ', '\'', '/', '(', ')', '.', ':', '-' };
            foreach (char c in ignoreChar)
                number = number.Replace(c.ToString(), string.Empty);

            if (phoneInt1.IsMatch(number) || phoneInt2.IsMatch(number) || phoneFr.IsMatch(number) || phoneSpecial.IsMatch(number))
                return true;

            return false;
        }

        /// <summary/>
        public static bool ValidRac(string rac)
        {
            if (phoneRac.IsMatch(rac))
                return true;

            return false;
        }

        /// <summary/>
        public static bool ValidMail(string mail)
        {
            if (filterMail.IsMatch(mail))
                return true;

            return false;
        }
    }
}
