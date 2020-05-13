namespace DummyVirtualMachine.Instructions
{
    public class JmpInstruction : InstructionWithPointer
    {
        public JmpInstruction(int pointer) : base(pointer, "JMP", 0x01) {}
    }
}
