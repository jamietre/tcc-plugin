using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TccPlugin.Parser
{
    public class CommandLineArgs: IEnumerable<CommandLineArg>
    {
        /// <summary>
        /// Map a command line, passing each argument 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static CommandLineArgs Map(string args, Func<CommandLineArg, string> transform)
        {
            var clArgs = new CommandLineArgs(args);
            return new CommandLineArgs(clArgs.Select(item =>
            {
                var newArg = CommandLineArg.Clone(item);
                newArg.Value = transform(item);
                return newArg;
            }));
        }
        /// <summary>
        /// Map a command line, passing just the value of each argument
        /// </summary>
        /// <param name="args"></param>
        /// <param name="transform"></param>
        /// <returns></returns>
        public static CommandLineArgs Map(string args, Func<string, string> transform)
        {
            var clArgs = new CommandLineArgs(args);
            return new CommandLineArgs(clArgs.Select(item => {
                var newArg = CommandLineArg.Clone(item);
                newArg.Value = transform(item.Value);
                return newArg;
            }));
        }
        
        public CommandLineArgs(IEnumerable<CommandLineArg> args) {
            Args = new List<CommandLineArg>(args);
        }
            
        public CommandLineArgs(string text)
        {
            
            if (!String.IsNullOrEmpty(text))
            {
                var parts = text.Trim().Split(' ');

                Args = new List<CommandLineArg>(parts.Select(item=>new CommandLineArg(item)));
            }
            else
            {
                Args = new List<CommandLineArg>();
            }
        }

        private List<CommandLineArg> Args;

        public override string ToString()
        {

            return Args.Count > 0 ?
                String.Join(" ", Args.Select(item => item.ToString())) :
                "";
        }

        public IEnumerator<CommandLineArg> GetEnumerator()
        {
            return Args.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
