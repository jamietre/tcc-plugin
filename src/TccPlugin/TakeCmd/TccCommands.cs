using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TccPlugin.TakeCmd
{
 
    /// <summary>
    /// 
    /// </summary>
    public unsafe class TccCommands
    {
        private TccCommandExecutor CmdExecutor = new TccCommandExecutor();
        
        /// <summary>
        /// Execute a command on the Tcc API
        /// </summary>
        /// <param name="name"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public uint ExecuteCmd(TccCommandName name, string commandText)
        {
            var cmd = TccCommandRepo.Get(name);
            return CmdExecutor.Execute(cmd.WinApiCmd, commandText);
        }

        /// <summary>
        /// A default handler for mapping paths. Any method invoked involving a file path parameter
        /// should automatically use this to pre-process the path (if present)
        /// </summary>
        /// <returns></returns>
        public Func<string, string> MapPath
        {
            get
            {
                return CmdExecutor.MapPath;
            }
            set
            {
                CmdExecutor.MapPath = value;
            }
        }
    }
}
