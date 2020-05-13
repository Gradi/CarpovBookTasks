using System;
using System.Collections.Generic;
using System.IO;

namespace DummyVirtualMachine.Instructions.Decoders
{
    /// <summary>
    /// Table of <see cref="IInstructionDecoder"/>.
    /// </summary>
    public interface IInstructionDecoderTable
    {
        /// <summary>
        /// Decoders dictionary.
        /// Key: opcode, Value: decoder for that opcode.
        /// </summary>
        IReadOnlyDictionary<byte, IInstructionDecoder> Decoders { get; }

        /// <summary>
        /// Fetches next instruction, decodes it and runs single tick.
        /// </summary>
        void Tick(Machine machine);

        /// <summary>
        /// Reads all instructions from the stream and tries to decode them.
        /// </summary>
        IEnumerable<(Instruction? Instruction, long Offset)> DecodeAll(Stream stream);

        /// <summary>
        /// Reads all instructions from the span and tries to decode them.
        /// </summary>
        IEnumerable<(Instruction? Instruction, long Offset)> DecodeAll(ReadOnlyMemory<byte> span);
    }
}
