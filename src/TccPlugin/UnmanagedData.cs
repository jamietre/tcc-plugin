using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;


namespace TccPlugin
{
    public class UnmanagedData<T> : IDisposable
    {

        private bool freed = true;

        public void Dispose()
        {
            if (!freed)
            {
                // This causes a crash on exit, so.. i removed it... read soemthing that said I should do this, but don't know why

                //Marshal.DestroyStructure(Pointer, typeof(T));
                Marshal.FreeHGlobal(Pointer);
                freed = true;
            }
        }
        
        /// <summary>
        /// Potential race condition? Should I lock Dispose method?
        /// </summary>
        ~UnmanagedData()
        {
            Dispose();
        }

        public UnmanagedData(T obj)
        {
            Source = obj;
            Pointer = Marshal.AllocHGlobal(Marshal.SizeOf(obj));
            Marshal.StructureToPtr(obj, Pointer, false);
            freed = false;
            
        }

        public IntPtr Pointer
        {
            get;
            private set;
        }

        public T Source
        {
            get;
            private set;
        }
    }
}
