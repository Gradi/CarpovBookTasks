using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DummyVirtualMachine.Instructions
{
    public abstract class SingleCodeInstruction : Instruction
    {
        public override int ByteSize => Constants.OpCodeSize;

        protected SingleCodeInstruction(string mnemonic, byte code) : base(mnemonic, code) {}

        public override void Write(Stream stream) => stream.WriteByte(Code);

        public override Task WriteAsync(Stream stream, CancellationToken cancellationToken = default)
        {
            var bytes = new byte[] { Code };
            return stream.WriteAsync(bytes, 0, bytes.Length, cancellationToken);
        }
    }
}
