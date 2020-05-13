using System;

namespace DummyVirtualMachine.Exceptions
{
    public class InvalidPointerException : Exception
    {
        public int Pointer { get; }

        public InvalidPointerException(int pointer) : base($"Pointer has invalid value ({pointer:X8}).")
        {
            Pointer = pointer;
        }

        public InvalidPointerException(int pointer, Machine machine)
            : base($"Pointer has invalid value ({pointer:X8}, machine memory: {machine.Memory.Length:X8} bytes).")
        {
            Pointer = pointer;
        }

        public InvalidPointerException(int pointer, string message) : base(message)
        {
            Pointer = pointer;
        }

        public static void ThrowNotEnoughSizeForWord(int pointer) =>
            throw new InvalidPointerException(pointer, "Pointer points to memory with not enough size for word.");
    }
}
