using System;
using IF = DummyVirtualMachine.Instructions.InstructionFactory;

namespace DummyVirtualMachine.Instructions.Decoders
{
    public class PrnInstructionDecoder : IInstructionDecoder
    {
        public int StackElementsRequired => 1;

        public int FreeStackElementsRequired => 0;

        public int InstructionDataSize => 0;

        public void Tick(byte opcode, Span<byte> ip, Machine machine)
        {
            machine.IOSystem.Write(machine.Stack!.Pop());
            machine.InstructionPointer += Constants.OpCodeSize;
        }

        public Instruction? Decode(byte opcode, ReadOnlyMemory<byte> data) => IF.Prn();
    }
}
