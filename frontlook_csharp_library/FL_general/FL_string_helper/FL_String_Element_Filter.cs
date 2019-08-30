using System.Text;

namespace frontlook_csharp_library.FL_general.FL_string_helper
{
    /// <summary>
    /// String Maintainer
    /// </summary>

    /// <summary>
    /// Filters string.
    /// </summary>
    public class FL_String_Element_Filter
    {
        /// <summary>
        /// Filters and removes only special characters.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FL_remove_special_char(string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Accepts only digits
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FL_acceptonlydigit(string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9'))
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Accepts only alphabets
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FL_acceptonlyalphabet(string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z'))
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Accepts alphabet and digit.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FL_acceptAlphabetAndNumber(string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z'))
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Accepts Alphabet Number Point and Underscore
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FL_acceptAlphabetNumberPointUnderscore(string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Accepts number and point
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FL_acceptNumberAndPoint(string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || c == '.')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Accepts Emailaddress Parameters
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FL_acceptEmailAddress(string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' ||
                    c == '_' || c == '@')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
    }
}