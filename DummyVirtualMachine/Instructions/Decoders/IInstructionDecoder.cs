using System;

namespace DummyVirtualMachine.Instructions.Decoders
{
    /// <summary>
    /// Interface for decoding instructions and running single tick on machine.
    /// </summary>
    public interface IInstructionDecoder
    {
        /// <summary>
        /// Amount of elements on top of stack
        /// to run current instruction.
        /// If zero then stack is not required.
        /// </summary>
        int StackElementsRequired { get; }

        /// <summary>
        /// Amount of free space on stack
        /// to run current instruction.
        /// If zero then stack is not required.
        /// </summary>
        int FreeStackElementsRequired { get; }

        /// <summary>
        /// Size in bytes of instruction data(e.g. arguments)
        /// without opcode itself.
        /// </summary>
        int InstructionDataSize { get; }

        /// <summary>
        /// Decodes current instruction and performs necessary
        /// action on machine.
        /// </summary>
        /// <param name="opcode">Current opcode. Equals to this decoder's supported instruction.</param>
        /// <param name="ip">If <see cref="InstructionDataSize"/> is not zero then <paramref name="ip"/>  will hold current instruction's data.</param>
        /// <param name="machine">Machine to tick</param>
        void Tick(byte opcode, Span<byte> ip, Machine machine);

        /// <summary>
        /// Decodes current instruction.
        /// </summary>
        /// <param name="opcode">Current opcode. Equals to this decoder's supported instruction.</param>
        /// <param name="data">If <see cref="InstructionDataSize"/> is not zero then <paramref name="data"/> will hold current instruction's data.</param>
        /// <returns>Decoded instruction. May return null if decoder can't decode instruction arguments (for example, due to arguments have out of range value).</returns>
        Instruction? Decode(byte opcode, ReadOnlyMemory<byte> data);
    }
}
