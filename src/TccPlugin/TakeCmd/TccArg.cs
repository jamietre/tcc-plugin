using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using RGiesecke.DllExport;

namespace TccPlugin.TakeCmd
{
    
    public class TccArg
    {
        public TccArg(string @switch, bool required, string description)
        {
            Description = description;
            Required = required;
            Switch = @switch == "" ? null : @switch;
        }

        public string Description { get; protected set; }
        public string Switch { get; protected set; }
        public bool Required { get; protected set; }
    }
}
