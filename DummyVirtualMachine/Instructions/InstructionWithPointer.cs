using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DummyVirtualMachine.Exceptions;

namespace DummyVirtualMachine.Instructions
{
    public abstract class InstructionWithPointer : Instruction
    {
        public int Pointer { get; }

        public override int ByteSize => Constants.OpCodeSize + Constants.PointerSize;

        protected InstructionWithPointer(int pointer, string mnemonic, byte code) : base(mnemonic, code)
        {
            if (pointer < 0)
                throw new InvalidPointerException(pointer);
            Pointer = pointer;
        }

        public override void Write(Stream stream)
        {
            using var writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true);
            writer.Write(Code);
            writer.Write(Pointer);
        }

        public override Task WriteAsync(Stream stream, CancellationToken cancellationToken = default)
        {
            Write(stream);
            return Task.CompletedTask;
        }

        public override string ToString() => $"{Mnemonic} {Pointer:X8}";

        protected override bool InnerEquals(Instruction other) => Pointer == ((InstructionWithPointer)other).Pointer;

        protected override int GetInnerHashCode() => Pointer.GetHashCode();
    }
}
