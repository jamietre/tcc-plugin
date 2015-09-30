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

namespace TildeSupport.Processes
{
    public class ProcessInfo
    {
        public ProcessInfo()
        {
        }

        public ProcessInfo(IEnumerable<ProcessInfo> allProcesses, ManagementBaseObject process)
        {
            ProcessId = (uint)process["ProcessId"];
            ParentProcessId = (uint)process["ParentProcessId"];
            Name = (string)process["Name"];
            CommandLine = (string)process["CommandLine"];
            AllProcesses = allProcesses;
            //CreationDate = (DateTime)process["CreationDate"];
        }

        private IEnumerable<ProcessInfo> AllProcesses;

        public uint ProcessId
        {
            get;
            set;
        }

        public uint ParentProcessId
        {
            get;
            set;
        }

        public string Name
        {
            get; set;
        }

        public string CommandLine
        {
            get;
            set;
        }

        public DateTime CreationDate
        {
            get; set;
        }

        public ProcessInfo ParentProcess
        {
            get
            {
                return AllProcesses.Where(item => item.ProcessId == this.ParentProcessId).Single();
            }
        }

        public IEnumerable<ProcessInfo> ChildProcesses
        {
            get
            {
                return AllProcesses.Where(item => item.ParentProcessId == this.ProcessId);
            }
        }

    }
}

