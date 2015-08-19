using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using TccPlugin.Parser;

namespace TccPlugin.TakeCmd
{
    public unsafe static class TakeCmdLib
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
        private static extern uint Cd_Cmd(char* command);

        [DllImport("TakeCmd.dll")]
        private static extern uint Cdd_Cmd(char* command);

        [DllImport("TakeCmd.dll")]
        private static extern uint Dir_Cmd(char* command);

        [DllImport("TakeCmd.dll", EntryPoint="ExpandVariables")]
        private static extern uint TC_ExpandVariables(char* text, int recurse);

        #endregion

        #region public methods

        /// <summary>
        /// Change directory
        /// </summary>
        /// <param name="path">The path to change to</param>
        public static void CD(string args)
        {
            exec(Cd_Cmd, args);
        }


        /// <summary>
        /// Change directory (and drive, if applicable). Replaces CDD and CD
        /// </summary>
        /// <param name="path">The path to change to</param>
        public static void CDD(string args)
        {
            exec(Cdd_Cmd, args);
        }

        /// <summary>
        /// Change directory (and drive, if applicable). Replaces CDD and CD
        /// </summary>
        /// <param name="path">The path to change to</param>
        public static void DIR(string args)
        {
            exec(Dir_Cmd, args);
        }

        /// <summary>
        /// Expand all variables, aliases and functions in a string
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ExpandVariables(string text)
        {
            char[] chars = new char[BUF_SIZE];
            text.CopyTo(0, chars,0,text.Length);

            fixed (char* textPtr = chars)
            {
                TC_ExpandVariables(textPtr, 0);
                // wierd thing with padding end with zeros if new one is shorter
                return new string(chars.Where(item => item != (char)0).ToArray());
            }
        }


        /// <summary>
        /// A default handler for mapping paths. Any method invoked involving a file path parameter
        /// should automatically use this to pre-process the path (if present)
        /// </summary>
        /// <returns></returns>
        public static Func<string, string> MapPath { 
            get; set;
        }
        
        #endregion

        #region private methods

        private delegate uint TCAction(char* command);

        private static uint exec(TCAction action, string text)
        {
            var args = ParseArgs(text);

            uint result;
            fixed (char* textPtr = args.ToString())
            {
                result = action(textPtr);
            }
            return result;
        }

        public static CommandLineArgs ParseArgs(StringBuilder sb)
        {
            return ParseArgs(sb.ToString());
        }

        public static CommandLineArgs ParseArgs(string commandLine)
        {
            var cmd = new CommandLineArgs(commandLine);

            if (MapPath != null) {
                foreach (var arg in cmd.Where(item=>!item.IsSwitch))
                {
                    arg.Value = MapPath(arg.Value);

                }
            }

            return cmd;

        }

        #endregion
    }
}
