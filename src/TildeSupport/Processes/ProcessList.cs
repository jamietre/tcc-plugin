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
    public class ProcessList: IEnumerable<ProcessInfo>
    {
        public ProcessList()
        {
         
        }

        public ProcessList(ManagementBaseObject process)
        {

        }
        private List<string> Criteria = new List<string>();
        private List<ProcessInfo> Processes;


        /// <summary>
        /// Return a heirarchy of processes related to an ID
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public IEnumerable<ProcessInfo> GetHierarchy(ProcessInfo process)
        {
            
            while (process.ParentProcessId != 0)
            {
                process = GetProcesses().Where(item => item.ProcessId == process.ParentProcessId).Single();
            }

            return EnumerableHelper.Enumerate(process).Concat(GetChildProcesses(process, 0));
        }

        public IEnumerable<ProcessInfo> GetHierarchyForId(int pid)
        {
            var process = GetProcesses().Where(item => item.ProcessId == pid).SingleOrDefault();

            if (process == null)
            {
                Enumerable.Empty<ProcessInfo>();
            }

            return GetHierarchy(process);
        }
        /// <summary>
        /// Get all 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProcessInfo> GetHierarchy()
        {
            var root = GetProcesses().Where(item => item.ParentProcessId == 0);
            return root.SelectMany(item=>GetHierarchy(item));
        }

        private IEnumerable<ProcessInfo> GetChildProcesses(ProcessInfo process, int depth)
        {

            var children = GetProcesses().Where(item => item.ParentProcessId == process.ProcessId);
            if (!children.Any()) {
                return Enumerable.Empty<ProcessInfo>();
            } else {
                return children.SelectMany(item => GetChildProcesses(item, depth+1));
            }
        }


        private void AddCriteria(string criteria)
        {
            Processes = null;
            Criteria.Add(criteria);
        }

        private ProcessList FilterById(int id)
        {
        
            AddCriteria(String.Format("ParentProcessID={0} or ProcessId={0}", id));
            return this;
        }

        /// <summary>
        /// Get processes by name, use %xxx% for wildcard matching
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private ProcessList GetProcessesByName(string name)
        {
            AddCriteria(String.Format("Name like '{0}'", name));
            return this;
            
            //foreach (ManagementObject mo in mos.Get())
            //{
            //    children.Add(new ProcessWrapper(Process.GetProcessById(Convert.ToInt32(mo["ProcessID"])), processId));
            //}
        }
        
        private IEnumerable<ProcessInfo> GetProcesses()
        {
            if (Processes != null)
            {
                return Processes;
            }
            Processes = new List<ProcessInfo>();

            var query = "Select * From Win32_Process";
            if (Criteria.Count > 0)
            {
                query += " where " + String.Join(" AND ", Criteria);
            }
            ManagementObjectSearcher mos = new ManagementObjectSearcher(query);
            
            foreach (ManagementObject mo in mos.Get())
            {
                Processes.Add(new ProcessInfo(Processes, mo));
            }
            return Processes;

        }


        public IEnumerator<ProcessInfo> GetEnumerator()
        {
            return GetProcesses().GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
