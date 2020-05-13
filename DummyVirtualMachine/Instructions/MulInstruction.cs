namespace DummyVirtualMachine.Instructions
{
    public class MulInstruction : SingleCodeInstruction
    {
        public static readonly MulInstruction Instance = new MulInstruction();

        private MulInstruction() : base("MUL", 0x0A) {}
    }
}
