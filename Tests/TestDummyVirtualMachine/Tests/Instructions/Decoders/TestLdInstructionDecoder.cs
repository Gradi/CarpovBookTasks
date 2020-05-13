using System.IO;
using System.Text;
using DummyVirtualMachine;
using NUnit.Framework;
using IF = DummyVirtualMachine.Instructions.InstructionFactory;

namespace TestDummyVirtualMachine.Tests.Instructions.Decoders
{
    public class TestLdInstructionDecoder : BaseInstructionDecoderTest
    {
        private const int Expected = int.MaxValue - 123;

        [Test]
        public void Ld()
        {
            Assert.That(_machine.Stack!.Count, Is.EqualTo(1));
            Assert.That(_machine.Stack.Pop(), Is.EqualTo(Expected));
            Assert.That(_machine.InstructionPointer, Is.EqualTo(IF.Ld(0).ByteSize + Constants.WordSize));
        }

        protected override void WriteInstructions(Stream stream)
        {
            using var writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true);
            writer.Write(Expected);
            writer.Flush();
            IF.Ld(0).Write(stream);
            _machine.InstructionPointer = Constants.WordSize;
        }
    }
}
