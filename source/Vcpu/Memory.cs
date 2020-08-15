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

        public void SetWord(int address, int value) => Validation.ValidateResult(vcpu_memory_set_word(Inner, address, value));
        public void SetHalf(int address, short value) => Validation.ValidateResult(vcpu_memory_set_half(Inner, address, value));
        public void SetByte(int address, byte value) => Validation.ValidateResult(vcpu_memory_set_byte(Inner, address, value));

        public int GetWord(int address)
        {
            Validation.ValidateResult(vcpu_memory_get_word(Inner, address, out var value));
            return value;
        }
        public int GetHalf(int address)
        {
            Validation.ValidateResult(vcpu_memory_get_half(Inner, address, out var value));
            return value;
        }
        public int GetByte(int address)
        {
            Validation.ValidateResult(vcpu_memory_get_byte(Inner, address, out var value));
            return value;
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
        [DllImport(Constants.VcpuLib)]
        internal static extern int vcpu_memory_get_ptr(IntPtr memory, out IntPtr ptr, out int size);

        [DllImport(Constants.VcpuLib)]
        internal static extern int vcpu_memory_read(IntPtr memory, IntPtr dest, int offset, int length);

        [DllImport(Constants.VcpuLib)]
        internal static extern int vcpu_memory_write(IntPtr memory, IntPtr src, int offset, int length);

        [DllImport(Constants.VcpuLib)]
        internal static extern int vcpu_memory_set_word(IntPtr memory, int address, int value);

        [DllImport(Constants.VcpuLib)]
        internal static extern int vcpu_memory_set_half(IntPtr memory, int address, short value);

        [DllImport(Constants.VcpuLib)]
        internal static extern int vcpu_memory_set_byte(IntPtr memory, int address, byte value);

        [DllImport(Constants.VcpuLib)]
        internal static extern int vcpu_memory_get_word(IntPtr memory, int address, out int value);

        [DllImport(Constants.VcpuLib)]
        internal static extern int vcpu_memory_get_half(IntPtr memory, int address, out short value);

        [DllImport(Constants.VcpuLib)]
        internal static extern int vcpu_memory_get_byte(IntPtr memory, int address, out byte value);

        [DllImport(Constants.VcpuLib)]
        internal static extern int vcpu_memory_resize(IntPtr memory, int size);

        [DllImport(Constants.VcpuLib)]
        internal static extern void vcpu_memory_destroy(IntPtr memory);
#pragma warning restore IDE1006 // Naming Styles
    }
}
