using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DummyVirtualMachine.Instructions
{
    public class StkInstruction : InstructionWithPointer
    {
        public int WordCount { get; }

        public override int ByteSize => Constants.OpCodeSize + Constants.PointerSize + Constants.WordSize;

        public StkInstruction(int pointer, int wordCount) : base(pointer, "STK", 0x0E)
        {
            if (wordCount < 0)
                throw new ArgumentOutOfRangeException(nameof(wordCount), $"Amount of words can't be negative ({wordCount}).");
            WordCount = wordCount;
        }

        public override void Write(Stream stream)
        {
            using var writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true);
            writer.Write(Code);
            writer.Write(Pointer);
            writer.Write(WordCount);
        }

        public override Task WriteAsync(Stream stream, CancellationToken cancellationToken = default)
        {
            Write(stream);
            return Task.CompletedTask;
        }

        public override string ToString() => $"{Mnemonic} {Pointer:X8}, {WordCount:X8}";

        protected override bool InnerEquals(Instruction other)
        {
            var otherStk = (StkInstruction)other;
            return base.InnerEquals(other) &&
                   WordCount == otherStk.WordCount;
        }

        protected override int GetInnerHashCode()
        {
            var code = new HashCode();
            code.Add(base.GetInnerHashCode());
            code.Add(WordCount.GetHashCode());
            return code.ToHashCode();
        }
    }
}
