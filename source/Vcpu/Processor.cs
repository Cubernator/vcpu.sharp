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

        public uint ProgramCounter => vcpu_processor_get_program_counter(_inner);

        public ExitCode? ExitCode
        {
            get
            {
                var state = vcpu_processor_get_state(_inner);
                return state < 0 ? default(ExitCode?) : new ExitCode(state);
            }
        }

        public bool IsStopped => ExitCode.HasValue;

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

        public void Reset() => vcpu_processor_reset(_inner);

        public int GetRegister(int index)
        {
            Validation.ValidateResult(vcpu_processor_get_register(_inner, index, out var value));
            return value;
        }

        public void SetRegister(int index, int value) => Validation.ValidateResult(vcpu_processor_set_register(_inner, index, value));

#pragma warning disable IDE1006 // Naming Styles
        [DllImport(Constants.VcpuLib)]
        internal static extern IntPtr vcpu_processor_create();

        [DllImport(Constants.VcpuLib)]
        internal static extern int vcpu_processor_get_register(IntPtr processor, int index, out int value);

        [DllImport(Constants.VcpuLib)]
        internal static extern int vcpu_processor_set_register(IntPtr processor, int index, int value);

        [DllImport(Constants.VcpuLib)]
        internal static extern uint vcpu_processor_get_program_counter(IntPtr processor);

        [DllImport(Constants.VcpuLib)]
        internal static extern int vcpu_processor_get_state(IntPtr processor);

        [DllImport(Constants.VcpuLib)]
        internal static extern int vcpu_processor_tick(IntPtr processor, IntPtr instr, int instr_len, IntPtr memory);

        [DllImport(Constants.VcpuLib)]
        internal static extern int vcpu_processor_run(IntPtr processor, IntPtr instr, int instr_len, IntPtr memory);

        [DllImport(Constants.VcpuLib)]
        internal static extern void vcpu_processor_reset(IntPtr processor);

        [DllImport(Constants.VcpuLib)]
        internal static extern void vcpu_processor_destroy(IntPtr processor);
#pragma warning restore IDE1006 // Naming Styles

        #region Implementation
        IntPtr _inner;
        #endregion
    }
}
