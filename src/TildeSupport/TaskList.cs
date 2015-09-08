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

namespace TildeSupport
{
    public class TaskList
    {
        public TaskList(string name)
        {
            ProcessCache  = new TaskCache
            {
                Name = name,
                Processes = Process.GetProcessesByName(name)
            };
        }

        private class TaskCache
        {
            public Process[] Processes { get; set; }
            public string Name { get; set; }
        }

        private TaskCache ProcessCache;


        /// <summary>
        /// Return any processes added since last Push
        /// </summary>
        public IEnumerable<Process> Compare()
        {
            var cache = ProcessCache;

            var current = Process.GetProcessesByName(cache.Name);
            
            var ids = current.Select(item=>item.Id)
                .Except(cache.Processes.Select(item=>item.Id));
            
            return current.Where(item=>ids.Contains(item.Id));
        }

        /// <summary>
        /// Show only those 
        /// </summary>
        /// <param name="pidList"></param>
        /// <returns></returns>
        public IEnumerable<Process> Filter(IEnumerable<int> pidList)
        {
            return ProcessCache.Processes.Where(item=>pidList.Contains(item.Id));
        }
    }
}
