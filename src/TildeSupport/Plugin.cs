using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using RGiesecke.DllExport;
using TccPlugin.Parser;
using TccPlugin.TakeCmd;

namespace TccPlugin
{


    public static class TildeSupportPlugin
    {

        static TildeSupportPlugin()
        {
            TccPlugin.Register(new PluginInfo
            {
                 Author = "James Treworgy",
                 Description = "Add tilde support on some commands",
                 Email = "alien@outsharked.com",
                 ModuleName = "?",
                 WebSite = "https://github.com/jamietre/tcc-plugin"

            });

            Tcc = new TccCommands();
            Tcc.MapPath = MapPath;

        }

        private static TccCommands Tcc;
      


        [DllExport("UNKNOWN_CMD", CallingConvention = CallingConvention.Cdecl)]
        public static int UNKNOWN_CMD([MarshalAs(UnmanagedType.LPTStr)] StringBuilder sb)
        {
            return 0;
        }

        /// <summary>
        /// Rewrites ~ to the home directory in CD. This is just an example.
        /// </summary>
        /// <param name="sb"></param>
        /// <returns></returns>
        [DllExport("CD", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static uint CD([MarshalAs(UnmanagedType.LPTStr)] StringBuilder sb)
        {
            return Tcc.ExecuteCmd(TccCommandName.CD, sb.ToString());
        }

        [DllExport("DIR", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static uint DIR([MarshalAs(UnmanagedType.LPTStr)] StringBuilder sb)
        {
            return MapCommands(sb);
        }


        /// <summary>
        /// Map each command path using the MapPath command to replace ~ with current path
        /// </summary>
        /// <param name="sb"></param>
        private static uint MapCommands(this StringBuilder sb)
        {
            sb.Replace(" " + CommandLineArgs.Map(sb.ToString(), MapPath).ToString());
            return TccLib.RETURN_DEFER;
        }

        private static string MapPath(string path)
        {
            if (path.StartsWith("~"))
            {
                path = Tcc.ExpandVariables("%HOMEDRIVE%%HOMEPATH%") + path.Substring(1);
            }
            path = path.Replace("/", "\\");

            return path;

        }
    }
}
