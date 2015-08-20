using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using RGiesecke.DllExport;
using TccPlugin.TakeCmd;

namespace TccPlugin.Parser
{
    
    public unsafe class CommandLine
    {
         
        public CommandLine(IntPtr keyInfoPtr)
        {
            Source = (KeyInfo*)keyInfoPtr;
        }

        /// <summary>
        /// The maximum size of the command line buffer. Don't actually know this but seems to be at least 512 bytes
        /// based on playing around, this should be safe enough
        /// </summary>

        private bool IsSetup = false;

        private KeyInfo* Source;
        private char* LinePtr;
        private char* CurrentPosPtr;
        private char[] Line;
        private int CurrentPos;

        /// <summary>
        /// Replace the current command line with the text specified. If the replacement is longer than the current command line text,
        /// it will pad it with spaces. (Redrawing a shorter string does not appear to clear the current line, not sure how to do this yet)
        /// </summary>
        /// <param name="text">The text</param>
        public void Replace(string text)
        {
            Setup();
            Replace(text, CurrentPos);
        }

        /// <summary>
        /// The keycode for the key pressed. Unfortunately there seems to be no obvious rule here for key combinations (e.g. a bitmask
        /// for ctrl or alt or something). Not sure if this is a windows thing or what, but need to just test for specific keys, I think. 
        /// Ctrl+key seems to be ascii starting at 1 for letters, and used a flag in bit 17 for other things. Regular keys are ascii.
        /// </summary>
        public int KeyCode
        {
            get
            {
                return Source->nKey;
            }
        }

        /// <summary>
        /// Replace the current command line with the text, starting from startingPos
        /// </summary>
        /// <param name="text"></param>
        /// <param name="startingPos"></param>
        public void Replace(string text, int startingPos)
        {
            Setup();
            var chars = text.ToCharArray();

            Line = Line.Skip(startingPos)
                .Concat(text.ToCharArray())
                .Concat(Line.Skip(startingPos + text.Length))
                .ToArray();

            CurrentPos += text.Length;
            
        }

        /// <summary>
        /// Insert the text at the current position
        /// </summary>
        /// <param name="text"></param>
        /// <param name="startingPos"></param>
        public void Insert(string text)
        {
            Setup();
            Insert(text, CurrentPos);
        }

        /// <summary>
        /// Insert the text at the position specified
        /// </summary>
        /// <param name="text"></param>
        /// <param name="startingPos"></param>
        public void Insert(string text, int startingPos)
        {
            Setup();
            Line = Line.Take(startingPos)
                .Concat(text.ToCharArray())
                .Concat(Line.Skip(startingPos))
                .ToArray();

            CurrentPos += text.Length;

        }

        /// <summary>
        /// Write the changes back to the command line
        /// </summary>
        public unsafe void Write()
        {
            // erase current buffer to EOL
            char* buf = LinePtr;

            var pos = 0;
            bool passedEnd = false;
            while (pos < TccLib.BUF_SIZE && pos < Line.Length)
            {
                if (buf[pos] == (char)0)
                {
                    passedEnd = true;
                }
                buf[pos] = Line[pos];
                pos++;
            }
            
            // pad if replacing with smaller-lenght string
            if (!passedEnd)
            {
                while (pos < TccLib.BUF_SIZE && buf[pos] != (char)0) {
                    buf[pos++] = (char)32;
                }
            }

            // move pointer after new text

            Source->pszCurrent = Source->pszLine + Math.Min(TccLib.BUF_SIZE, CurrentPos) * 2;

            Source->fRedraw = 1;
            Source->nKey = 0;
          
        }

        private unsafe void Setup()
        {
            // TODO .. encapsulate just the object separately to avoid this, use lazy instead

            if (IsSetup)
            {
                return;
            }

            LinePtr = (char*)Source->pszLine;
            CurrentPosPtr = (char*)Source->pszCurrent;
            CurrentPos = (int)(CurrentPosPtr - LinePtr);

            // erase current buffer to EOL
            char* buf = (char*)Source->pszLine;

            List<char> chars = new List<char>();
            var pos = 0;
            
            while (buf[pos] != (char)0) {
                chars.Add(buf[pos++]);
            }

            Line = chars.ToArray();

            IsSetup = true;
        }
    }
    
}
