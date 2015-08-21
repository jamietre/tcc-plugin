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
        public PluginInfo()
        {
            if (!String.IsNullOrEmpty(PluginFunctions))
            {
                return;
            }

            //Console.WriteLine("Assembly:" + Assembly.GetCallingAssembly().GetName().FullName);
            var types = Assembly.GetCallingAssembly().GetTypes();
            

            // Get all static methods in with [DllExport] froem every assembly, except our pre-configured ones
            var exported = types.SelectMany(type =>
            {
                return type.GetMethods(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public)
                    .Where(method =>
                    {
                        return method.CustomAttributes
                            .Count(attr => attr.AttributeType == typeof(PluginMethodAttribute)) > 0;
                    });
            });

            PluginFunctions = String.Join(",", 
                exported.Select(method =>
                {
                    var name = method.Name;
                    return name.StartsWith("f_") ? 
                        "@" + name.Substring(2) :
                        name == "key" ? "*key" :
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
