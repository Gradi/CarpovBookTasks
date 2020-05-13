using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DummyVirtualMachine.Instructions
{
    public class CmpInstruction : Instruction
    {
        public CmpType Type { get; }

        public override int ByteSize => Constants.OpCodeSize + sizeof(CmpType);

        public CmpInstruction(CmpType type) : base("CMP", 0x0D)
        {
            Type = type;
        }

        public override void Write(Stream stream)
        {
            using var writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true);
            writer.Write(Code);
            writer.Write((byte)Type);
        }

        public override Task WriteAsync(Stream stream, CancellationToken cancellationToken = default)
        {
            Write(stream);
            return Task.CompletedTask;
        }

        public override string ToString() => $"{Mnemonic} {(byte)Type}";

        protected override bool InnerEquals(Instruction other) => Type == ((CmpInstruction)other).Type;

        protected override int GetInnerHashCode() => Type.GetHashCode();

        public enum CmpType : byte
        {
            Equal = 0,
            NotEqual = 1,
            LessThan = 2,
            GreaterThan = 3,
            LessOrEqualThan = 4,
            GreaterOrEqualThan = 5
        }
    }
}
