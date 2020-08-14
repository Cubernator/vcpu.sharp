using System;

namespace Vcpu
{
    public class DelegateIoHandler : IIoHandler
    {
        public DelegateIoHandler(Func<IntPtr, int, int, int, bool> canWrite, Action<IntPtr, int, int, int> onWrite)
        {
            _canWrite = canWrite ?? throw new ArgumentNullException(nameof(canWrite));
            _onWrite = onWrite ?? throw new ArgumentNullException(nameof(onWrite));
        }

        #region Implementation
        readonly Func<IntPtr, int, int, int, bool> _canWrite;
        readonly Action<IntPtr, int, int, int> _onWrite;
        #endregion

        #region IIoHandler
        bool IIoHandler.CanWrite(IntPtr data, int dataLen, int address, int size) => _canWrite(data, dataLen, address, size);
        void IIoHandler.OnWrite(IntPtr data, int dataLen, int address, int size) => _onWrite(data, dataLen, address, size);
        #endregion
    }
}
