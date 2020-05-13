using System;

namespace DummyVirtualMachine.Exceptions
{
    public class InvalidMachineStateException : Exception
    {
        public InvalidMachineStateException(string message) : base(message) {}
    }
}
