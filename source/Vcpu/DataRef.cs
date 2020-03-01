using System;
using System.Runtime.InteropServices;

namespace Vcpu
{
    public ref struct DataRef
    {
        internal DataRef(IntPtr pointer, int size, bool isReadonly)
        {
            Pointer = pointer;
            Size = size;
            IsReadonly = isReadonly;
        }

        public int Size { get; }
        public bool IsReadonly { get; }

        public DataRef AsReadOnly => new DataRef(Pointer, Size, true);

        public DataRef SubRange(int offset, int size)
        {
            if (offset < 0 || offset > Size)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

            if (offset + size > Size)
            {
                throw new ArgumentOutOfRangeException(nameof(size));
            }

            return new DataRef(Pointer + offset, size, IsReadonly);
        }

        public void Read(int srcOffset, int length, byte[] dest, int destOffset)
        {
            if (dest is null)
            {
                throw new ArgumentNullException(nameof(dest));
            }

            if (destOffset >= dest.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(destOffset));
            }

            if (srcOffset < 0 || srcOffset >= Size)
            {
                throw new ArgumentOutOfRangeException(nameof(srcOffset));
            }

            if (srcOffset + length > Size || destOffset + length > dest.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            Marshal.Copy(Pointer + srcOffset, dest, destOffset, length);
        }

        public byte[] Read(int offset, int length)
        {
            var dest = new byte[length];

            Read(offset, length, dest, length);

            return dest;
        }

        public void Write(byte[] src, int srcOffset, int length, int destOffset)
        {
            if (src is null)
            {
                throw new ArgumentNullException(nameof(src));
            }

            if (srcOffset >= src.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(srcOffset));
            }

            if (destOffset >= Size)
            {
                throw new ArgumentOutOfRangeException(nameof(destOffset));
            }

            if (srcOffset + length > src.Length || destOffset + length > Size)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            Marshal.Copy(src, srcOffset, Pointer + destOffset, length);
        }

        public static void Copy(DataRef src, DataRef dst)
        {
            if (src.Size != dst.Size)
            {
                throw new InvalidOperationException("Memory regions must have the same length.");
            }

            _ = vcpu_memcpy(dst.Pointer, src.Pointer, src.Size);
        }

        internal IntPtr Pointer { get; }

#pragma warning disable IDE1006 // Naming Styles
        [DllImport("vcpu_interop")]
        internal static extern IntPtr vcpu_memcpy(IntPtr dst, IntPtr src, int length);
#pragma warning restore IDE1006 // Naming Styles
    }
}
