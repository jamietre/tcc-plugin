using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TccPlugin.Parser;
using TccPlugin.TakeCmd;
using TccPlugin;
using System.Diagnostics;
using System.Management;

namespace TildeSupport
{
    public class ProcessInfo
    {
        public ProcessInfo(TccCommands tcc)
        {
            Tcc = tcc;
        }
        
        private TccCommands Tcc;
        
        /// <summary>
        /// All PIDs spawned by this console
        /// </summary>
        private static List<int> SpawnedPIDs = new List<int>();
   

        /// <summary>
        /// Last spawned by this console
        /// </summary>
        public static int LastPID { get; private set; }

        /// <summary>
        /// Current running PIDs
        /// </summary>
        public static IEnumerable<int> RunningPIDs
        {
            get
            {
                return GetRunningProcesses().Select(item=>item.Id);
            }
        }

        public void Spawn(string text)
        {
            var TaskList = new TaskList("tcc");

            Tcc.ExecuteCmd(TccCommandName.START, "/B /C /PGM " + text + " <NUL");
            var processes = TaskList.Compare();
            Process process;
            try {
                process = processes.Single();
                var pid = process.Id;
                LastPID = pid;
                SpawnedPIDs.Add(pid);

                Console.Write(String.Format("Started with PID {0}", pid));
            }
            catch (Exception e)
            {
                Console.WriteLine("Unable to determine new process ID.");
            }
        }

        public static IEnumerable<Process> GetRunningProcesses()
        {
            var TaskList = new TaskList("tcc");
            return TaskList.Filter(SpawnedPIDs);            
        }

        public static  ProcessWrapper GetProcessById(int pid)
        {
            return new ProcessWrapper(Process.GetProcessById(pid));
        }

        public static IEnumerable<ProcessWrapper> GetChildProcesses(int processId)
        {
            List<ProcessWrapper> children = new List<ProcessWrapper>();
            ManagementObjectSearcher mos = new ManagementObjectSearcher(
                String.Format("Select * From Win32_Process Where ParentProcessID={0}", processId));

            foreach (ManagementObject mo in mos.Get())
            {
                children.Add(new ProcessWrapper(Process.GetProcessById(Convert.ToInt32(mo["ProcessID"])), processId));
            }

            return children;
        }

        public static string GetCommandLine(int processId)
        {
            string wmiQuery = string.Format("select CommandLine from Win32_Process where ProcessID='{0}'", processId);
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(wmiQuery);
            ManagementObjectCollection retObjectCollection = searcher.Get();

            if (retObjectCollection.Count != 1)
            {
                return "";
            }

            var enumerator = retObjectCollection.GetEnumerator();
            enumerator.MoveNext();
            return (string)enumerator.Current["CommandLine"];
        }

        public static IEnumerable<ProcessWrapper> GetRunningProcessesWithChildren()
        {

            return ProcessInfo.GetRunningProcesses().SelectMany(item =>
            {
                var items = new List<ProcessWrapper>();
                items.Add(new ProcessWrapper(item));

                var children = ProcessInfo.GetChildProcesses(item.Id);
                foreach (var child in children)
                {
                    items.Add(child);
                }
                return items;
            });

        }

        public static void Kill(int pid)
        {

            var process = ProcessInfo.GetProcessById(pid);
            if (process == null)
            {
                Console.WriteLine(String.Format("No process with id {0} was found running.", pid));
            }

            var children = ProcessInfo.GetChildProcesses(pid);
            foreach (var child in children)
            {
                KillSingle(child);
            }

            KillSingle(process);
        }

        private static void KillSingle(ProcessWrapper process)
        {
                try
                {
                    process.Kill();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.GetType().ToString() + ": " + e.Message);
                }
        }

        public static string FormatProcess(ProcessWrapper process)
        {
            return  String.Format("{0}{1} {2} {3}", 
                (process.ParentId == 0 ? "   " : "-->"),
                (process.Id.ToString() + (process.HasExited ? "*" : "")).PadRight(10),
                process.ProcessName.PadRight(12),
                process.CommandLine);
        }
    }
}

