using System;

// ReSharper disable Checkstring namespace

namespace frontlook_csharp_library.FL_General.FL_string_helper
{
    /// <summary>
    /// String Maintainer
    /// </summary>
    
    /// <summary>
    /// String Validator
    /// </summary>
    public class FL_String_Validator
    {
        /// <summary>
        /// Validates all but special characters
        /// </summary>
        /// <param string name="str"></param>
        /// <returns></returns>
        public static Boolean FL_notval_special_char(string str)
        {
            Boolean a = true;
            foreach (char c in str)
            {
                /*if (!((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' ||
                    c == '_'))
                {
                    
                }*/
                Boolean b = (a && !((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_'));
                a = b;
            }
            return a;
        }
        /// <summary>
        /// Validates only digit
        /// </summary>
        /// <param string name="str"></param>
        /// <returns></returns>
        public static Boolean FL_val_onlydigit(string str)
        {
            Boolean a = true;
            foreach (char c in str)
            {
                Boolean b = (a && !(c >= '0' && c <= '9'));
                a = b;
            }
            return a;
        }
        /// <summary>
        /// Validates only alphabet
        /// </summary>
        /// <param string name="str"></param>
        /// <returns></returns>
        public static Boolean FL_val_onlyalphabet(string str)
        {
            Boolean a = true;
            foreach (char c in str)
            {
                /*if (!(c >= 'A' && c <= 'Z') || !(c >= 'a' && c <= 'z'))
                {
                    
                }*/
                Boolean b = (a && !((c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z')));
                a = b;
            }
            return a;
        }
        /// <summary>
        /// Validates alphabet and digit
        /// </summary>
        /// <param string name="str"></param>
        /// <returns></returns>
        public static Boolean FL_val_AlphabetAndDigit(string str)
        {
            Boolean a = true;
            foreach (char c in str)
            {
                Boolean b = (a && !((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z')));
                a = b;
            }
            return a;
        }
        /// <summary>
        /// Validates Alphabet Number Point and Underscore
        /// </summary>
        /// <param string name="str"></param>
        /// <returns></returns>
        public static Boolean FL_val_AlphabetNumberPointUnderscore(string str)
        {
            Boolean a = true;
            foreach (char c in str)
            {
                Boolean b = (a && !((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_'));
                a = b;
            }
            return a;
        }
        /// <summary>
        /// Validates number and point
        /// </summary>
        /// <param string name="str"></param>
        /// <returns></returns>
        public static Boolean FL_val_NumberAndPoint(string str)
        {
            Boolean a = true;
            foreach (char c in str)
            {
                Boolean b = (a && !((c >= '0' && c <= '9') || c == '.'));
                a = b;
            }
            return a;
        }
        /// <summary>
        /// Accepts Emailaddress Parameters
        /// </summary>
        /// <param string name="str"></param>
        /// <returns></returns>
        public static Boolean FL_val_EmailAddress(string str)
        {
            Boolean a = true;
            foreach (char c in str)
            {
                Boolean b = (a && !((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_' || c == '@'));
                a = b;
            }
            return a;
        }
    }
}
