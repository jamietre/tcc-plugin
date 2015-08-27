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
        public TccCommand(string name, TccLib.TCAction winApiCmd): this(name, winApiCmd, null)
        {
            CreatedWithoutOptions = true;
        }

        /// <summary>
        /// Create a new TccCommand spec, defining all options available. When this method is used, 
        /// the parser can automatically resolve paths starting with a forward slash.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="winApiCmd"></param>
        /// <param name="options"></param>
        public TccCommand(string name, TccLib.TCAction winApiCmd, IEnumerable<TccArg> options)
        {
            Name = name.ToUpper();
            var defaultOpts = EnumerableHelper.Enumerate(new TccArg("?"));

            _Options = new HashSet<TccArg>(options.Concat(defaultOpts) ?? defaultOpts);
            WinApiCmd = winApiCmd;
        }

        private HashSet<TccArg> _Options;

        /// <summary>
        /// The command name
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// All valid option flags
        /// </summary>
        public IEnumerable<TccArg> Options
        {
            get
            {
                return _Options.AsEnumerable();
            }
        }

        /// <summary>
        /// The internal API call on the Tcc DLL for this command
        /// </summary>
        public TccLib.TCAction WinApiCmd { get; private set; }

        /// <summary>
        /// Test whether an option is defined on this command
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public bool HasOption(string option, bool isFlag)
        {
            return _Options.Contains(new TccArg(option, !isFlag));
        }

        /// <summary>
        /// When true, the options are well defined for this command and can be used to determine forward-slash intent
        /// </summary>
        public bool IsOptionsDefined
        {
            get {
                return !CreatedWithoutOptions;
            }
        }

        private bool CreatedWithoutOptions { get; set; }
    }
}
