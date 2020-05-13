namespace DummyVirtualMachine.Instructions
{
    public class InnInstruction : SingleCodeInstruction
    {
        public static readonly InnInstruction Instance = new InnInstruction();

        private  InnInstruction() : base("INN", 0x04) {}
    }
}
