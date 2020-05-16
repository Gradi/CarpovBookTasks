using System.IO;
using DummyVirtualMachine;
using DummyVirtualMachine.Instructions.Decoders;
using Moq;
using NUnit.Framework;

namespace TestDummyVirtualMachine.Tests.Instructions.Decoders
{
    [TestFixture]
    public abstract class BaseInstructionDecoderTest
    {
        protected const int MemorySize = 1024;

        protected readonly IInstructionDecoderTable _decoderTable;

        protected Machine _machine;

        protected BaseInstructionDecoderTest()
        {
            _decoderTable = new InstructionDecoderTable();
            _machine = null!;
        }

        [SetUp]
        public void Setup()
        {
            _machine = CreateMachine();
            using (var stream = new MemoryStream(_machine.Memory))
            {
                WriteInstructions(stream);
            }
            Tick();
        }

        protected virtual Machine CreateMachine()
        {
            var machine = new Machine(MemorySize, Mock.Of<IOSystem>());
            machine.Stack = new Stack<int>(new byte[MemorySize]);
            return machine;
        }

        protected virtual void Tick() => _decoderTable.Tick(_machine);

        protected abstract void WriteInstructions(Stream stream);
    }
}
