using System;
using System.Runtime.InteropServices;

namespace Vcpu
{
    public class Executable : IDisposable
    {
        public Executable(string source, int dataOffset = 0)
        {
            var result = vcpu_executable_assemble(source, dataOffset, out _inner, out var sourceMap, out var error);
            if (result != 0)
            {
                throw new VcpuException(result, Utf8Marshal.PtrToString(error));
            }

            vcpu_source_map_get_data(sourceMap, out var sourceMapData, out var sourceMapLength);

            _sourceMap = new int[sourceMapLength];
            Marshal.Copy(sourceMapData, _sourceMap, 0, sourceMapLength);

            vcpu_source_map_destroy(sourceMap);
        }

        ~Executable()
        {
            Dispose();
        }

        public int DataOffset => vcpu_executable_get_data_offset(_inner);

        public DataRef Data
        {
            get
            {
                vcpu_executable_get_data(_inner, out var data, out var dataLength);
                return new DataRef(data, dataLength, true);
            }
        }

        public DataRef Instructions
        {
            get
            {
                vcpu_executable_get_instructions(_inner, out var instructions, out var instructionsLength);
                return new DataRef(instructions, instructionsLength, true);
            }
        }

        public void Dispose()
        {
            if (_inner != IntPtr.Zero)
            {
                vcpu_executable_destroy(_inner);
                _inner = IntPtr.Zero;
            }

            GC.SuppressFinalize(this);
        }

        public SourceSpan GetInstructionSpan(int instructionIndex)
        {
            var mapIndex = instructionIndex * 2;

            return mapIndex < _sourceMap.Length
                ? new SourceSpan(_sourceMap[mapIndex], _sourceMap[mapIndex + 1])
                : throw new ArgumentOutOfRangeException(nameof(instructionIndex));
        }

#pragma warning disable IDE1006 // Naming Styles
        [DllImport(Constants.VcpuLib)]
        internal static extern int vcpu_executable_assemble(string source, int data_offset, out IntPtr executable, out IntPtr source_map, out IntPtr error);

        [DllImport(Constants.VcpuLib)]
        internal static extern void vcpu_executable_get_data(IntPtr executable, out IntPtr data, out int data_len);

        [DllImport(Constants.VcpuLib)]
        internal static extern void vcpu_executable_get_instructions(IntPtr executable, out IntPtr instr, out int instr_len);

        [DllImport(Constants.VcpuLib)]
        internal static extern int vcpu_executable_get_data_offset(IntPtr executable);

        [DllImport(Constants.VcpuLib)]
        internal static extern void vcpu_executable_destroy(IntPtr executable);

        [DllImport(Constants.VcpuLib)]
        internal static extern void vcpu_source_map_get_data(IntPtr source_map, out IntPtr data, out int data_len);

        [DllImport(Constants.VcpuLib)]
        internal static extern void vcpu_source_map_destroy(IntPtr source_map);
#pragma warning restore IDE1006 // Naming Styles

        #region Implementation
        readonly int[] _sourceMap;
        IntPtr _inner;
        #endregion
    }
}
