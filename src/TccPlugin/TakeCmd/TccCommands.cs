using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TccPlugin.TakeCmd
{
 
    /// <summary>
    /// 
    /// </summary>
    public unsafe class TccCommands
    {
        private TccCommandExecutor CmdExecutor = new TccCommandExecutor();
        
        /// <summary>
        /// Execute a command on the Tcc API
        /// </summary>
        /// <param name="name"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public uint ExecuteCmd(TccCommandName name, string commandText)
        {
            var cmd = TccCommandRepo.Get(name);
            return CmdExecutor.Execute(cmd.WinApiCmd, commandText);
        }

        /// <summary>
        /// Run an arbitrary command
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public uint Command(string text)
        {
            return CmdExecutor.Execute(TccLib.TC_Command, text, 0);
        }

        /// <summary>
        /// Run an arbitrary command
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public uint Command(params string[] args)
        {

            string command = String.Join(" ", args.Select(item =>
            {
                return item.IndexOf(" ") >= 0 ? "\"" + item + "\"" : item;
            }));

            return CmdExecutor.Execute(TccLib.TC_Command, command, 0);
        }


        /// <summary>
        /// Expand all variables, aliases and functions in a string
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ExpandVariables(string text)
        {
            char[] chars = new char[TccLib.BUF_SIZE];
            text.CopyTo(0, chars, 0, text.Length);

            fixed (char* textPtr = chars)
            {
                TccLib.TC_ExpandVariables(textPtr, 0);

                return chars.ToStringSafe();
            }
        }

        /// <summary>
        /// A default handler for mapping paths. Any method invoked involving a file path parameter
        /// should automatically use this to pre-process the path (if present)
        /// </summary>
        /// <returns></returns>
        public Func<string, string> MapPath
        {
            get
            {
                return CmdExecutor.MapPath;
            }
            set
            {
                CmdExecutor.MapPath = value;
            }
        }
    }
}
