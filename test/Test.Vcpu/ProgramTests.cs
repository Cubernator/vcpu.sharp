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

            using var executable = new Executable(source);

            Assert.NotNull(executable);
            Assert.Equal(0, executable.Data.Size);
            Assert.Equal(4, executable.Instructions.Size);
        }

        [Fact]
        public void MinimalProgramRuns()
        {
            var source = @"
.data
.instructions
HALT";

            using var executable = new Executable(source);
            using var processor = new Processor();
            using var memory = new PlainMemory(4);

            Assert.NotNull(executable);
            Assert.NotNull(processor);
            Assert.NotNull(memory);

            processor.Run(executable, memory);

            Assert.NotNull(processor.ExitCode);
            Assert.False(processor.ExitCode.Value.IsError);
        }

        [Fact]
        public void InvalidProgramFailsToAssemble()
        {
            var source = "INVALIDPROGRAM";

            var exception = Assert.Throws<VcpuException>(() =>
            {
                using var executable = new Executable(source);
            });
        }
    }
}
