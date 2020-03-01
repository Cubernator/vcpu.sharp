using System;
using System.Runtime.InteropServices;

namespace Vcpu
{
    public class PlainMemory : ContiguousMemory
    {
        public PlainMemory(int size) : base(vcpu_memory_create_plain(size))
        {
        }

        public static PlainMemory CreateFrom(DataRef data)
        {
            var memory = new PlainMemory(data.Size);
            memory.CopyFrom(data, 0, data.Size, 0);
            return memory;
        }

#pragma warning disable IDE1006 // Naming Styles
        [DllImport("vcpu_interop")]
        internal static extern IntPtr vcpu_memory_create_plain(int size);
#pragma warning restore IDE1006 // Naming Styles
    }
}
