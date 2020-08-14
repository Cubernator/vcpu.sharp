using Vcpu;
using Xunit;

namespace Test.Vcpu
{
    public class IoMemoryTests
    {
        [Fact]
        public void CanWriteToIoMemory()
        {
            var ioHandler = new DelegateIoHandler((data, dataLen, address, size) => true, (data, dataLen, address, size) => { });
            using var ioMemory = new IoMemory(1, ioHandler);

            ioMemory.SetByte(0, 1);

            var data = ioMemory.Data.Read(0, 1);
            Assert.Equal(1, data[0]);
        }
    }
}
