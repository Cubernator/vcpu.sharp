using System;
using System.Runtime.InteropServices;

namespace Vcpu
{
    public class Program : IDisposable
    {
        public Program(string source)
        {
            var result = vcpu_program_assemble(source, out _inner, out var error);
            if (result != 0)
            {
                throw new VcpuException(result, error);
            }
        }

        ~Program()
        {
            Dispose();
        }

        public DataRef Data
        {
            get
            {
                vcpu_program_get_data(_inner, out var data, out var dataLength);
                return new DataRef(data, dataLength, true);
            }
        }

        public DataRef Instructions
        {
            get
            {
                vcpu_program_get_instructions(_inner, out var instructions, out var instructionsLength);
                return new DataRef(instructions, instructionsLength, true);
            }
        }

        public void Dispose()
        {
            if (_inner != IntPtr.Zero)
            {
                vcpu_program_destroy(_inner);
                _inner = IntPtr.Zero;
            }

            GC.SuppressFinalize(this);
        }

#pragma warning disable IDE1006 // Naming Styles
        [DllImport("vcpu_interop")]
        internal static extern int vcpu_program_assemble(string source, out IntPtr program, out string error);

        [DllImport("vcpu_interop")]
        internal static extern void vcpu_program_get_data(IntPtr program, out IntPtr data, out int data_len);

        [DllImport("vcpu_interop")]
        internal static extern void vcpu_program_get_instructions(IntPtr program, out IntPtr instr, out int instr_len);

        [DllImport("vcpu_interop")]
        internal static extern void vcpu_program_destroy(IntPtr program);
#pragma warning restore IDE1006 // Naming Styles

        #region Implementation
        IntPtr _inner;
        #endregion
    }
}
