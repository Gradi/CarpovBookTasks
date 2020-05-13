namespace DummyVirtualMachine.Instructions
{
    public class LdInstruction : InstructionWithPointer
    {
        public LdInstruction(int pointer) : base(pointer, "LD", 0x06) {}
    }
}
