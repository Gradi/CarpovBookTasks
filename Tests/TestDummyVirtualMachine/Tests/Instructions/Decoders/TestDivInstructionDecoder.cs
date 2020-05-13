using System;
using System.IO;
using NUnit.Framework;
using IF =DummyVirtualMachine.Instructions.InstructionFactory;

namespace TestDummyVirtualMachine.Tests.Instructions.Decoders
{
    public class TestDivInstructionDecoder : BaseInstructionDecoderTest
    {

        [Test]
        public void Div()
        {
            const int left = 123;
            const int right = 4;
            int expected = left / right;

            _machine.Stack!.Push(left);
            _machine.Stack.Push(right);
            _decoderTable.Tick(_machine);

            Assert.That(_machine.Stack.Count, Is.EqualTo(1));
            Assert.That(_machine.Stack.Pop(), Is.EqualTo(expected));
            Assert.That(_machine.InstructionPointer, Is.EqualTo(IF.Div().ByteSize));
        }

        [Test]
        public void DivThrowsDivideByZeroException()
        {
            _machine.Stack!.Push(123);
            _machine.Stack.Push(0);

            Assert.That(() => _decoderTable.Tick(_machine), Throws.TypeOf<DivideByZeroException>());
        }

        protected override void WriteInstructions(Stream stream)
        {
            IF.Div().Write(stream);
        }

        protected override void Tick() {}
    }
}
