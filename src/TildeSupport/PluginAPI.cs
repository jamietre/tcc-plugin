using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using RGiesecke.DllExport;
using TccPlugin;
using TccPlugin.Parser;
using TccPlugin.TakeCmd;

namespace TildeSupport
{
    public static class TildeSupportPlugin
    {

        static TildeSupportPlugin()
        {
            TccEventManager.Register(new PluginInfo
            {
                Author = "James Treworgy",
                Description = "Add tilde support on some commands",
                Email = "alien@outsharked.com",
                ModuleName = "?",
                WebSite = "https://github.com/jamietre/tcc-plugin"

            });
            
            Tcc = new TccCommands();
            Tcc.MapPath = Helpers.MapPath;

        }

        private static TccCommands Tcc;


        [DllExport]
        private static int InitializePlugin()
        {
            return TccEventManager.InitializePlugin();
        }

        [DllExport]
        private static IntPtr GetPluginInfo(IntPtr hModule)
        {
            return TccEventManager.GetPluginInfo(hModule);
        }

        [DllExport]
        private static int ShutdownPlugin(int bEndProcess)
        {
            return TccEventManager.ShutdownPlugin(bEndProcess);
        }

        [PluginMethod, DllExport]
        private static int key(IntPtr keyInfoPtr)
        {
            return TccEventManager.Key(keyInfoPtr);
        }

        [PluginMethod, DllExport]
        public static int UNKNOWN_CMD([MarshalAs(UnmanagedType.LPTStr)] StringBuilder sb)
        {
            return TccEventManager.UnknownCommand(sb);
        }



        /// <summary>
        /// Rewrites ~ to the home directory in CD. This is just an example.
        /// </summary>
        /// <param name="sb"></param>
        /// <returns></returns>
        [PluginMethod, DllExport]
        private unsafe static uint CD([MarshalAs(UnmanagedType.LPTStr)] StringBuilder sb)
        {
            return Tcc.ExecuteCmd(TccCommandName.CD, sb.ToString());
        }

        [PluginMethod, DllExport]
        public unsafe static uint DIR([MarshalAs(UnmanagedType.LPTStr)] StringBuilder sb)
        {
            return Helpers.MapCommands(sb);
        }

            


      
    }
}
