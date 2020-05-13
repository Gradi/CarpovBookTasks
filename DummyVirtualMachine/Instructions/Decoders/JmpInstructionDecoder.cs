using System;
using DummyVirtualMachine.Exceptions;
using DummyVirtualMachine.Extensions;
using IF = DummyVirtualMachine.Instructions.InstructionFactory;

namespace DummyVirtualMachine.Instructions.Decoders
{
    public class JmpInstructionDecoder : IInstructionDecoder
    {
        public int StackElementsRequired => 0;

        public int FreeStackElementsRequired => 0;

        public int InstructionDataSize => Constants.PointerSize;

        public void Tick(byte opcode, Span<byte> ip, Machine machine)
        {
            int pointer = ip.AsInt32();
            if (!machine.IsValidPointer(pointer))
                throw new InvalidPointerException(pointer, machine);

            machine.InstructionPointer = pointer;
        }

        public Instruction? Decode(byte opcode, ReadOnlyMemory<byte> data)
        {
            int pointer = data.AsInt32();
            return pointer >= 0 ? IF.Jmp(pointer) : null;
        }
    }
}
