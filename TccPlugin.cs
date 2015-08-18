using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using RGiesecke.DllExport;


namespace TccPlugin
{


    public static class TccPlugin
    {
        [DllImport("msvcrt.dll",  SetLastError = false)]
        static extern IntPtr memcpy(IntPtr dest, IntPtr src, int count);

        private static UnmanagedData<PluginInfo> pluginInfo;

        /// <summary>
        /// Called when plugin is being shut down. if bEndProcess = 0, only the plugin is being closed
        /// </summary>
        /// <param name="bEndProcess"></param>
        /// <returns></returns>
        [DllExport("ShutdownPlugin", CallingConvention = CallingConvention.Cdecl)]
        public static int ShutdownPlugin(int bEndProcess)
        {
#if DEBUG
            Console.WriteLine("Shutting down " + pluginInfo.Source.pszDll);
#endif
            pluginInfo.Dispose();

            return 0;
        }

        

        [DllExport("GetPluginInfo", CallingConvention = CallingConvention.Cdecl)]
        public static IntPtr GetPluginInfo(IntPtr hModule)
        {
            // TODO: Abstract this into a class. Use reflection to generate pszFunctions

            PluginInfo piInfo = new PluginInfo();
            piInfo.pszDll = "TccPlugin";
            piInfo.pszAuthor = "James Treworgy";
            piInfo.pszEmail = "alien@outsharked.com";
            piInfo.pszWWW = "http://outsharked.com";
            piInfo.pszDescription = "Plugin Demo";
            piInfo.pszFunctions = "_hello,@rev,UNKNOWN_CMD,CD,*key";
            piInfo.nMajor = 1;
            piInfo.nMinor = 0;
            piInfo.nBuild = 1;

            pluginInfo = new UnmanagedData<PluginInfo>(piInfo);
            return pluginInfo.Pointer;
        }

        [DllExport("InitializePlugin", CallingConvention = CallingConvention.Cdecl)]
        public static int InitializePlugin()
        {
#if DEBUG
            Console.WriteLine("Initialized");
#endif
            return 0;
        }

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
            string path = sb.ToString().Replace(" ~", "%HOMEDRIVE%%HOMEPATH%");
            var expanded = TakeCmdLib.ExpandVariables(path);

            TakeCmdLib.CDD(expanded);
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


        [DllExport("key", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static int key(IntPtr keyInfoPtr)
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

        //[TccMethod("rev", TccMethodTypes.Function)]
        [DllExport("f_rev", CallingConvention = CallingConvention.Cdecl)]
        public static int f_rev([MarshalAs(UnmanagedType.LPTStr)] StringBuilder sb)
        {
            if (sb == null)
            {
                return 1;
            }

            var reverse = sb.ToString().Reverse().ToArray();
            sb.Clear();
            sb.Append(reverse);
            

            return 0;
        }

        //[TccMethod("hello", TccMethodTypes.InternalVariable)]
        [DllExport("_hello", CallingConvention = CallingConvention.Cdecl)]
        public static int _hello([MarshalAs(UnmanagedType.LPTStr)] StringBuilder sb)
        {
            if (sb == null)
            {
                return 1;
            }

            sb.Append("Hello world! This is an internal variable.");
            
            return 0;
        }

    }
}
