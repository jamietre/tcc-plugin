using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Reflection;
using RGiesecke.DllExport;


namespace TccPlugin
{
    public class PluginInfo
    {
        static PluginInfo()
        {
            var types = Assembly.GetExecutingAssembly().GetTypes();

            // Get all static methods in with [DllExport] froem every assembly, except our pre-configured ones
            var exported = types.SelectMany(type =>
            {
                return type.GetMethods(BindingFlags.Static)
                    .Where(method => method.CustomAttributes
                        .Count(attr => attr.GetType() == typeof(DllExportAttribute)) > 0);
            });

            PluginFunctions = String.Join(",", 
                exported.Select(method =>
                {
                    var name = ((DllExportAttribute)method.GetCustomAttribute(typeof(DllExportAttribute))).ExportName;
                    return name.StartsWith("f_") ? 
                        "@" + name.Substring(2) :
                        name;
                })
            );

        }

        static string PluginFunctions;

        /// <summary>
        /// Plugin author's name
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Plugin author's email
        /// </summary>
        public string Email { get; set; }
        
        /// <summary>
        /// Plugin web site URL
        /// </summary>
        public string WebSite { get; set; }
        
        /// <summary>
        /// Description of plugin
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Module Name
        /// </summary>
        public string ModuleName { get; set; }

        
        /// <summary>
        /// Returns a string with the names of all functions exposed from the DLL
        /// </summary>
        /// <returns></returns>
        public string Functions {
            get {
                return PluginFunctions;
            }
        }
    }
    
}
