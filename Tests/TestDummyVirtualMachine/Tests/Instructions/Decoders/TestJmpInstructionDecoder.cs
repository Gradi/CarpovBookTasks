using System.IO;
using NUnit.Framework;
using IF = DummyVirtualMachine.Instructions.InstructionFactory;

namespace TestDummyVirtualMachine.Tests.Instructions.Decoders
{
    public class TestJmpInstructionDecoder : BaseInstructionDecoderTest
    {
        [Test]
        public void Jmps()
        {
            Assert.That(_machine.InstructionPointer, Is.EqualTo(MemorySize - 1));
        }

        protected override void WriteInstructions(Stream stream)
        {
            IF.Jmp(MemorySize - 1).Write(stream);
        }
    }
}
