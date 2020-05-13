using System;
using IF = DummyVirtualMachine.Instructions.InstructionFactory;

namespace DummyVirtualMachine.Instructions.Decoders
{
    public class DivInstructionDecoder : IInstructionDecoder
    {
        public int StackElementsRequired => 2;

        public int FreeStackElementsRequired => 1;

        public int InstructionDataSize => 0;

        public void Tick(byte opcode, Span<byte> ip, Machine machine)
        {
            var right = machine.Stack!.Pop();
            var left = machine.Stack.Pop();

            machine.Stack.Push(left / right);
            machine.InstructionPointer += Constants.OpCodeSize;
        }

        public Instruction? Decode(byte opcode, ReadOnlyMemory<byte> data) => IF.Div();
    }
}
