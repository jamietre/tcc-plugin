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

        /// <summary>
        /// Custom Commands pass the contents of the command line (except the command iteslf) as the argument. 
        /// </summary>
        /// <param name="sb">The arguments to the command</param>
        /// <returns></returns>
        [PluginMethod, DllExport]
        public unsafe static uint CUSTOM_COMMAND([MarshalAs(UnmanagedType.LPTStr)] StringBuilder sb)
        {
            /// .. do something
            return 0;
        }

        /// <summary>
        /// Dynamic variables are evaluated using this code each time they are accessed. Use the extension method
        /// sb.Replace(...) to return their value. In TCC you access a variable as %_DYNAMIC_VARIABLE
        /// </summary>
        /// <param name="sb"></param>
        /// <returns></returns>
        [PluginMethod, DllExport]
        public unsafe static uint _DYNAMIC_VARIABLE([MarshalAs(UnmanagedType.LPTStr)] StringBuilder sb) {
            sb.Replace("value of variable");
            return 0;
        }


        /// <summary>
        /// Custom functions are similar to dynamic variables but can take input.
        /// They are accessed as %@CUSTOM_FUNC[arguments]
        /// </summary>
        /// <param name="sb"></param>
        /// <returns></returns>
        [PluginMethod, DllExport]
        public unsafe static uint f_CUSTOM_FUNC([MarshalAs(UnmanagedType.LPTStr)] StringBuilder sb)
        {
            sb.Replace("result of function evaluation");
            return 0;
        }

        /// These methods are supposed to be supported but I couldn't get them to work.

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