using System;
using System.Runtime.InteropServices;

namespace Vcpu
{
    public readonly struct ExitCode
    {
        public ExitCode(int value)
        {
            Value = value;
        }
        public int Value { get; }

        public bool IsError => Value != 0;

        public string Description => vcpu_exit_code_get_description(Value, out var desc) == 0
            ? Utf8Marshal.PtrToString(desc)
            : "Unknown exit code";

#pragma warning disable IDE1006 // Naming Styles
        [DllImport(Constants.VcpuLib)]
        internal static extern int vcpu_exit_code_get_description(int code, out IntPtr desc);
#pragma warning restore IDE1006 // Naming Styles
    }
}
