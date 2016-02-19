using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace TccPlugin.TakeCmd
{
    
    public class TccArg
    {
        /// <summary>
        /// Create a TccArg with a switch-type option, e.g. having no value
        /// </summary>
        /// <param name="option"></param>
        public TccArg(string option): this(option, false)
        {

        }

        public TccArg(string option, bool hasValue)
        {

            //Required = required;
            HasValue = HasValue;
            Option = option == "" ? null : option.ToUpperInvariant();
        }

        public string Option { get; protected set; }
        public bool HasValue { get; protected set; }
       // public bool Required { get; protected set; }

        public override int GetHashCode()
        {
            return Option.GetHashCode() * (HasValue ? 7 : 1);
        }

        public override bool Equals(object obj)
        {
            var arg = obj as TccArg;
            return arg != null &&
                arg.Option == this.Option &&
                arg.HasValue == this.HasValue;
        }
    }
}
