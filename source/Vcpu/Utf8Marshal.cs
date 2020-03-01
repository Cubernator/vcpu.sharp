using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Vcpu
{
    class Utf8Marshal
    {
        public static int GetStringLength(IntPtr ptr)
        {
            var length = 0;

            while (Marshal.ReadByte(ptr, length) != 0)
            {
                ++length;
            }

            return length;
        }
        public static string PtrToString(IntPtr ptr)
        {
            var buffer = new byte[GetStringLength(ptr)];
            Marshal.Copy(ptr, buffer, 0, buffer.Length);
            return Encoding.UTF8.GetString(buffer);
        }
    }
}
