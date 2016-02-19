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
using System.Diagnostics;
using System.IO;
using System.Reflection;

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

            //LoadAssemblies();
        }

        

        private static void LoadAssemblies() {
            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();

            loadedAssemblies
                .SelectMany(x => x.GetReferencedAssemblies())
                .Distinct()
                .Where(y => loadedAssemblies.Any((a) => a.FullName == y.FullName) == false)
                .ToList()
                .ForEach(x => loadedAssemblies.Add(AppDomain.CurrentDomain.Load(x)));
        }

        //private static Configuration Config;
        private static TccCommands Tcc;
        private static ExternalLoader Loader;

        #region TCC API

        [DllExport]
        private static uint InitializePlugin()
        {
            TccEventManager.LoadConfig();
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
            return Helpers.Safe<uint>(() =>
            {
                
                if (sb.ToStringTrimmed() == String.Empty)
                {
                    return _SPAWNED(sb);
                }

                var loader = new ProcessInfo(Tcc);
                var pid = loader.Spawn(sb.ToStringTrimmed());
                Console.WriteLine(String.Format("Started with PID {0}", pid));
                return 0;
            });
            

            
        }

        [PluginMethod, DllExport]
        public unsafe static uint SPAWNED([MarshalAs(UnmanagedType.LPTStr)] StringBuilder sb)
        {
            return Helpers.Safe<uint>(() =>
            {
                return _SPAWNED(sb);
            });
        }

        private static uint _SPAWNED(StringBuilder sb)
        {
            var processes = ProcessInfo.GetMyProcessesWithChildren();

            Tcc.WriteStdout(processes.Any() ?
                String.Join(System.Environment.NewLine,
                processes.Select(item => ProcessInfo.FormatProcess(item))) :
                "No running processes were spawned from this console.");

            return 0;
        }
            

        [PluginMethod, DllExport]
        public unsafe static uint PLIST([MarshalAs(UnmanagedType.LPTStr)] StringBuilder sb)
        {
            var processes = new Processes.ProcessList();

            Tcc.WriteStdout(processes.Any() ?
                String.Join(System.Environment.NewLine,
                processes.Select(item => Processes.ProcessFormatter.FormatProcess(item))) :
                "No running processes were spawned from this console.");

            return 0;
        }

        [PluginMethod, DllExport]
        public unsafe static uint KILL([MarshalAs(UnmanagedType.LPTStr)] StringBuilder sb)
        {

            int pid;
            if (!int.TryParse(sb.ToStringTrimmed(), out pid)) {
                Tcc.WriteStdout("Must pass PID to kill.");
                return 0;
            }

            ProcessInfo.Kill(pid);
            return 0;


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sb"></param>
        /// <returns></returns>
        [PluginMethod, DllExport]
        public unsafe static uint CONFIG([MarshalAs(UnmanagedType.LPTStr)] StringBuilder sb)
        {
            TccEventManager.LoadConfig();
            return 0;

        }

        [PluginMethod, DllExport]
        public unsafe static uint _GITBRANCH([MarshalAs(UnmanagedType.LPTStr)] StringBuilder sb)
        {
            string output = runCommand("git", "status -s -b --porcelain").Replace("\r", "");

            string[] parts = output.Split(new string[] { "\n"}, StringSplitOptions.RemoveEmptyEntries);

            // format ## master...origin/master [ahead 2]
            // format ## master...origin/master [behind 2]

            string branch = "";
            string offset = "";
            string changed = "";

            if (parts.Length > 0)
            {
                if (parts.Length > 1)
                {
                    changed = "*";
                }
                string text = parts[0];
                int pos = text.IndexOf("...");
                
                
                if (pos >= 0) {
                    branch = text.Substring(3, pos - 3);
                } else {
                    branch = text.Substring(3);
                }

                pos = text.IndexOf("[", Math.Max(pos, 0));
                if (pos >= 0) {
                    offset = text.Substring(pos).Trim().Replace("ahead ", "+").Replace("behind ", "-");
                }
                

                if (!string.IsNullOrEmpty(branch))
                {
                    branch = " " + changed + branch + offset;
                }
            }

            sb.Replace(branch);
            
            return 0;
        }

        [PluginMethod, DllExport]
        public unsafe static uint _PATHLAST([MarshalAs(UnmanagedType.LPTStr)] StringBuilder sb)
        {
            // get current folder
            var path = TccCommands.ExpandVariables("%CD");
            var parts = path.Split('\\');
            sb.Replace(String.Join("/", parts.Reverse().Take(2).Reverse()));
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



        [PluginMethod, DllExport]
        public unsafe static uint f_TESTFUNC([MarshalAs(UnmanagedType.LPTStr)] StringBuilder sb)
        {
            sb.Replace(sb.ToString() + "-");
            return 0;
        }


        private static RunJs.JsEngine Engine;

        [PluginMethod, DllExport]
        public static uint f_JS([MarshalAs(UnmanagedType.LPTStr)] StringBuilder sb)
        {
            if (Engine == null)
            {
                Engine = new RunJs.JsEngine();
            }
            string raw = sb.ToString().Trim();

            try
            {
                var result = Engine.Execute(ParseJsInput(raw));
                if (result != RunJs.JsValue.Undefined) {
                    sb.Replace(ParseJsOutput(result.ToString()));
                }
            }
            catch(Exception e) {
                sb.Replace(e.Message);
            }
         
            return 0;
        }

        private static string ParseJsInput(string text)
        {

            if (text.Length >= 2 && text[0] == '\"' && text[text.Length - 1] == '\"')
            {
                text = text.Substring(1, text.Length - 2);
            }
            return text.Replace("\"\"", "\"");
        }


        private static string ParseJsOutput(string text)
        {
            return text.Replace("\\\\","\\");
        }

        private static string runCommand(string command, string args)
        {
            Process p = new Process();
            // Redirect the output stream of the child process.
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.Arguments = args;
            p.StartInfo.FileName = command;
            p.Start();

            string output = p.StandardOutput.ReadToEnd() ?? "";

            p.WaitForExit();

            return output;
        }


        #endregion

    }
}
