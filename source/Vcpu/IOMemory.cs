using System;
using System.Runtime.InteropServices;

namespace Vcpu
{
    public class IOMemory : ContiguousMemory
    {
        public delegate bool CanWriteCallback(IntPtr data, int data_len, int address, int size, IntPtr user_data);
        public delegate void OnWriteCallback(IntPtr data, int data_len, int address, int size, IntPtr user_data);

        public IOMemory(int size, CanWriteCallback canWrite, OnWriteCallback onWrite)
            : base(vcpu_memory_create_io(size, canWrite, onWrite, IntPtr.Zero))
        {
        }

#pragma warning disable IDE1006 // Naming Styles
        [DllImport("vcpu_interop")]
        internal static extern IntPtr vcpu_memory_create_io(int size, CanWriteCallback can_write, OnWriteCallback on_write, IntPtr user_data);
#pragma warning restore IDE1006 // Naming Styles
    }
}
