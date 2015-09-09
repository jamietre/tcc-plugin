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

namespace TildeSupport
{
    public class ExternalLoader
    {
        public ExternalLoader(TccCommands tcc)
        {
            Tcc = tcc;
        }

        private TccCommands Tcc;

        public void Load(string loader, string text)
        {
            var fullPath = Tcc.MakeFullName(text);

            Tcc.Command("C:/Program Files/Sublime Text 3/subl.exe",  fullPath );
        }

        public void Explorer(string text)
        {
            text = String.IsNullOrEmpty(text) ?
                "." : text;

            Tcc.Command("c:/windows/explorer.exe", "/e,", text);
        }

    }
}

