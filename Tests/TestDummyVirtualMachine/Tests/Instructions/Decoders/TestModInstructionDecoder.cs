using System;
using System.IO;
using NUnit.Framework;
using IF = DummyVirtualMachine.Instructions.InstructionFactory;

namespace TestDummyVirtualMachine.Tests.Instructions.Decoders
{
    public class TestModInstructionDecoder : BaseInstructionDecoderTest
    {
        [Test]
        public void Mod()
        {
            const int left = 987;
            const int right = 56;
            const int expected = left % right;

            _machine.Stack!.Push(left);
            _machine.Stack.Push(right);
            _decoderTable.Tick(_machine);

            Assert.That(_machine.Stack.Count, Is.EqualTo(1));
            Assert.That(_machine.Stack.Pop(), Is.EqualTo(expected));
            Assert.That(_machine.InstructionPointer, Is.EqualTo(IF.Mod().ByteSize));
        }

        [Test]
        public void ModThrowsDivideByZeroException()
        {
            _machine.Stack!.Push(1231);
            _machine.Stack.Push(0);

            Assert.That(() =>  _decoderTable.Tick(_machine), Throws.TypeOf<DivideByZeroException>());
        }

        protected override void WriteInstructions(Stream stream)
        {
            IF.Mod().Write(stream);
        }

        protected override void Tick() {}
    }
}
