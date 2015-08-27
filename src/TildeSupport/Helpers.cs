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
                path = TccCommands.ExpandVariables("%HOMEDRIVE%%HOMEPATH%") + path.Substring(1);
            }

            path = FixSlashes(path);

            if (path[0] == '\\' && path.Length > 1 && IsAlpha(path[1]) &&
                (path.Length == 2 || (path.Length > 2 && path[2] == '\\'))) {
                    path = path[1] + ":\\" + (path.Length > 3 ?
                        path.Substring(3) :
                        "");
            }

            return path;
        }

        public static string FixSlashes(string path)
        {
            return path.Replace("/", "\\");
        }
        private static bool IsAlpha(char c) {
            return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z');
        }
    }

   
}