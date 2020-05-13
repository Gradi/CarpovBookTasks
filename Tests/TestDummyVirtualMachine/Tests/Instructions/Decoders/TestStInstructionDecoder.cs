using System;
using System.IO;
using DummyVirtualMachine;
using NUnit.Framework;
using IF = DummyVirtualMachine.Instructions.InstructionFactory;

namespace TestDummyVirtualMachine.Tests.Instructions.Decoders
{
    public class TestStInstructionDecoder : BaseInstructionDecoderTest
    {
        private const int Expected = int.MaxValue - 2222;

        [Test]
        public void St()
        {
            Assert.That(_machine.Stack!.Count, Is.Zero);
            Assert.That(_machine.InstructionPointer, Is.EqualTo(IF.St(0).ByteSize + Constants.WordSize));

            Assert.That(BitConverter.ToInt32(_machine.Memory.AsSpan(0, Constants.WordSize)), Is.EqualTo(Expected));
        }

        protected override void WriteInstructions(Stream stream)
        {
            stream.Seek(Constants.WordSize, SeekOrigin.Begin);
            IF.St(0).Write(stream);
            _machine.Stack!.Push(Expected);
            _machine.InstructionPointer = Constants.WordSize;
        }
    }
}
