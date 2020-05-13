namespace DummyVirtualMachine.Instructions
{
    public class Jmp1Instruction : InstructionWithPointer
    {
        public Jmp1Instruction(int pointer) : base(pointer, "JMP1", 0x03) {}
    }
}
