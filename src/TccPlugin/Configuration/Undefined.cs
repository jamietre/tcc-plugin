using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TccPlugin.Configuration
{
    public class Undefined 
    {
        private static Undefined _Instance = new Undefined();
        public static Undefined Instance
        {
            get
            {
                return _Instance;
            }
        }
        private Undefined()
        {

        }
       
    }
}
