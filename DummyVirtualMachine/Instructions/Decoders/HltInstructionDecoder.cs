using System;
using IF = DummyVirtualMachine.Instructions.InstructionFactory;

namespace DummyVirtualMachine.Instructions.Decoders
{
    public class HltInstructionDecoder : IInstructionDecoder
    {
        public int StackElementsRequired => 0;

        public int FreeStackElementsRequired => 0;

        public int InstructionDataSize => 0;

        public void Tick(byte opcode, Span<byte> ip, Machine machine)
        {
            machine.IsHalted = true;
            machine.InstructionPointer += Constants.OpCodeSize;
        }

        public Instruction? Decode(byte opcode, ReadOnlyMemory<byte> data) => IF.Hlt();
    }
}
