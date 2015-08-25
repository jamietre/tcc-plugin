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

            Loader = new ExternalLoader(Tcc);
        }

        private static TccCommands Tcc;
        private static ExternalLoader Loader;

        #region TCC API

        [DllExport]
        private static uint InitializePlugin()
        {
            return TccEventManager.InitializePlugin();
        }

        [DllExport]
        private static IntPtr GetPluginInfo(IntPtr hModule)
        {
            return TccEventManager.GetPluginInfo(hModule);
        }

        [DllExport]
        private static uint ShutdownPlugin(int bEndProcess)
        {
            return TccEventManager.ShutdownPlugin(bEndProcess);
        }

        [PluginMethod, DllExport]
        private static uint key(IntPtr keyInfoPtr)
        {
            return TccEventManager.Key(keyInfoPtr);
        }

        [PluginMethod, DllExport]
        public static uint UNKNOWN_CMD([MarshalAs(UnmanagedType.LPTStr)] StringBuilder sb)
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

        [PluginMethod, DllExport]
        public unsafe static uint EDIT([MarshalAs(UnmanagedType.LPTStr)] StringBuilder sb)
        {
            var loader = new ExternalLoader(Tcc);
            loader.Load("edit", sb.ToStringTrimmed());
            return 0;
        }

        [PluginMethod, DllExport]
        public unsafe static uint EXPLORE([MarshalAs(UnmanagedType.LPTStr)] StringBuilder sb)
        {
            var loader = new ExternalLoader(Tcc);
            loader.Explorer(sb.ToStringTrimmed());
            return 0;
        }

        #endregion

    }
}
