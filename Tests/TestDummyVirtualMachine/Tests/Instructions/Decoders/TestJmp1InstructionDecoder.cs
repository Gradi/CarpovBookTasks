using System.IO;
using NUnit.Framework;
using IF = DummyVirtualMachine.Instructions.InstructionFactory;

namespace TestDummyVirtualMachine.Tests.Instructions.Decoders
{
    public class TestJmp1InstructionDecoder : BaseInstructionDecoderTest
    {
        [Test]
        public void Jmps()
        {
            _machine.Stack!.Push(1);
            _decoderTable.Tick(_machine);

            Assert.That(_machine.Stack.Count, Is.Zero);
            Assert.That(_machine.InstructionPointer, Is.EqualTo(MemorySize - 1));
        }

        [Test]
        public void NotJmpsIfZero()
        {
            _machine.Stack!.Push(0);
            _decoderTable.Tick(_machine);

            Assert.That(_machine.Stack.Count, Is.Zero);
            Assert.That(_machine.InstructionPointer, Is.EqualTo(IF.Jmp1(MemorySize - 1).ByteSize));
        }

        protected override void WriteInstructions(Stream stream)
        {
            IF.Jmp1(MemorySize - 1).Write(stream);
        }

        protected override void Tick() {}
    }
}
