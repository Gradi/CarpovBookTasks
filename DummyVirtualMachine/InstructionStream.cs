using System;
using System.IO;
using System.Threading.Tasks;
using DummyVirtualMachine.Instructions;

namespace DummyVirtualMachine
{
    /// <summary>
    /// Stream writes binary data to some stream.
    /// </summary>
    public class InstructionStream : IInstructionStream
    {
        private readonly Stream _stream;
        private readonly bool _isDisposeStream;

        public long Position => _stream.Position;

        public InstructionStream(Stream stream, bool disposeStream = true)
        {
            _stream = stream ?? throw new ArgumentNullException(nameof(stream));
            if (!stream.CanSeek)
                throw new ArgumentOutOfRangeException(nameof(stream), "Stream must support seek operation.");
            _isDisposeStream = disposeStream;
        }

        public void Seek(long position, SeekOrigin origin = SeekOrigin.Begin) => _stream.Seek(position, origin);

        public void Write(byte @byte) => _stream.WriteByte(@byte);

        public Task WriteAsync(byte @byte)
        {
            var bytes = new byte[] { @byte };
            return _stream.WriteAsync(bytes).AsTask();
        }

        public void Write(int word) => _stream.Write(BitConverter.GetBytes(word));

        public Task WriteAsync(int word) => _stream.WriteAsync(BitConverter.GetBytes(word)).AsTask();

        public void Write(Instruction instruction) => instruction.Write(_stream);

        public Task WriteAsyn(Instruction instruction) => instruction.WriteAsync(_stream);

        public void Flush() => _stream.Flush();

        public Task FlushAsync() => _stream.FlushAsync();

        public void Dispose()
        {
            if (_isDisposeStream)
            {
                _stream.Dispose();
            }
        }
    }
}
