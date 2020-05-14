using System;
using System.IO;
using System.Threading.Tasks;
using DummyVirtualMachine.Instructions;

namespace DummyVirtualMachine
{
    public interface IInstructionStream : IDisposable
    {
        long Position { get; }

        void Seek(long position, SeekOrigin origin);

        void Write(byte @byte);

        Task WriteAsync(byte @byte);

        void Write(int word);

        Task WriteAsync(int word);

        void Write(Instruction instruction);

        Task WriteAsyn(Instruction instruction);

        void Flush();

        Task FlushAsync();
    }
}
