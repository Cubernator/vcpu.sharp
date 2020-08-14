using System;
using System.Runtime.InteropServices;

namespace Vcpu
{
    public class IoMemory : ContiguousMemory
    {
        public IoMemory(int size, IIoHandler handler)
            : this(size, GCHandle.Alloc(new IoHandlerWrapper(handler)))
        {
        }

        #region Overrides
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            _handle.Free();
        }
        #endregion

        internal delegate bool CanWriteCallback(IntPtr data, int dataLen, int address, int size, IntPtr userData);
        internal delegate void OnWriteCallback(IntPtr data, int dataLen, int address, int size, IntPtr userData);

#pragma warning disable IDE1006 // Naming Styles
        [DllImport(Constants.VcpuLib)]
        internal static extern IntPtr vcpu_memory_create_io(int size, CanWriteCallback can_write, OnWriteCallback on_write, IntPtr user_data);
#pragma warning restore IDE1006 // Naming Styles

        #region Implementation
        GCHandle _handle;

        IoMemory(int size, GCHandle handle)
            : base(vcpu_memory_create_io(size, CanWrite, OnWrite, GCHandle.ToIntPtr(handle)))
        {
            _handle = handle;
        }

        static bool CanWrite(IntPtr data, int dataLen, int address, int size, IntPtr userData)
        {
            var handlerWrapper = (IoHandlerWrapper)GCHandle.FromIntPtr(userData).Target;
            return handlerWrapper.Inner.CanWrite(data, dataLen, address, size);
        }

        static void OnWrite(IntPtr data, int dataLen, int address, int size, IntPtr userData)
        {
            var handlerWrapper = (IoHandlerWrapper)GCHandle.FromIntPtr(userData).Target;
            handlerWrapper.Inner.OnWrite(data, dataLen, address, size);
        }

        class IoHandlerWrapper
        {
            public IoHandlerWrapper(IIoHandler inner)
            {
                Inner = inner ?? throw new ArgumentNullException(nameof(inner));
            }

            public IIoHandler Inner { get; }
        }
        #endregion
    }
}
