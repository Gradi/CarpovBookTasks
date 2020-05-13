using System;

namespace DummyVirtualMachine
{
    public class Machine
    {
        public byte[] Memory { get; }

        public Stack<int>? Stack { get; set; }

        public int InstructionPointer { get; set; }

        public bool IsHalted { get; set; }

        public long TickCount { get; set; }

        public IOSystem IOSystem { get; }

        public Machine(byte[] memory, IOSystem ioSystem)
        {
            Memory = memory ?? throw new ArgumentNullException(nameof(memory));
            Stack = null;
            InstructionPointer = 0;
            IsHalted = false;
            TickCount = 0L;
            IOSystem = ioSystem ?? throw new ArgumentNullException(nameof(ioSystem));
        }

        public Machine(int memorySize, IOSystem ioSystem) : this(new byte[memorySize], ioSystem) {}
    }
}
