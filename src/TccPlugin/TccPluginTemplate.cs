/*
 * Template for the TCC plugin. This has to be in the primary DLL; 
 * we can't IL merge either because it's not managed code.
 * Copy these methods into your main entry file.
 * 
 * TODO: Use T4 codegen to deal with this
 * */

#if NOCOMPILE
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

    public static class TccPluginTemplate
    {
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

    }
}
#endif