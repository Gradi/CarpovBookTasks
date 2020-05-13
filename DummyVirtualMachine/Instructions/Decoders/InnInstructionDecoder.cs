using System;
using IF = DummyVirtualMachine.Instructions.InstructionFactory;

namespace DummyVirtualMachine.Instructions.Decoders
{
    public class InnInstructionDecoder : IInstructionDecoder
    {
        public int StackElementsRequired => 0;

        public int FreeStackElementsRequired => 1;

        public int InstructionDataSize => 0;

        public void Tick(byte opcode, Span<byte> ip, Machine machine)
        {
            machine.Stack!.Push(machine.IOSystem.Read());
            machine.InstructionPointer += Constants.OpCodeSize;
        }

        public Instruction? Decode(byte opcode, ReadOnlyMemory<byte> data) => IF.Inn();
    }
}
