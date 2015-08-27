using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TildeSupport
{
    private class Configuration
    {
        /// <summary>
        /// Map editors to a launcher path
        /// </summary>
        public IDictionary<string, string> Launchers;
        /// <summary>
        /// Map file extensions to editors
        /// </summary>
        public IDictionary<string, string> Extensions;
    }
}
