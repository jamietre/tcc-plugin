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

namespace TccPlugin.Tests
{
    public static class MockPublicApi
    {

        [PluginMethod]
        [DllExport("ShutdownPlugin", CallingConvention = CallingConvention.Cdecl)]
        private static int ShutdownPlugin(int bEndProcess)
        {
            return 0;
        }

        [PluginMethod]
        [DllExport("GetPluginInfo", CallingConvention = CallingConvention.Cdecl)]
        private static IntPtr GetPluginInfo(IntPtr hModule)
        {
            return new IntPtr();
        }

        [PluginMethod]
        [DllExport("InitializePlugin", CallingConvention = CallingConvention.Cdecl)]
        private static int InitializePlugin()
        {
            return 0;
        }

        [DllExport("UNKNOWN_CMD", CallingConvention = CallingConvention.Cdecl)]
        private static int UNKNOWN_CMD([MarshalAs(UnmanagedType.LPTStr)] StringBuilder sb)
        {
            return 0;
        }

        [DllExport("key", CallingConvention = CallingConvention.Cdecl)]
        private static int key(IntPtr keyInfoPtr)
        {
            return 0;
        }

        /// <summary>
        /// Rewrites ~ to the home directory in CD. This is just an example.
        /// </summary>
        /// <param name="sb"></param>
        /// <returns></returns>
        [DllExport("CD", CallingConvention = CallingConvention.Cdecl)]
        private static uint CD([MarshalAs(UnmanagedType.LPTStr)] StringBuilder sb)
        {
            return 0;
        }

        [DllExport("DIR", CallingConvention = CallingConvention.Cdecl)]
        private static uint DIR([MarshalAs(UnmanagedType.LPTStr)] StringBuilder sb)
        {
            return 0;
        }

        [DllExport("f_testfunc", CallingConvention = CallingConvention.Cdecl)]
        private static uint f_testfunc([MarshalAs(UnmanagedType.LPTStr)] StringBuilder sb)
        {
            return 0;
        }

        [DllExport("_testvar", CallingConvention = CallingConvention.Cdecl)]
        private static uint _testvar([MarshalAs(UnmanagedType.LPTStr)] StringBuilder sb)
        {
            return 0;
        }
    }
}
