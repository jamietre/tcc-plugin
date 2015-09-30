using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TccPlugin
{
    public static class FileHelper
    {

        public static string FindInSearchPath(string fileName)
        {
            bool hasExt = fileName.IndexOf('.') > 0;

            var paths = new[] { Environment.CurrentDirectory }
              .Concat(Environment.GetEnvironmentVariable("PATH").Split(';'));
            
            var extensions = Environment.GetEnvironmentVariable("PATHEXT").Split(';')
                               .Where(e => e.StartsWith(".") );

            var combinations = hasExt ?
                paths.Select((path)=> Path.Combine(path, fileName)) :
                paths.SelectMany(x => extensions,
                    (path, extension) => Path.Combine(path, fileName + extension));

            return combinations.FirstOrDefault(File.Exists);
        }
    }
}
