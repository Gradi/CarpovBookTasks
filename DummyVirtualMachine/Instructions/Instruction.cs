using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DummyVirtualMachine.Instructions
{
    public abstract class Instruction : IEquatable<Instruction>
    {
        public string Mnemonic { get; }

        public byte Code { get; }

        public abstract int ByteSize { get; }

        protected Instruction(string mnemonic, byte code)
        {
            if (string.IsNullOrWhiteSpace(mnemonic))
                throw new ArgumentNullException(nameof(mnemonic));

            Mnemonic = mnemonic;
            Code = code;
        }

        public abstract void Write(Stream stream);

        public abstract Task WriteAsync(Stream stream, CancellationToken cancellationToken = default);

        public virtual Instruction Clone() => (Instruction)MemberwiseClone();

        public bool Equals(Instruction other)
        {
            if (ReferenceEquals(other, null))
                return false;
            if (ReferenceEquals(this, other))
                return true;

            return Mnemonic == other.Mnemonic &&
                   Code == other.Code &&
                   GetType() == other.GetType() &&
                   InnerEquals(other);
        }

        public override bool Equals(object obj) => obj is Instruction other && Equals(other);

        public override string ToString() => Mnemonic;

        public override int GetHashCode()
        {
            var code = new HashCode();
            code.Add(Mnemonic);
            code.Add(Code);
            code.Add(GetInnerHashCode());
            return code.ToHashCode();
        }

        protected virtual bool InnerEquals(Instruction other) => true;

        protected virtual int GetInnerHashCode() => 0;

        public static bool operator==(Instruction? left, Instruction? right)
        {
            if (ReferenceEquals(left, null) && ReferenceEquals(right, null))
                return true;
            if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
                return false;
            return left.Equals(right);
        }

        public static bool operator!=(Instruction? left, Instruction? right) => !(left == right);
    }
}
