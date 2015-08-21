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
            _Commands.Add(TccCommandName.CD, new TccCommand("CD", TccLib.Cd_Cmd));
            _Commands.Add(TccCommandName.CDD, new TccCommand("CDD", TccLib.Cdd_Cmd));
            _Commands.Add(TccCommandName.DIR, new TccCommand("DIR", TccLib.Dir_Cmd));
        }



    }
}
