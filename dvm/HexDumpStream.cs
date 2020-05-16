using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DummyVirtualMachine.Instructions;

namespace dvm
{
    public class HexDumpStream : IDisposable
    {
        private readonly TextWriter _writer;
        private readonly bool _isDisposeWriter;

        public long Position { get; private set; }

        public long LineSizeInBytes { get; private set; }

        private readonly StringBuilder _lineBuilder;
        private readonly ICollection<string> _currentLineComments;

        public HexDumpStream(TextWriter writer, bool diposeWriter = true)
        {
            _writer = writer ?? throw new ArgumentNullException(nameof(writer));
            _isDisposeWriter = diposeWriter;

            Position = 0;
            LineSizeInBytes = 0;
            _lineBuilder = new StringBuilder();
            _currentLineComments = new List<string>();
        }

        public void Write(byte @byte, string? comment = null)
        {
            AddSeparatorIfMust();
            _lineBuilder.Append(@byte.ToString("X2"));
            LineSizeInBytes += sizeof(byte);
            if (comment != null)
                _currentLineComments.Add(comment);
        }

        public void Write(Instruction instruction, string? comment = null)
        {
            AddSeparatorIfMust();
            _lineBuilder.Append(instruction);
            LineSizeInBytes += instruction.ByteSize;
            if (comment != null)
                _currentLineComments.Add(comment);
        }

        public void FlushLine()
        {
            if (_lineBuilder.Length != 0)
            {
                _writer.WriteLine($"{Position:X8}: {_lineBuilder} | {string.Join(", ", _currentLineComments.Distinct())}");
                Position += LineSizeInBytes;

                LineSizeInBytes = 0;
                _lineBuilder.Clear();
                _currentLineComments.Clear();
            }
        }

        public void Dispose()
        {
            FlushLine();
            if (_isDisposeWriter)
            {
                _writer.Dispose();
            }
        }

        private void AddSeparatorIfMust()
        {
            if (_lineBuilder.Length != 0)
            {
                _lineBuilder.Append(' ');
            }
        }




    }
}
