using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using RGiesecke.DllExport;

namespace TccPlugin
{
    /// <summary>
    /// Expose a method for TCC
    /// </summary>
    public class TccMethod: DllExportAttribute
    {
        public TccMethod(string name, TccMethodTypes type): base(TransformName(type,name), CallingConvention.Cdecl)
        {
          
        }

        public TccMethod(string name)
            : base(TransformName(TccMethodTypes.Other, name), CallingConvention.Cdecl)
        {

        }

        private static string TransformName(TccMethodTypes type, string name)
        {
            switch (type)
            {
                case TccMethodTypes.Function:
                    return "f_" + name;
                case TccMethodTypes.InternalVariable:
                    return "_" + name;
                case TccMethodTypes.Other:
                default:
                    return name;            
            }
        }
    }
}
