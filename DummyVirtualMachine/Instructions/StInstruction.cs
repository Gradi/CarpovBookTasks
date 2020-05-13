namespace DummyVirtualMachine.Instructions
{
    public class StInstruction : InstructionWithPointer
    {
        public StInstruction(int pointer) : base(pointer, "ST", 0x07) {}
    }
}
