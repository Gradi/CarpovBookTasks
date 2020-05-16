using System;
using System.IO;
using DummyVirtualMachine;
using Moq;
using NUnit.Framework;
using IF = DummyVirtualMachine.Instructions.InstructionFactory;

namespace TestDummyVirtualMachine.Tests.Instructions.Decoders
{
    public class TestPrnInstructionDecoder : BaseInstructionDecoderTest
    {
        private const int Expected = Int32.MaxValue - 124;

        private Mock<IOSystem> _ioSystem = null!;

        [Test]
        public void Prn()
        {
            Assert.That(_machine.Stack!.Count, Is.Zero);
            Assert.That(_machine.InstructionPointer, Is.EqualTo(IF.Prn().ByteSize));

            _ioSystem.Verify(m => m.Write(Expected), Times.Once);
        }


        protected override Machine CreateMachine()
        {
            _ioSystem = new Mock<IOSystem>();
            var machine = new Machine(MemorySize, _ioSystem.Object);
            machine.Stack = new Stack<int>(new byte[MemorySize]);
            return machine;
        }

        protected override void WriteInstructions(Stream stream)
        {
            IF.Prn().Write(stream);
            _machine.Stack!.Push(Expected);
        }
    }
}
