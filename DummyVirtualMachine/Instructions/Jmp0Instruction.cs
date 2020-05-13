namespace DummyVirtualMachine.Instructions
{
    public class Jmp0Instruction : InstructionWithPointer
    {
        public Jmp0Instruction(int pointer) : base(pointer, "JMP0", 0x02) {}
    }
}
