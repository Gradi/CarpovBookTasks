using System;
using DummyVirtualMachine.Exceptions;
using DummyVirtualMachine.Extensions;
using IF = DummyVirtualMachine.Instructions.InstructionFactory;

namespace DummyVirtualMachine.Instructions.Decoders
{
    public class LdInstructionDecoder : IInstructionDecoder
    {
        public int StackElementsRequired => 0;

        public int FreeStackElementsRequired => 1;

        public int InstructionDataSize => Constants.PointerSize;

        public void Tick(byte opcode, Span<byte> ip, Machine machine)
        {
            var pointer = ip.AsInt32();
            if (!machine.IsValidPointer(pointer))
                throw new InvalidPointerException(pointer, machine);

            var element = machine.Memory.AsSpan(pointer);
            if (element.Length < Constants.WordSize)
                InvalidPointerException.ThrowNotEnoughSizeForWord(pointer);

            machine.Stack!.Push(element.AsInt32());
            machine.InstructionPointer += Constants.OpCodeSize + Constants.PointerSize;
        }

        public Instruction? Decode(byte opcode, ReadOnlyMemory<byte> data)
        {
            var pointer = data.AsInt32();
            return pointer >= 0 ? IF.Ld(pointer) : null;
        }
    }
}
