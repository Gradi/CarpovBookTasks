using System;

namespace DummyVirtualMachine.Exceptions
{
    public class InvalidInstructionException : Exception
    {
        public byte Opcode { get; }

        public InvalidInstructionException(byte opcode) : base($"Invalid instruction opcode ({opcode}).")
        {
            Opcode = opcode;
        }

        public InvalidInstructionException(byte opcode, string message) : base(message)
        {
            Opcode = opcode;
        }
    }
}
