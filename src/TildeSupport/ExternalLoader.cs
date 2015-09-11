using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TccPlugin.Parser;
using TccPlugin.TakeCmd;
using TccPlugin;
using System.Diagnostics;
using System.Management;
using System.Runtime.InteropServices;

namespace TildeSupport
{
    public class ExternalLoader
    {
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
            var fullPath = Tcc.MakeFullName(text);
            var ext = text.Split('.').Last();
            
            // use config here

            var app = AssocQueryString("." + ext);
            Process.Start(app, text);

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

