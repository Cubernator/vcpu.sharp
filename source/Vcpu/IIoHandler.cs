using System;

namespace Vcpu
{
    public interface IIoHandler
    {
        bool CanWrite(IntPtr data, int dataLen, int address, int size);
        void OnWrite(IntPtr data, int dataLen, int address, int size);
    }
}
