﻿using System;
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
    public static class TccEventManager
    {

        public static void Register(PluginInfo clientPluginInfo) {
            ClientPluginInfo = clientPluginInfo;
        }

        private static PluginInfo ClientPluginInfo;

        private static UnmanagedData<TccPluginInfo> pluginInfo;

        public static uint ShutdownPlugin(int bEndProcess)
        {
#if DEBUG
            Console.WriteLine("Shutting down " + pluginInfo.Source.pszDll);
#endif
            if (OnShutdown!= null)
            {
                OnShutdown();
            }

            pluginInfo.Dispose();

            return 0;
        }

        public static IntPtr GetPluginInfo(IntPtr hModule)
        {

            TccPluginInfo piInfo = new TccPluginInfo(hModule, ClientPluginInfo);

            pluginInfo = new UnmanagedData<TccPluginInfo>(piInfo);
            return pluginInfo.Pointer;
        }

        public static uint InitializePlugin()
        {

#if DEBUG
            Console.WriteLine("TCC Plugin Initialized");
#endif
            if (OnInitialize != null)
            {
                OnInitialize();
            }
            return 0;
        }

        public static uint UnknownCommand(StringBuilder sb)
        {
            return TccLib.RETURN_DEFER;
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

        public delegate void KeypressEventHandler(CommandLine commandLine);
        public static event KeypressEventHandler OnKeyPress;

        public delegate void PluginStateEventHandler();

        public static event PluginStateEventHandler OnInitialize;
        public static event PluginStateEventHandler OnShutdown;


        /// <summary>
        /// TODO: What is return value for?
        /// </summary>
        /// <param name="keyInfoPtr"></param>
        /// <returns></returns>
        public unsafe static uint Key(IntPtr keyInfoPtr)
        {
            if (OnKeyPress != null)
            {
                OnKeyPress(new CommandLine(keyInfoPtr));
            }

            return 0;

        }

    }
}
