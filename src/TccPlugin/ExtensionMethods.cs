using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TccPlugin.Parser;

namespace TccPlugin
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Return a string from a character array, ignoring trailing zero values
        /// </summary>
        /// <param name="chars"></param>
        /// <returns></returns>
        public static string ToStringSafe(this char[] chars)
        {
            StringBuilder sb = new StringBuilder();
            int pos = 0;
            while (pos < chars.Length && chars[pos] != '\0')
            {
                sb.Append(chars[pos++]);
            }
            return sb.ToString();
        }

        public static StringBuilder Replace(this StringBuilder sb, string text)
        {
            sb.Clear();
            sb.Append(text);
            return sb;
        }

        public static string ToStringTrimmed(this StringBuilder sb)
        {
            return sb.ToString().Trim();
        }

        //public static unsafe string CharPtrToString(char* text)
        //{
        //    var sb = new StringBuilder();
        //    do
        //    {

        //    }
        //    while (text++!='\0');
        //}
        
        
    }
}
