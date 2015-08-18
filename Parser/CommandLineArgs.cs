using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TccPlugin.Parser
{
    public class CommandLineArgs: IEnumerable<CommandLineArg>
    {
        public CommandLineArgs(string text)
        {
            Args = new List<CommandLineArg>();
            if (!String.IsNullOrEmpty(text))
            {
                var parts = text.Trim().Split(' ');

             
                foreach (var part in parts)
                {
                    Args.Add(new CommandLineArg(part));
                }
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
