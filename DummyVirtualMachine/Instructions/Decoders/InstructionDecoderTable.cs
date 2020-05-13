using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DummyVirtualMachine.Exceptions;
using DummyVirtualMachine.Extensions;

namespace DummyVirtualMachine.Instructions.Decoders
{
    public class InstructionDecoderTable : IInstructionDecoderTable
    {
        private readonly IInstructionDecoder[] _decoders;

        private IReadOnlyDictionary<byte, IInstructionDecoder>? _decodersDictionary;

        public IReadOnlyDictionary<byte, IInstructionDecoder> Decoders =>
            _decodersDictionary ??= _decoders
                .WithIndexes()
                .Where(i => i.Item != null).ToDictionary(i => (byte)i.Index, i => i.Item);


        public InstructionDecoderTable()
        {
            _decoders = new IInstructionDecoder[(int)byte.MaxValue];
            _decoders[0x00] = new HltInstructionDecoder();
            _decoders[0x01] = new JmpInstructionDecoder();
            _decoders[0x02] = new Jmp0InstructionDecoder();
            _decoders[0x03] = new Jmp1InstructionDecoder();
            _decoders[0x04] = new InnInstructionDecoder();
            _decoders[0x05] = new PrnInstructionDecoder();
            _decoders[0x06] = new LdInstructionDecoder();
            _decoders[0x07] = new StInstructionDecoder();
            _decoders[0x08] = new AddInstructionDecoder();
            _decoders[0x09] = new SubInstructionDecoder();
            _decoders[0x0A] = new MulInstructionDecoder();
            _decoders[0x0B] = new DivInstructionDecoder();
            _decoders[0x0C] = new ModInstructionDecoder();
            _decoders[0x0D] = new CmpInstructionDecoder();
            _decoders[0x0E] = new StkInstructionDecoder();
        }

        public void Tick(Machine machine)
        {
            if (machine.IsHalted)
                throw new InvalidMachineStateException("Machine is halted.");
            if (machine.InstructionPointer < 0 || machine.InstructionPointer >= machine.Memory.Length)
                throw new InvalidMachineStateException($"IP points to invalid memory region (IP:{machine.InstructionPointer:X8}, memory size: {machine.Memory.Length:X8}).");

            byte opcode = machine.Memory[machine.InstructionPointer];
            var decoder = _decoders[opcode];
            if (decoder == null)
                throw new InvalidInstructionException(opcode);

            if (decoder.StackElementsRequired != 0 &&
                (machine.Stack == null || machine.Stack.Count < decoder.StackElementsRequired))
            {
                throw new StackNotInitializedException();
            }
            if (decoder.FreeStackElementsRequired != 0 &&
                (machine.Stack == null || machine.Stack.Available < decoder.FreeStackElementsRequired))
            {
                throw new StackNotInitializedException();
            }
            if (decoder.InstructionDataSize != 0 &&
               (machine.Memory.Length - machine.InstructionPointer - Constants.OpCodeSize) < decoder.InstructionDataSize)
            {
                throw new InvalidInstructionException(opcode, "Not enough memory for opcode arguments.");
            }

            Span<byte> ip = default;
            if (decoder.InstructionDataSize != 0)
            {
                ip = machine.Memory.AsSpan(machine.InstructionPointer + Constants.OpCodeSize, decoder.InstructionDataSize);
            }
            decoder.Tick(opcode, ip, machine);
            machine.TickCount += 1L;
        }

        public IEnumerable<(Instruction? Instruction, long Offset)> DecodeAll(Stream stream)
        {
            long offset = stream.Position;
            int opcode = -1;

            while ((opcode = stream.ReadByte()) != -1)
            {
                var decoder = _decoders[opcode];
                if (decoder != null)
                {
                    if (decoder.InstructionDataSize == 0)
                    {
                        yield return (decoder.Decode((byte)opcode, default), offset);
                    }
                    else
                    {
                        var originalPosition = stream.Position;
                        var buffer = new byte[decoder.InstructionDataSize];
                        int read = stream.Read(buffer);
                        if (read == buffer.Length)
                        {
                            var instruction = decoder.Decode((byte)opcode, buffer);
                            yield return (instruction, offset);
                            if (instruction == null && stream.CanSeek)
                                stream.Seek(originalPosition, SeekOrigin.Begin);
                        }
                        else
                        {
                            yield return (null, offset);
                            if (stream.CanSeek)
                                stream.Seek(originalPosition, SeekOrigin.Begin);
                        }
                    }
                }
                else
                {
                    yield return (null, offset);
                }

                offset = stream.Position;
            }
        }

        public IEnumerable<(Instruction? Instruction, long Offset)> DecodeAll(ReadOnlyMemory<byte> span)
        {
            long offset = 0L;

            while (span.Length > 0)
            {
                var opcode = span.Span[0];
                var decoder = _decoders[opcode];

                if (decoder != null)
                {
                    if (decoder.InstructionDataSize == 0)
                    {
                        yield return (decoder.Decode(opcode, default), offset);
                    }
                    else if ((span.Length - 1) >= decoder.InstructionDataSize)
                    {
                        var instruction = decoder.Decode(opcode, span.Slice(1, decoder.InstructionDataSize));
                        yield return(instruction, offset);
                        if (instruction != null)
                        {
                            offset += decoder.InstructionDataSize;
                            span = span.Slice(decoder.InstructionDataSize);
                        }
                    }
                    else
                    {
                        yield return (null, offset);
                    }
                }
                else
                {
                    yield return (null, offset);
                }

                offset += 1L;
                span = span.Slice(1);
            }
        }
    }
}
