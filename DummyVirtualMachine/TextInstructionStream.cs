using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DummyVirtualMachine.Instructions;

namespace DummyVirtualMachine
{
    /// <summary>
    /// Stream writes text representation of data and instructions
    /// in form of pseudo assembly.
    /// </summary>
    public class TextInstructionStream : IInstructionStream
    {
        //Key: stream offset in bytes, Value: (data and it's size)
        private readonly IDictionary<long, (string Data, int Size)> _data;
        private readonly TextWriter _textWriter;
        private readonly bool _isDisposeWriter;

        public long Position { get; private set; }

        public TextInstructionStream(TextWriter textWriter, bool disposeWriter = true)
        {
            _data = new Dictionary<long, (string Data, int Size)>();
            _textWriter = textWriter ?? throw new ArgumentNullException(nameof(textWriter));
            _isDisposeWriter = disposeWriter;
        }

        public void Seek(long position, SeekOrigin origin = SeekOrigin.Begin)
        {
            long maxOffset = _data.Keys.Max();
            maxOffset += _data.TryGetValue(maxOffset, out var lastData) ? lastData.Size : 0;

            position = origin switch
            {
                SeekOrigin.Begin => position,
                SeekOrigin.Current => Position + position,
                SeekOrigin.End => maxOffset - position,
                var val => throw new ArgumentOutOfRangeException(nameof(origin), $"Unsupported argument value ({val}).")
            };

            // This weird check is just to make sure
            // that, in case we seek backwards, resulting position
            // exists in _data dictionary
            // e.g. if dictionary looks like
            // [0+4], [4+11], [15+p],....
            // we can seek to any position after 15+p or any of 0, 4 and 15, but not in between.
            // Later, Write() methods check if we write to already occupied position, but new size != old size we throw an exception.
            if (position != 0 && position < maxOffset && !_data.ContainsKey(position))
            {
                throw new ArgumentException($"Can't seek to {position} position from {Position}. You can only seek to already written positions. ");
            }

            Position = position;
        }

        public void Write(byte @byte)
        {
            CheckAndThrowOnUnalignedWrite(sizeof(byte));
            _data[Position] = ($"0x{@byte:X2}", sizeof(byte));
            Position += sizeof(byte);
        }

        public Task WriteAsync(byte @byte)
        {
            Write(@byte);
            return Task.CompletedTask;
        }

        public void Write(int word)
        {
            CheckAndThrowOnUnalignedWrite(sizeof(int));
            _data[Position] = ($"0x{word:X8}", sizeof(int));
            Position += sizeof(int);
        }

        public Task WriteAsync(int word)
        {
            Write(word);
            return Task.CompletedTask;
        }

        public void Write(Instruction instruction)
        {
            CheckAndThrowOnUnalignedWrite(instruction.ByteSize);
            _data[Position] = (instruction.ToString(), instruction.ByteSize);
            Position += instruction.ByteSize;
        }

        public Task WriteAsyn(Instruction instruction)
        {
            Write(instruction);
            return Task.CompletedTask;
        }

        public void Flush() {}

        public Task FlushAsync() => Task.CompletedTask;

        public void Dispose()
        {
            var keys = _data.Keys.ToArray();
            Array.Sort(keys);
            foreach (var offset in keys)
            {
                _textWriter.Write($"{offset:X8}: {_data[offset].Data}{Environment.NewLine}");
            }
            if (_isDisposeWriter)
            {
                _textWriter.Dispose();
            }
        }

        private void CheckAndThrowOnUnalignedWrite(int newSize)
        {
            if (_data.TryGetValue(Position, out var currentData) && currentData.Size != newSize)
            {
                throw new IOException("Can't write new data to already written postion. You can only rewrite some data with data with the same size as original. " +
                                      $"Current position {Position}, data size on that postion: {_data[Position].Size}, attempted data size: {newSize}.");
            }
        }
    }
}
