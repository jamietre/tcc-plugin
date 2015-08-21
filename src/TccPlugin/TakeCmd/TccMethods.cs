using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TccPlugin.TakeCmd
{
    /// <summary>
    /// Facade for the TccLib non-command methods
    /// </summary>
    public unsafe static class TccMethods
    {
        /// <summary>
        /// Expand all variables, aliases and functions in a string
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ExpandVariables(string text)
        {
            char[] chars = new char[TccLib.BUF_SIZE];
            text.CopyTo(0, chars,0,text.Length);

            fixed (char* textPtr = chars)
            {
                TccLib.TC_ExpandVariables(textPtr, 0);

                return chars.ToStringSafe();
            }
        }
    }
}
