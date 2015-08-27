using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TccPlugin.Parser
{
    /// <summary>
    /// A command line argument
    /// </summary>
    public class CommandLineArg
    {
        public static CommandLineArg Clone(CommandLineArg arg)
        {
            var newArg = new CommandLineArg
            {
                IsOption = arg.IsOption,
                Option = arg.Option,
                Value = arg.Value
            };
            return newArg;
        }
        private CommandLineArg() { }

        public CommandLineArg(string arg)
        {
            if (String.IsNullOrEmpty(arg)) {
                throw new ArgumentException("Can't create a null argument");
            }

            // Any argument must be non-null; if value is null it always means that it was unassigned
            // as a result of a value-less option and we treat it as a flag

            if (arg[0] == '/' && arg.Length > 1)
            {
                IsOption = true;
                string[] parts = arg.Substring(1).Split(':');

                Option = parts[0].ToUpperInvariant();
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

        public static implicit operator CommandLineArg(string arg)
        {
            return new CommandLineArg(arg);
        }

        /// <summary>
        /// When true, this is a switch type argument
        /// </summary>
        public bool IsOption { get; set; }

        /// <summary>
        /// When true, the option was called as a flag (e.g. with no value)
        /// </summary>

        public bool IsFlag
        {
            get
            {
                return Value == null;
            }
        }
        /// <summary>
        /// When this is a flag, contains the name of the switch
        /// </summary>
        public string Option { get; set; }
        /// <summary>
        /// The argument or switch value
        /// </summary>
        public string Value { get; set; }

        public override string ToString()
        {
            return String.Format("{0}{1}{2}",
                IsOption ? "/" + Option : "",
                !String.IsNullOrEmpty(Value) && IsOption ? ":" : "",
                Value);
        
        }
    }
}
