using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TccPlugin.Parser
{
    public class CommandLineArg
    {
        public CommandLineArg(string arg)
        {
            if (String.IsNullOrEmpty(arg)) {
                throw new ArgumentException("Can't create a null argument");
            }

            if (arg[0] == '/')
            {
                IsSwitch = true;
                string[] parts = arg.Split(':');

                Switch = parts[0];
                if (parts.Length > 1)
                {
                    Value = parts[1];
                }
            }
            else
            {
                Value = arg;
            }
        }
        /// <summary>
        /// When true, this is a switch type argument
        /// </summary>
        public bool IsSwitch { get; set; }
        /// <summary>
        /// When this is a flag, contains the name of the switch
        /// </summary>
        public string Switch { get; set; }
        /// <summary>
        /// The argument or switch value
        /// </summary>
        public string Value { get; set; }

        public override string ToString()
        {
            return String.Format("{0}{1}{2}",
                IsSwitch ? "/" + Switch : "",
                !String.IsNullOrEmpty(Value) && IsSwitch ? ":" : "",
                Value);
        
        }
    }
}
