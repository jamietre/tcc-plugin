using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TccPlugin.Parser;

namespace TccPlugin.TakeCmd
{
    public unsafe class TccCommandExecutor
    {
        /// <summary>
        /// A default handler for mapping paths. Any method invoked involving a file path parameter
        /// should automatically use this to pre-process the path (if present)
        /// </summary>
        /// <returns></returns>
        public Func<string, string> MapPath
        {
            get;
            set;
        }

        public uint Execute(TccLib.TCAction action, string text)
        {
            var args = ParseArgs(text);

            uint result;
            fixed (char* textPtr = args.ToString())
            {
                result = action(textPtr);
            }

            return result;
        }

        public CommandLineArgs ParseArgs(StringBuilder sb)
        {
            return ParseArgs(sb.ToString());
        }

        public CommandLineArgs ParseArgs(string commandLine)
        {
            var cmd = new CommandLineArgs(commandLine);

            if (MapPath != null)
            {
                foreach (var arg in cmd.Where(item => !item.IsSwitch))
                {
                    arg.Value = MapPath(arg.Value);

                }
            }

            return cmd;

        }
    }
}
