#if !UNITY_EDITOR

using Vcpu;
using Xunit;

namespace Test.VcpuSharp
{
    public class ProgramTests
    {
        [Fact]
        public void MinimalProgramAssembles()
        {
            var source = @"
.data
.instructions
HALT";

            var program = new Program(source);

            Assert.Equal(0, program.Data.Size);
            Assert.Equal(4, program.Instructions.Size);
        }
    }
}

#endif
