using System.IO;
using NUnit.Framework;
using IF = DummyVirtualMachine.Instructions.InstructionFactory;

namespace TestDummyVirtualMachine.Tests.Instructions.Decoders
{
    public class TestMulInstructionDecoder : BaseInstructionDecoderTest
    {
        private const int Left = 876;
        private const int Right = 344;
        private const int Expected = Left * Right;

        [Test]
        public void Mul()
        {
            Assert.That(_machine.Stack!.Count, Is.EqualTo(1));
            Assert.That(_machine.Stack.Pop(), Is.EqualTo(Expected));
            Assert.That(_machine.InstructionPointer, Is.EqualTo(IF.Mul().ByteSize));
        }

        protected override void WriteInstructions(Stream stream)
        {
            IF.Mul().Write(stream);
            _machine.Stack!.Push(Left);
            _machine.Stack!.Push(Right);
        }
    }
}
