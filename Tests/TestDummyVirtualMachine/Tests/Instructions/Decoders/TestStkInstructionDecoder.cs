using System.IO;
using DummyVirtualMachine;
using NUnit.Framework;
using IF = DummyVirtualMachine.Instructions.InstructionFactory;

namespace TestDummyVirtualMachine.Tests.Instructions.Decoders
{
    public class TestStkInstructionDecoder : BaseInstructionDecoderTest
    {
        [Test]
        public void Stk()
        {
            Assert.That(_machine.Stack, Is.Not.Null);
            Assert.That(_machine.Stack.Count, Is.Zero);
            Assert.That(_machine.Stack.MaxElements, Is.EqualTo(1));
            Assert.That(_machine.InstructionPointer, Is.EqualTo(IF.Stk(0, 0).ByteSize));
        }

        protected override void WriteInstructions(Stream stream)
        {
            IF.Stk(MemorySize - Constants.WordSize, 1).Write(stream);
            _machine.Stack = null;
        }
    }
}
