using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using TccPlugin.Parser;

namespace TccPlugin.TakeCmd
{
    public unsafe static class TccLib
    {
        #region constants

        /// <summary>
        /// Command line buffer size
        /// </summary>
        public const int BUF_SIZE = 508;

        /// <summary>
        /// Special return value that says continue to process same named command. You can use this to rewrite a 
        /// command's arguments and simply defer it to the builtin command (or other overrides)
        /// </summary>
        public const uint RETURN_DEFER = 0xFEDCBA98;

        #endregion

        #region TakeCmd.dll API

        [DllImport("TakeCmd.dll")]
        internal static extern uint Cd_Cmd(char* command);

        [DllImport("TakeCmd.dll")]
        internal static extern uint Cdd_Cmd(char* command);

        [DllImport("TakeCmd.dll")]
        internal static extern uint Dir_Cmd(char* command);

        [DllImport("TakeCmd.dll", EntryPoint="ExpandVariables")]
        internal static extern uint TC_ExpandVariables(char* text, int recurse);

        #endregion

        #region private methods

        public delegate uint TCAction(char* command);

        
        #endregion
    }
}
