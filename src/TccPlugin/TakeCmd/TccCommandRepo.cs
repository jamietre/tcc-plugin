using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TccPlugin.TakeCmd
{
 
    public unsafe static class TccCommandRepo
    {

        private static Dictionary<TccCommandName, TccCommand> Commands;

        static TccCommandRepo() {
            Commands = new Dictionary<TccCommandName, TccCommand>();
            PopulateCommands();
        }

        public static TccCommand Get(TccCommandName name) {
            return Commands[name];
        }

    
        private static void PopulateCommands() {
            Commands.Add(TccCommandName.CD, new TccCommand("CD", TccLib.Cd_Cmd));
            Commands.Add(TccCommandName.CD, new TccCommand("CDD", TccLib.Cdd_Cmd));
            Commands.Add(TccCommandName.CD, new TccCommand("DIR", TccLib.Dir_Cmd));
        }



    }
}
