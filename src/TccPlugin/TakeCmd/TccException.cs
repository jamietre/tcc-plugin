using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TccPlugin.Parser;

namespace TccPlugin.TakeCmd
{
    public class TccException: Exception
    {
        public TccException(string message) : base(message) { }
    }
}
