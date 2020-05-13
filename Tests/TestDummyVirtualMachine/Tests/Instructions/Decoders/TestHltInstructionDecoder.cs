using System.IO;
using NUnit.Framework;
using IF = DummyVirtualMachine.Instructions.InstructionFactory;

namespace TestDummyVirtualMachine.Tests.Instructions.Decoders
{
    public class TestHltInstructionDecoder : BaseInstructionDecoderTest
    {
        [Test]
        public void Hlt()
        {
            Assert.That(_machine.IsHalted, Is.True);
            Assert.That(_machine.InstructionPointer, Is.EqualTo(IF.Hlt().ByteSize));
        }

        protected override void WriteInstructions(Stream stream)
        {
            IF.Hlt().Write(stream);
        }
    }
}
