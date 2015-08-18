using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using RGiesecke.DllExport;

namespace TccPlugin
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct KeyInfo
    {
       
        /// <summary>
        /// Code for the key entered
        /// </summary>
	    public int	nKey;
        /// <summary>
        /// The starting row
        /// </summary>
	    public int	nHomeRow;
        /// <summary>
        /// The starting column
        /// </summary>
	    public int	nHomeColumn;
        /// <summary>
        /// The current row in the window
        /// </summary>
	    public int	nRow;
        /// <summary>
        /// The current column in the window
        /// </summary>
	    public int	nColumn;

        /// <summary>
        /// The command line
        /// </summary>
        //[MarshalAs(UnmanagedType.LPTStr)]
        public IntPtr pszLine;

        /// <summary>
        /// Pointer to the position in the line
        /// </summary>
        //[MarshalAs(UnmanagedType.LPTStr)]
        public IntPtr pszCurrent;

        /// <summary>
        /// if != 0, redraw the line
        /// </summary>
        public int fRedraw;
    }
    
}
