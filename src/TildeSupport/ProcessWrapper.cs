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
    public class ProcessWrapper
    {
        public ProcessWrapper(Process process): this(process, 0)
        {
        }

        public ProcessWrapper(Process process, int parentId)
        {
            _Process = process;
            ParentId = parentId;
        }

        private Process _Process;

        public void Kill()
        {
            _Process.Kill();
        }

        public int Id
        {
            get
            {
                return _Process.Id;
            }
        }

        public int ParentId
        {
            get;
            private set;
        }

        public string ProcessName
        {
            get
            {
                return _Process.ProcessName;
            }
        }

        public string CommandLine
        {
            get
            {
                return ProcessInfo.GetCommandLine(Id);
            }
        }

        public bool HasExited
        {
            get
            {
                return _Process.HasExited;
            }
        }
    }
}

