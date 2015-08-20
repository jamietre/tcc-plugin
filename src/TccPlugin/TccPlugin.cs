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


    public static class TccPlugin
    {

        public static void Register(PluginInfo clientPluginIngo) {
            ClientPluginInfo = clientPluginIngo;
        }

        private static PluginInfo ClientPluginInfo;

        private static UnmanagedData<TccPluginInfo> pluginInfo;

        /// <summary>
        /// Called when plugin is being shut down. if bEndProcess = 0, only the plugin is being closed
        /// </summary>
        /// <param name="bEndProcess"></param>
        /// <returns></returns>
        [IgnoreDllExport]
        [DllExport("ShutdownPlugin", CallingConvention = CallingConvention.Cdecl)]
        private static int ShutdownPlugin(int bEndProcess)
        {
#if DEBUG
            Console.WriteLine("Shutting down " + pluginInfo.Source.pszDll);
#endif
            pluginInfo.Dispose();

            return 0;
        }

        
        [IgnoreDllExport]
        [DllExport("GetPluginInfo", CallingConvention = CallingConvention.Cdecl)]
        private static IntPtr GetPluginInfo(IntPtr hModule)
        {
            // TODO: Abstract this into a class. Use reflection to generate pszFunctions

            TccPluginInfo piInfo = new TccPluginInfo(hModule, ClientPluginInfo);
            //piInfo.pszDll = "TccPlugin";
            //piInfo.pszAuthor = "James Treworgy";
            //piInfo.pszEmail = "alien@outsharked.com";
            //piInfo.pszWWW = "http://outsharked.com";
            //piInfo.pszDescription = "Plugin Demo";
            //piInfo.pszFunctions = "_hello,@rev,CD,DIR,*key";
            //piInfo.nMajor = 1;
            //piInfo.nMinor = 0;
            //piInfo.nBuild = 1;

            piInfo.pszFunctions += ",*key";

            pluginInfo = new UnmanagedData<TccPluginInfo>(piInfo);
            return pluginInfo.Pointer;
        }

        [IgnoreDllExport]
        [DllExport("InitializePlugin", CallingConvention = CallingConvention.Cdecl)]
        private static int InitializePlugin()
        {

#if DEBUG
            Console.WriteLine("TCC Plugin Initialized");
#endif
            return 0;
        }

        [IgnoreDllExport]
        [DllExport("UNKNOWN_CMD", CallingConvention = CallingConvention.Cdecl)]
        private static int UNKNOWN_CMD([MarshalAs(UnmanagedType.LPTStr)] StringBuilder sb)
        {
            return 0;
        }


        //[DllExport("PRE_INPUT", CallingConvention = CallingConvention.Cdecl)]
        //public static int PRE_INPUT([MarshalAs(UnmanagedType.LPTStr)] StringBuilder ignored)
        //{
        //    return 0;
        //}


        //[DllExport("PRE_EXEC", CallingConvention = CallingConvention.Cdecl)]
        //public static void PRE_EXEC([MarshalAs(UnmanagedType.LPTStr)] StringBuilder sb)
        //{
        //    sb.Clear();
        //    sb.Append("cd d:\\projects");
        //}

        //[DllExport("POST_EXEC", CallingConvention = CallingConvention.Cdecl)]
        //public static int POST_EXEC([MarshalAs(UnmanagedType.LPTStr)] StringBuilder ignored)
        //{
        //    return 0;
        //}

        [IgnoreDllExport]
        [DllExport("key", CallingConvention = CallingConvention.Cdecl)]
        private unsafe static int key(IntPtr keyInfoPtr)
        {
            var line = new CommandLine(keyInfoPtr);

            switch (line.KeyCode)
            {
                case 65655:
                    var text = "foo";

                    line.Insert(text);
                    line.Write();
                    
                    break;
            }

            return 0;

        }

    }
}
