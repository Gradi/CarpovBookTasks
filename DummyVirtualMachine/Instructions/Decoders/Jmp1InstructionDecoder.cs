using System;
using DummyVirtualMachine.Exceptions;
using DummyVirtualMachine.Extensions;
using IF = DummyVirtualMachine.Instructions.InstructionFactory;

namespace DummyVirtualMachine.Instructions.Decoders
{
    public class Jmp1InstructionDecoder : IInstructionDecoder
    {
        public int StackElementsRequired => 1;

        public int FreeStackElementsRequired => 0;

        public int InstructionDataSize => Constants.PointerSize;

        public void Tick(byte opcode, Span<byte> ip, Machine machine)
        {
            var pointer = ip.AsInt32();
            if (!machine.IsValidPointer(pointer))
                throw new InvalidPointerException(pointer, machine);

            var element = machine.Stack!.Pop();
            if (element != 0)
            {
                machine.InstructionPointer = pointer;
            }
            else
            {
                machine.InstructionPointer += Constants.OpCodeSize + Constants.PointerSize;
            }
        }

        public Instruction? Decode(byte opcode, ReadOnlyMemory<byte> data)
        {
            var pointer = data.AsInt32();
            return pointer >= 0 ? IF.Jmp1(pointer) : null;
        }
    }
}
