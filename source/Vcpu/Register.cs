using System;
using System.Runtime.InteropServices;

namespace Vcpu
{
    public static class Register
    {
        public static int Count => vcpu_register_get_count();

        public static string GetName(int index)
        {
            Validation.ValidateResult(vcpu_register_get_name(index, out var name));
            return Utf8Marshal.PtrToString(name);
        }

#pragma warning disable IDE1006 // Naming Styles
        [DllImport(Constants.VcpuLib)]
        internal static extern int vcpu_register_get_count();

        [DllImport(Constants.VcpuLib)]
        internal static extern int vcpu_register_get_name(int index, out IntPtr name);
#pragma warning restore IDE1006 // Naming Styles
    }
}
