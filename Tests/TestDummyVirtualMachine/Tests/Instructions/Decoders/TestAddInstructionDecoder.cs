using System.IO;
using NUnit.Framework;
using IF = DummyVirtualMachine.Instructions.InstructionFactory;

namespace TestDummyVirtualMachine.Tests.Instructions.Decoders
{
    public class TestAddInstructionDecoder : BaseInstructionDecoderTest
    {
        private const int Left = 123;
        private const int Right = 444;
        private const int Expected = Left + Right;

        [Test]
        public void Adds()
        {
            Assert.That(_machine.Stack!.Count, Is.EqualTo(1));
            Assert.That(_machine.Stack.Pop(), Is.EqualTo(Expected));
            Assert.That(_machine.InstructionPointer, Is.EqualTo(IF.Add().ByteSize));
        }

        protected override void WriteInstructions(Stream stream)
        {
            IF.Add().Write(stream);
            _machine.Stack!.Push(Left);
            _machine.Stack.Push(Right);
        }
    }
}
