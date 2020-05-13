using System;
using DummyVirtualMachine.Exceptions;
using DummyVirtualMachine.Extensions;
using IF = DummyVirtualMachine.Instructions.InstructionFactory;

namespace DummyVirtualMachine.Instructions.Decoders
{
    public class StkInstructionDecoder : IInstructionDecoder
    {
        public int StackElementsRequired => 0;

        public int FreeStackElementsRequired => 0;

        public int InstructionDataSize => Constants.PointerSize + Constants.WordSize;

        public void Tick(byte opcode, Span<byte> ip, Machine machine)
        {
            var pointer = ip.AsInt32();
            if (!machine.IsValidPointer(pointer))
                throw new InvalidPointerException(pointer, machine);

            var wordCount = ip.Slice(Constants.PointerSize).AsInt32();
            if (wordCount < 0)
                throw new InvalidInstructionException(opcode, $"Word count can't be < 0 ({wordCount}).");
            var size = wordCount * Constants.WordSize;
            if ((pointer + size) > machine.Memory.Length)
                throw new InvalidInstructionException(opcode, $"Pointer + word count is greater than amount of available memory (Pointer: {pointer:X8}, Stack size: {size:X8}).");

            machine.Stack = new Stack<int>(machine.Memory.AsMemory(pointer, size));
            machine.InstructionPointer += Constants.OpCodeSize + Constants.PointerSize + Constants.WordSize;
        }

        public Instruction? Decode(byte opcode, ReadOnlyMemory<byte> data)
        {
            var pointer = data.AsInt32();
            var wordCount = data.Slice(Constants.PointerSize).AsInt32();

            return pointer >= 0 && wordCount >= 0 ? IF.Stk(pointer, wordCount) : null;
        }
    }
}
