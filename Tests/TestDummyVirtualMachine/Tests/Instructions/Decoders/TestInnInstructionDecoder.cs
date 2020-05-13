using System.IO;
using DummyVirtualMachine;
using Moq;
using NUnit.Framework;
using IF = DummyVirtualMachine.Instructions.InstructionFactory;

namespace TestDummyVirtualMachine.Tests.Instructions.Decoders
{
    public class TestInnInstructionDecoder : BaseInstructionDecoderTest
    {
        private const int Expected = 556;

        [Test]
        public void Inn()
        {
            Assert.That(_machine.Stack!.Count, Is.EqualTo(1));
            Assert.That(_machine.Stack.Pop(), Is.EqualTo(Expected));
            Assert.That(_machine.InstructionPointer, Is.EqualTo(IF.Inn().ByteSize));
        }

        protected override Machine CreateMachine()
        {
            var mock = new Mock<IOSystem>();
            mock.Setup(s => s.Read()).Returns(Expected);

            var machine = new Machine(MemorySize, mock.Object);
            machine.Stack = new Stack<int>(new byte[MemorySize]);
            return machine;
        }

        protected override void WriteInstructions(Stream stream)
        {
            IF.Inn().Write(stream);
        }
    }
}
