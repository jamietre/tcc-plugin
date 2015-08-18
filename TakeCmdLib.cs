using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace TccPlugin
{
    public unsafe static class TakeCmdLib
    {
        #region constants

        /// <summary>
        /// Command line buffer size
        /// </summary>
        public const int BUF_SIZE = 512;

        /// <summary>
        /// Special return value that says continue to process same named command. You can use this to rewrite a 
        /// command's arguments and simply defer it to the builtin command (or other overrides)
        /// </summary>
        public const uint RETURN_DEFER = 0xFEDCBA98;

        #endregion

        #region TakeCmd.dll API

        [DllImport("TakeCmd.dll")]
        private static extern uint Cd_Cmd(char* command);

        [DllImport("TakeCmd.dll")]
        private static extern uint Cdd_Cmd(char* command);

        [DllImport("TakeCmd.dll", EntryPoint="ExpandVariables")]
        private static extern uint TC_ExpandVariables(char* text, int recurse);

        #endregion

        #region public methods

        /// <summary>
        /// Change directory
        /// </summary>
        /// <param name="path">The path to change to</param>
        public static void CD(string path)
        {
            exec(Cd_Cmd, path);
        }


        /// <summary>
        /// Change directory (and drive, if applicable). Replaces CDD and CD
        /// </summary>
        /// <param name="path">The path to change to</param>
        public static void CDD(string path)
        {
            exec(Cdd_Cmd, path);
        }

        /// <summary>
        /// Expand all variables, aliases and functions in a string
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ExpandVariables(string text)
        {
            text = text.PadRight(BUF_SIZE);
            
            fixed (char* textPtr = text)
            {
                TC_ExpandVariables(textPtr, 0);
                return text;
            }
        }
        #endregion

        #region private methods

        private delegate uint TCAction(char* command);

        private static uint exec(TCAction action, string text)
        {
            uint result;
            fixed (char* textPtr = text)
            {
                result = action(textPtr);
            }
            return result;
        }

        #endregion
    }
}
