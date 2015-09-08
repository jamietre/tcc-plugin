using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TccPlugin.Parser;

namespace TccPlugin.TakeCmd
{
 
    /// <summary>
    /// 
    /// </summary>
    public unsafe class TccCommands
    {
        private TccCommandExecutor CmdExecutor = new TccCommandExecutor();

        /// <summary>
        /// Process and rewrite a command line with path mapping, etc
        /// </summary>
        /// <param name="name"></param>
        /// <param name="commandArgs"></param>
        /// <returns></returns>
        public uint ProcessCmd(TccCommandName name, StringBuilder commandArgs)
        {
            return ProcessCmd(name, commandArgs, null);
        }

        /// <summary>
        /// Process and rewrite a command line with path mapping, etc, but call back with commandLineArgs object 
        /// for further modification
        /// </summary>
        /// <param name="name"></param>
        /// <param name="commandArgs"></param>
        /// <param name="commandLineArgs"></param>
        /// <returns></returns>
        public uint ProcessCmd(TccCommandName name, StringBuilder commandArgs, Action<CommandLineArgs> commandLine)
        {
            try
            {
                var cmd = TccCommandRepo.Get(name);
                var commandLineArgs = ParseArgs(cmd, commandArgs.ToString());
                if (commandLine != null)
                {
                    commandLine(commandLineArgs);
                }

                commandArgs.Replace(" " + commandLineArgs.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return 1;
            }

            return TccLib.RETURN_DEFER;
        }
        
        
        /// <summary>
        /// Execute a command on the Tcc API.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public uint ExecuteCmd(TccCommandName name, string commandArgs)
        {
            TccCommand cmd;
            try
            {
                cmd = TccCommandRepo.Get(name);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return 1;
            }

            return ExecuteCmd(cmd, ParseArgs(cmd, commandArgs));
        }


        /// <summary>
        /// Execute a command on the Tcc API. If the command line arguments include /?, it will be deferred
        /// to the generic command line executor, as this seems to be pre-processed by TCC natively.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public uint ExecuteCmd(TccCommand command, CommandLineArgs commandLineArgs)
        {
            try
            {
                if (commandLineArgs.Any(item => item.IsFlag && item.Option == "?"))
                {
                    return Command(command.Name + " " + commandLineArgs.ToString());
                }


                return CmdExecutor.Execute(command.WinApiCmd, commandLineArgs.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return 1;
            }
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
            var chars = GetBuffer(text);

            fixed (char* textPtr = chars)
            {
                TccLib.TC_ExpandVariables(textPtr, 0);

                return chars.ToStringSafe();
            }
        }

        /// <summary>
        /// Take a file name and expand to full path
        /// </summary>
        public static string MakeFullName(string fileName)
        {
            var chars = GetBuffer(fileName);

            string fullPath;
            fixed (char* textPtr = chars)
            {
                fullPath = new string(TccLib.TC_MakeFullName(textPtr, 0));
            }
            return fullPath;
        }

        /// <summary>
        /// A default handler for mapping paths. Any method invoked involving a file path parameter
        /// should automatically use this to pre-process the path (if present)
        /// </summary>
        /// <returns></returns>
        public Func<string, string> MapPath
        {
            get; set;
        }

        /// <summary>
        /// Transform a command line string into CommandLineArgs, applying MapPath
        /// </summary>
        /// <param name="commandLine"></param>
        /// <returns></returns>
        public CommandLineArgs ParseArgs(TccCommand tccCommand, string commandLine)
        {
            var cmd = new CommandLineArgs(commandLine);

            if (MapPath != null)
            {
                foreach (var arg in cmd)
                {
                    ParseArg(tccCommand, arg);
                    
                }
            }

            return cmd;
        }

        private void ParseArg(TccCommand command, CommandLineArg argument)
        {
            // only parse things which we are sure aren't options; either by 
            // nature of it not having a slash, or if it starts with a slash,
            // then by see if it matches a drive letter. Note that this could conflict
            // with options

            if (!command.IsOptionsDefined) {
                return;
            }

            if (argument.IsOption && command.HasOption(argument.Option, argument.IsFlag)) {
                return;
            }

            argument.Value = MapPath(argument.ToString());
            argument.IsOption=false;

        }

        /// <summary>
        /// Get a buffer for passing strings to TCCLIB
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static char[] GetBuffer(string text)
        {
            char[] chars = new char[TccLib.BUF_SIZE];
            text.CopyTo(0, chars, 0, text.Length);
            return chars;
        }
    }
}
