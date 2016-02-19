using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using TccPlugin;
using TccPlugin.Parser;
using TccPlugin.TakeCmd;

namespace TccPlugin.Tests
{
    public static class MockPublicApi
    {

        private static int ShutdownPlugin(int bEndProcess)
        {
            return 0;
        }

        private static IntPtr GetPluginInfo(IntPtr hModule)
        {
            return new IntPtr();
        }

        private static int InitializePlugin()
        {
            return 0;
        }

        [PluginMethod]
        private static int UNKNOWN_CMD([MarshalAs(UnmanagedType.LPTStr)] StringBuilder sb)
        {
            return 0;
        }

        [ PluginMethod]
        private static int key(IntPtr keyInfoPtr)
        {
            return 0;
        }

        /// <summary>
        /// Rewrites ~ to the home directory in CD. This is just an example.
        /// </summary>
        /// <param name="sb"></param>
        /// <returns></returns>
        [ PluginMethod]
        private static uint CD([MarshalAs(UnmanagedType.LPTStr)] StringBuilder sb)
        {
            return 0;
        }

        [ PluginMethod]
        private static uint DIR([MarshalAs(UnmanagedType.LPTStr)] StringBuilder sb)
        {
            return 0;
        }

        [ PluginMethod]
        private static uint f_testfunc([MarshalAs(UnmanagedType.LPTStr)] StringBuilder sb)
        {
            return 0;
        }

        [ PluginMethod]
        private static uint _testvar([MarshalAs(UnmanagedType.LPTStr)] StringBuilder sb)
        {
            return 0;
        }
    }
}
