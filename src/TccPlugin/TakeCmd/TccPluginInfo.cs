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
    internal struct TccPluginInfo
    {

        public TccPluginInfo(IntPtr _hModule, PluginInfo pluginInfo)
        {
            var assembly = Assembly.GetExecutingAssembly();

            pszDll = assembly.FullName;
            pszAuthor = pluginInfo.Author;
            pszDescription = pluginInfo.Description;
            pszWWW = pluginInfo.WebSite;
            pszEmail = pluginInfo.Email;
            pszModule = pluginInfo.ModuleName;

            var version = assembly.GetName().Version;
            nMajor = version.Major;
            nMinor = version.Minor;
            nBuild = version.Build;

            hModule = _hModule;

            pszFunctions = pluginInfo.Functions;
            
        }

        [MarshalAs(UnmanagedType.LPTStr)]
        public string pszDll;			// name of the DLL
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pszAuthor;			// author's name
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pszEmail;			// author's email
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pszWWW;			// author's web page
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pszDescription;	// (brief) description of plugin
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pszFunctions;		// comma-delimited list of functions in the
        //   plugin (leading _ for internal vars, @ for 
        //   var funcs, * for keystroke function, 
        //   otherwise it's a command)
        
        public int nMajor;					// plugin's major version #
        
        public int nMinor;					// plugin's minor version #
        
        public int nBuild;					// plugin's build #

        public IntPtr hModule;			// module handle
        
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pszModule;			// module name
    }
    
}
