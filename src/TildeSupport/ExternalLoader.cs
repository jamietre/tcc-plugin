using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TccPlugin.Parser;
using TccPlugin.TakeCmd;
using TccPlugin;

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
            Tcc.Command("C:/Program Files/Sublime Text 3/subl.exe", text);
        }

        public void Explorer(string text)
        {
            Tcc.Command("c:/windows/explorer.exe", "/e,", text);
        }
    }
}

