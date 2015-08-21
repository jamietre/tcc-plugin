using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TccPlugin.Parser;
using TccPlugin.TakeCmd;
using TccPlugin;

namespace TildeSupport
{
    public static class Helpers
    {
        /// <summary>
        /// Map each command path using the MapPath command to replace ~ with current path
        /// </summary>
        /// <param name="sb"></param>
        public static uint MapCommands(StringBuilder sb)
        {
            sb.Replace(" " + CommandLineArgs.Map(sb.ToString(), MapPath).ToString());
            return TccLib.RETURN_DEFER;
        }

        public static string MapPath(string path)
        {
            if (path.StartsWith("~"))
            {
                path = TccMethods.ExpandVariables("%HOMEDRIVE%%HOMEPATH%") + path.Substring(1);
            }
            path = path.Replace("/", "\\");

            return path;

        }
    }
}
