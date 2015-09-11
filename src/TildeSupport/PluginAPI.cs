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

        //private static Configuration Config;
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
            return Tcc.ProcessCmd(TccCommandName.CD, sb, (args) =>
            {
                args.Insert(0, "/D");
            }); 
            
        }

        [PluginMethod, DllExport]
        public unsafe static uint DIR([MarshalAs(UnmanagedType.LPTStr)] StringBuilder sb)
        {
            return Tcc.ProcessCmd(TccCommandName.DIR, sb);
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

        [PluginMethod, DllExport]
        public unsafe static uint SPAWN([MarshalAs(UnmanagedType.LPTStr)] StringBuilder sb)
        {
            //if (sb.ToStringTrimmed() == String.Empty)
            //{
            //    return SPAWNED(sb);
            //}

            var loader = new ProcessInfo(Tcc);
            loader.Spawn(sb.ToStringTrimmed());
            return 0;
        }

        [PluginMethod, DllExport]
        public unsafe static uint SPAWNED([MarshalAs(UnmanagedType.LPTStr)] StringBuilder sb)
        {
            var processes = ProcessInfo.GetRunningProcessesWithChildren();

            Console.WriteLine(processes.Any() ?
                String.Join(System.Environment.NewLine, 
                processes.Select(item=>ProcessInfo.FormatProcess(item))) :
                "No running processes were spawned from this console.");

            return 0;
        }

        [PluginMethod, DllExport]
        public unsafe static uint KILL([MarshalAs(UnmanagedType.LPTStr)] StringBuilder sb)
        {
            int pid;
            if (!int.TryParse(sb.ToStringTrimmed(), out pid)) {
                Console.WriteLine("Must pass PID to kill.");
                return 0;
            }

            ProcessInfo.Kill(pid);
            return 0;


        }
        [PluginMethod, DllExport]
        public unsafe static uint CONFIG([MarshalAs(UnmanagedType.LPTStr)] StringBuilder sb)
        {
            TccEventManager.LoadConfig();
            return 0;

        }

        [PluginMethod, DllExport]
        public unsafe static uint _STARTPID([MarshalAs(UnmanagedType.LPTStr)] StringBuilder sb)
        {
            sb.Replace(ProcessInfo.LastPID.ToString());
            return 0;
        }

        [PluginMethod, DllExport]
        public unsafe static uint _STARTPIDS([MarshalAs(UnmanagedType.LPTStr)] StringBuilder sb)
        {

            sb.Replace(String.Join(",", ProcessInfo.RunningPIDs));
            return 0;
        }


        #endregion

    }
}
