using System.IO;
using NUnit.Framework;
using IF = DummyVirtualMachine.Instructions.InstructionFactory;

namespace TestDummyVirtualMachine.Tests.Instructions.Decoders
{
    public class TestSubInstructionDecoder : BaseInstructionDecoderTest
    {
        private const int Left = 123;
        private const int Right = 100;
        private const int Expected = Left - Right;

        [Test]
        public void Sub()
        {
            Assert.That(_machine.Stack!.Count, Is.EqualTo(1));
            Assert.That(_machine.Stack.Pop(), Is.EqualTo(Expected));
            Assert.That(_machine.InstructionPointer, Is.EqualTo(IF.Sub().ByteSize));
        }

        protected override void WriteInstructions(Stream stream)
        {
            IF.Sub().Write(stream);
            _machine.Stack!.Push(Left);
            _machine.Stack.Push(Right);
        }
    }
}
