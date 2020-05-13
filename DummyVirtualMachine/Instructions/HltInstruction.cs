namespace DummyVirtualMachine.Instructions
{
    public class HltInstruction : SingleCodeInstruction
    {
        public static readonly HltInstruction Instance = new HltInstruction();

        private HltInstruction() : base("HLT", 0x00) {}
    }
}
