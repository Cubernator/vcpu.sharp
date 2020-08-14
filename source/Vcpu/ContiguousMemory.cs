using System;

namespace Vcpu
{
    public abstract class ContiguousMemory : Memory
    {
        public DataRef Data
        {
            get
            {
                Validation.ValidateResult(vcpu_memory_get_ptr(Inner, out var ptr, out var size));
                return new DataRef(ptr, size, false);
            }
        }

        public void CopyFrom(DataRef src, int srcOffset, int length, int destOffset)
        {
            if (srcOffset >= src.Size)
            {
                throw new ArgumentOutOfRangeException(nameof(srcOffset));
            }

            var destSize = Data.Size;

            if (destOffset >= destSize)
            {
                throw new ArgumentOutOfRangeException(nameof(destOffset));
            }

            if (srcOffset + length > src.Size || destOffset + length > destSize)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            Validation.ValidateResult(vcpu_memory_write(Inner, src.Pointer + srcOffset, destOffset, length));
        }

        public void Resize(int size) => Validation.ValidateResult(vcpu_memory_resize(Inner, size));

        protected ContiguousMemory(IntPtr inner) : base(inner)
        {
        }
    }
}
