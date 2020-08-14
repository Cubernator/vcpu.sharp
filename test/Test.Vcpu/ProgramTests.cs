using Vcpu;
using Xunit;

namespace Test.Vcpu
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

            using var program = new Program(source);

            Assert.NotNull(program);
            Assert.Equal(0, program.Data.Size);
            Assert.Equal(4, program.Instructions.Size);
        }

        [Fact]
        public void MinimalProgramRuns()
        {
            var source = @"
.data
.instructions
HALT";

            using var program = new Program(source);
            using var processor = new Processor();
            using var memory = new PlainMemory(4);

            Assert.NotNull(program);
            Assert.NotNull(processor);
            Assert.NotNull(memory);

            processor.Run(program, memory);

            Assert.NotNull(processor.ExitCode);
            Assert.False(processor.ExitCode.Value.IsError);
        }

        [Fact]
        public void InvalidProgramFailsToAssemble()
        {
            var source = "INVALIDPROGRAM";

            var exception = Assert.Throws<VcpuException>(() =>
            {
                using var program = new Program(source);
            });
        }
    }
}
