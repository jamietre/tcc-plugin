using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TccPlugin.Parser;
using TccPlugin;
using TccPlugin.TakeCmd;
using TccPlugin.Configuration;
using System.Diagnostics;
using System.Management;
using System.Runtime.InteropServices;

namespace TildeSupport
{
    public class ExternalLoader
    {
        static ExternalLoader()
        {
            
            
        }

        private static void ConfigureLaunchers()
        {
            Launchers = new Dictionary<string, PluginConfig>();
            var launchers = TccEventManager.Config.GetNode("launchers");
            var edit = TccEventManager.Config.GetNode("actions.edit");

            foreach (var key in edit.Keys)
            {
                var extensions = edit.GetArray<string>(key);
                foreach (var ext in extensions)
                {
                    Launchers.Add(ext, launchers.GetNode(key));
                }
            }
        }
        private static Dictionary<string, PluginConfig> Launchers;

        [DllImport("Shlwapi.dll", CharSet = CharSet.Unicode)]
        public static extern uint AssocQueryString(uint flags, uint str,
           string pszAssoc, string pszExtra, [Out] StringBuilder pszOut, ref uint
           pcchOut); 

        public ExternalLoader(TccCommands tcc)
        {
            Tcc = tcc;
        }

        private TccCommands Tcc;

        public void Load(string loader, string text)
        {
                ConfigureLaunchers();
            
            var fullPath = Tcc.MakeFullName(text);
            var ext = text.Split('.').Last();

            string app;
            string args;

            PluginConfig config;
            if (!Launchers.TryGetValue(ext, out config) && !Launchers.TryGetValue("default", out config))
            {
                app = AssocQueryString("." + ext);
                args = text;
            }
            else
            {
                app = config.GetString("path");
                args = String.Format(config.GetString("args", "{0}"), text);
            }
            
            var startInfo = new ProcessStartInfo(app,args);
            startInfo.UseShellExecute = false;
            Process.Start(startInfo);


            //Tcc.Command("C:/Program Files/Sublime Text 3/subl.exe",  fullPath );
        }

        public void Explorer(string text)
        {
            text = String.IsNullOrEmpty(text) ?
                "." : text;
            
            Tcc.Command("c:/windows/explorer.exe", "/e,", text);
        }

        static string AssocQueryString(string extension)
        {
            const int S_OK = 0;
            const int S_FALSE = 1;

            uint length = 0;
            uint ret = AssocQueryString(0, 2, extension, null, null, ref length);
            if (ret != S_FALSE)
            {
                throw new InvalidOperationException("Could not determine associated string");
            }

            var sb = new StringBuilder((int)length); // (length-1) will probably work too as the marshaller adds null termination
            ret = AssocQueryString(0, 2, extension, null, sb, ref length);
            if (ret != S_OK)
            {
                throw new InvalidOperationException("Could not determine associated string");
            }

            return sb.ToString();
        }

    }
}

