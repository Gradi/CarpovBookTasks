using System;
using DummyVirtualMachine.Exceptions;
using IF = DummyVirtualMachine.Instructions.InstructionFactory;

namespace DummyVirtualMachine.Instructions.Decoders
{
    public class CmpInstructionDecoder : IInstructionDecoder
    {
        public int StackElementsRequired => 2;

        public int FreeStackElementsRequired => 1;

        public int InstructionDataSize => sizeof(CmpInstruction.CmpType);

        public void Tick(byte opcode, Span<byte> ip, Machine machine)
        {
            var right = machine.Stack!.Pop();
            var left = machine.Stack.Pop();

            var type = (CmpInstruction.CmpType)ip[0];
            var result = type switch
            {
                CmpInstruction.CmpType.Equal => left == right,
                CmpInstruction.CmpType.NotEqual => left != right,
                CmpInstruction.CmpType.LessThan => left < right,
                CmpInstruction.CmpType.GreaterThan => left > right,
                CmpInstruction.CmpType.LessOrEqualThan => left <= right,
                CmpInstruction.CmpType.GreaterOrEqualThan => left >= right,
                var val => throw new InvalidInstructionException(opcode, $"'{IF.CmpEqual().Mnemonic}' instruction has argument out of range ({val}).")
            };
            machine.Stack.Push(result ? 1 : 0);
            machine.InstructionPointer += Constants.OpCodeSize + sizeof(CmpInstruction.CmpType);
        }

        public Instruction? Decode(byte opcode, ReadOnlyMemory<byte> data)
        {
            var type = data.Span[0];
            return Enum.IsDefined(typeof(CmpInstruction.CmpType), type) ? IF.Cmp((CmpInstruction.CmpType)type) : null;
        }
    }
}
