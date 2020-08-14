using System;
using System.Runtime.InteropServices;

namespace Vcpu
{
    public class CompositeMemory : Memory
    {
        public CompositeMemory() : base(vcpu_memory_create_comp())
        {
        }

        public void Mount(int address, string key, Memory fragment)
            => Validation.ValidateResult(vcpu_memory_comp_mount(Inner, address, key, fragment.Inner));

        public void Unmount(string key)
            => Validation.ValidateResult(vcpu_memory_comp_unmount(Inner, key));

#pragma warning disable IDE1006 // Naming Styles
        [DllImport(Constants.VcpuLib)]
        internal static extern IntPtr vcpu_memory_create_comp();

        [DllImport(Constants.VcpuLib)]
        internal static extern int vcpu_memory_comp_mount(IntPtr memory, int address, string key, IntPtr fragment);

        [DllImport(Constants.VcpuLib)]
        internal static extern int vcpu_memory_comp_unmount(IntPtr memory, string key);
#pragma warning restore IDE1006 // Naming Styles

    }
}
