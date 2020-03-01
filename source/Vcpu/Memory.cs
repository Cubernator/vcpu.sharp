using System;
using System.Runtime.InteropServices;

namespace Vcpu
{
    public abstract class Memory : IDisposable
    {
        ~Memory()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected Memory(IntPtr inner)
        {
            Inner = inner;
        }

        internal IntPtr Inner { get; private set; }

        protected virtual void Dispose(bool disposing)
        {
            if (Inner != IntPtr.Zero)
            {
                vcpu_memory_destroy(Inner);
                Inner = IntPtr.Zero;
            }
        }

#pragma warning disable IDE1006 // Naming Styles
        [DllImport("vcpu_interop")]
        internal static extern int vcpu_memory_get_ptr(IntPtr memory, out IntPtr ptr, out int size);

        [DllImport("vcpu_interop")]
        internal static extern int vcpu_memory_read(IntPtr memory, IntPtr dest, int offset, int length);

        [DllImport("vcpu_interop")]
        internal static extern int vcpu_memory_write(IntPtr memory, IntPtr src, int offset, int length);

        [DllImport("vcpu_interop")]
        internal static extern void vcpu_memory_destroy(IntPtr memory);
#pragma warning restore IDE1006 // Naming Styles
    }
}
