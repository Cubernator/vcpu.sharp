using System;
using System.Runtime.InteropServices;

namespace Vcpu
{
    class Validation
    {
        public static void ValidateResult(int result)
        {
            if (result != 0)
            {
                var msg = vcpu_result_get_description(result, out var desc) == 0
                    ? Utf8Marshal.PtrToString(desc)
                    : "Unknown error";

                throw new VcpuException(result, msg);
            }
        }

#pragma warning disable IDE1006 // Naming Styles
        [DllImport(Constants.VcpuLib)]
        internal static extern int vcpu_result_get_description(int result, out IntPtr desc);
#pragma warning restore IDE1006 // Naming Styles
    }
}
