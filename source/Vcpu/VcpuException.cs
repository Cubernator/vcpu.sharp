using System;

namespace Vcpu
{

    public class VcpuException : Exception
    {
        public VcpuException(int code)
        {
            Code = code;
        }

        public VcpuException(int code, string message) : base(message)
        {
            Code = code;
        }

        public int Code { get; }
    }
}
