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
        public const int BUF_SIZE = 511;

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

        [DllImport("TakeCmd.dll", EntryPoint="Command")]
        internal static extern uint TC_Command(char* text, int reserved);

        [DllImport("TakeCmd.dll", EntryPoint = "Start_Cmd")]
        internal static extern uint Start_Command(char* command);

        [DllImport("TakeCmd.dll", EntryPoint = "Set_Cmd")]
        internal static extern uint Set_Command(char* command);
        
        [DllImport("TakeCmd.dll", EntryPoint = "MakeFullName")]
        internal static extern char* TC_MakeFullName(char* pszFileName, uint fFlags);
        
        [DllImport("TakeCmd.dll", EntryPoint="wwriteXP")]
        internal static extern uint TC_wwriteXP(IntPtr hFile, char* pszString, uint nLength);

        [DllImport("TakeCmd.dll", EntryPoint="QPuts")]
        internal static extern uint TC_QPuts(char* pszString);
        #endregion

        #region private methods

        public delegate uint TCAction(char* command);
        public delegate uint TCAction2(char* command, int parm);
        
        #endregion
    }
}
