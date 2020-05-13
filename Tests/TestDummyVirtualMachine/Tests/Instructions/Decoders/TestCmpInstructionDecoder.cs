using System.IO;
using DummyVirtualMachine.Instructions;
using NUnit.Framework;
using IF = DummyVirtualMachine.Instructions.InstructionFactory;

namespace TestDummyVirtualMachine.Tests.Instructions.Decoders
{
    public class TestCmpInstructionDecoder : BaseInstructionDecoderTest
    {

        [TestCase(CmpInstruction.CmpType.Equal, 1, 1, ExpectedResult = 1)]
        [TestCase(CmpInstruction.CmpType.Equal, 2, 1, ExpectedResult = 0)]

        [TestCase(CmpInstruction.CmpType.NotEqual, 2, 1, ExpectedResult = 1)]
        [TestCase(CmpInstruction.CmpType.NotEqual, 2, 2, ExpectedResult = 0)]

        [TestCase(CmpInstruction.CmpType.LessThan, 1, 2, ExpectedResult = 1)]
        [TestCase(CmpInstruction.CmpType.LessThan, 2, 2, ExpectedResult = 0)]
        [TestCase(CmpInstruction.CmpType.LessThan, 2, 1, ExpectedResult = 0)]

        [TestCase(CmpInstruction.CmpType.LessOrEqualThan, 1, 1, ExpectedResult = 1)]
        [TestCase(CmpInstruction.CmpType.LessOrEqualThan, 1, 2, ExpectedResult = 1)]
        [TestCase(CmpInstruction.CmpType.LessOrEqualThan, 2, 1, ExpectedResult = 0)]

        [TestCase(CmpInstruction.CmpType.GreaterThan, 2, 1, ExpectedResult = 1)]
        [TestCase(CmpInstruction.CmpType.GreaterThan, 2, 2, ExpectedResult = 0)]
        [TestCase(CmpInstruction.CmpType.GreaterThan, 1, 2, ExpectedResult = 0)]

        [TestCase(CmpInstruction.CmpType.GreaterOrEqualThan, 2, 1, ExpectedResult = 1)]
        [TestCase(CmpInstruction.CmpType.GreaterOrEqualThan, 2, 2, ExpectedResult = 1)]
        [TestCase(CmpInstruction.CmpType.GreaterOrEqualThan, 1, 2, ExpectedResult = 0)]
        public int Cmp(CmpInstruction.CmpType type, int left, int right)
        {
            using (var stream = new MemoryStream(_machine.Memory))
            {
                IF.Cmp(type).Write(stream);
            }
            _machine.Stack!.Push(left);
            _machine.Stack.Push(right);

            _decoderTable.Tick(_machine);
            Assert.That(_machine.Stack.Count, Is.EqualTo(1));
            Assert.That(_machine.InstructionPointer, Is.EqualTo(IF.Cmp(type).ByteSize));
            return _machine.Stack.Pop();
        }

        protected override void WriteInstructions(Stream stream) {}

        protected override void Tick() {}
    }
}
