namespace Vcpu
{
    class Validation
    {
        public static void ValidateResult(int result)
        {
            if (result != 0)
            {
                throw new VcpuException(result);
            }
        }
    }
}
