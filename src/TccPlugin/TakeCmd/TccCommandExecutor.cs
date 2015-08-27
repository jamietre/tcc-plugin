using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TccPlugin.Parser;

namespace TccPlugin.TakeCmd
{
    public unsafe class TccCommandExecutor
    {

        /// <summary>
        /// Execute the specificied TCC commmand. 
        /// </summary>
        /// <param name="action"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public uint Execute(TccLib.TCAction action, string text)
        {
            uint result;
            fixed (char* textPtr = text.ToString())
            {
                result = action(textPtr);
            }

            return result;
        }
        /// <summary>
        /// TODO Do I really need a separate signature for each variation? How to encapsulate with unsafe code? Func<char*> no work
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public uint Execute(TccLib.TCAction2 action, string text, int parm)
        {
            uint result;
            fixed (char* textPtr = text.ToString())
            {
                result = action(textPtr, parm);
            }

            return result;
        }

    }
}
