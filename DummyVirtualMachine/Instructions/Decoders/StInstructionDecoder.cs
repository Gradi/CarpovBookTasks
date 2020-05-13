using System;
using DummyVirtualMachine.Exceptions;
using DummyVirtualMachine.Extensions;
using IF = DummyVirtualMachine.Instructions.InstructionFactory;

namespace DummyVirtualMachine.Instructions.Decoders
{
    public class StInstructionDecoder : IInstructionDecoder
    {
        public int StackElementsRequired => 1;

        public int FreeStackElementsRequired => 0;

        public int InstructionDataSize => Constants.PointerSize;

        public void Tick(byte opcode, Span<byte> ip, Machine machine)
        {
            var pointer = ip.AsInt32();
            if (!machine.IsValidPointer(pointer))
                throw new InvalidPointerException(pointer, machine);

            var destination = machine.Memory.AsSpan(pointer);
            if (destination.Length < Constants.WordSize)
                InvalidPointerException.ThrowNotEnoughSizeForWord(pointer);

            var element = machine.Stack!.Pop();
            var result = BitConverter.TryWriteBytes(destination, element);
            System.Diagnostics.Debug.Assert(result);
            machine.InstructionPointer += Constants.OpCodeSize + Constants.PointerSize;
        }

        public Instruction? Decode(byte opcode, ReadOnlyMemory<byte> data)
        {
            var pointer = data.AsInt32();
            return pointer >= 0 ? IF.St(pointer) : null;
        }
    }
}
