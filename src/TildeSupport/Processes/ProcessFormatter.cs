using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TildeSupport.Processes
{
    public class ProcessFormatter
    {
        public static string FormatProcess(ProcessInfo process)
        {
            return String.Format("{0}{1} {2} {3}",
                (process.ParentProcessId == 0 ? "   " : "-->"),
                (process.ProcessId.ToString() + (false ? "*" : "")).PadRight(10),
                process.Name.PadRight(12),
                process.CommandLine);
        }
    }
}
