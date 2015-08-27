using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TccPlugin.TakeCmd
{
 
    public unsafe static class TccCommandRepo
    {

        private static Dictionary<TccCommandName, TccCommand> _Commands;
        private static Dictionary<TccCommandName, TccCommand> Commands
        {
            get
            {
                if (_Commands == null)
                {
                    PopulateCommands();
                }
                return _Commands;
            }
        }

        public static TccCommand Get(TccCommandName name) {
            return Commands[name];
        }

    
        private static void PopulateCommands() {
            _Commands = new Dictionary<TccCommandName, TccCommand>();

            // Note that for CD we purposely don't expose the /D option, since it conflicts with 
            // using /D as drive D, and is automatically used anyway
            _Commands.Add(TccCommandName.CD, new TccCommand("CD", TccLib.Cd_Cmd,
                EnumerableHelper.Enumerate(
                    new TccArg("N"),
                    new TccArg("X")
            )));

            _Commands.Add(TccCommandName.CDD, new TccCommand("CDD", TccLib.Cdd_Cmd,
                EnumerableHelper.Enumerate(
                    new TccArg("A"),
                    new TccArg("D", true),
                    new TccArg("N", true),
                    new TccArg("S", true),
                    new TccArg("U", true),
                    new TccArg("X", true),
                    new TccArg("T"),
                    new TccArg("TO")
                )
            ));
            _Commands.Add(TccCommandName.DIR, new TccCommand("DIR", TccLib.Dir_Cmd, 
                EnumerableHelper.Enumerate(
                    new TccArg("1"),
                    new TccArg("2")
                )));
        }



    }
}
