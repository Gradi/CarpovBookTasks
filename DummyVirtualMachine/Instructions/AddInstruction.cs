namespace DummyVirtualMachine.Instructions
{
    public class AddInstruction : SingleCodeInstruction
    {
        public static readonly AddInstruction Instance = new AddInstruction();

        private AddInstruction() : base("ADD", 0x08) {}
    }
}
