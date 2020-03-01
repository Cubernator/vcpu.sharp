using System;
using System.Runtime.InteropServices;

namespace Vcpu
{
    public class Processor : IDisposable
    {
        public Processor()
        {
            _inner = vcpu_processor_create();
        }

        ~Processor()
        {
            Dispose();
        }

        public int State => vcpu_processor_get_state(_inner);

        public void Dispose()
        {
            if (_inner != IntPtr.Zero)
            {
                vcpu_processor_destroy(_inner);
                _inner = IntPtr.Zero;
            }

            GC.SuppressFinalize(this);
        }

        public void Tick(DataRef instructions, Memory memory)
            => Validation.ValidateResult(vcpu_processor_tick(_inner, instructions.Pointer, instructions.Size, memory.Inner));

        public void Tick(Program program, Memory memory) => Tick(program.Instructions, memory);

        public void Run(DataRef instructions, Memory memory)
            => Validation.ValidateResult(vcpu_processor_run(_inner, instructions.Pointer, instructions.Size, memory.Inner));

        public void Run(Program program, Memory memory) => Run(program.Instructions, memory);

#pragma warning disable IDE1006 // Naming Styles
        [DllImport("vcpu_interop")]
        internal static extern IntPtr vcpu_processor_create();

        [DllImport("vcpu_interop")]
        internal static extern int vcpu_processor_get_state(IntPtr processor);

        [DllImport("vcpu_interop")]
        internal static extern int vcpu_processor_tick(IntPtr processor, IntPtr instr, int instr_len, IntPtr memory);

        [DllImport("vcpu_interop")]
        internal static extern int vcpu_processor_run(IntPtr processor, IntPtr instr, int instr_len, IntPtr memory);

        [DllImport("vcpu_interop")]
        internal static extern void vcpu_processor_destroy(IntPtr processor);
#pragma warning restore IDE1006 // Naming Styles

        #region Implementation
        IntPtr _inner;
        #endregion
    }
}
