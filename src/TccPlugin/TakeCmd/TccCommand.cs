using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TccPlugin.TakeCmd
{
    /// <summary>
    /// Spec that describes a command's arguments
    /// </summary>
    public class TccCommand
    {
        public TccCommand(string name, TccLib.TCAction winApiCmd/*, IEnumerable<TccArg> arguments*/)
        {
            Name = name;
            /*Arguments = new List<TccArg>(arguments).AsReadOnly();*/
            WinApiCmd = winApiCmd;
        }

        public string Name { get; private set; }
        /*public IReadOnlyList<TccArg> Arguments { get; private set; }*/
        public TccLib.TCAction WinApiCmd { get; private set; }
    }
}
