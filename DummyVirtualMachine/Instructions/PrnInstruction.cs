namespace DummyVirtualMachine.Instructions
{
    public class PrnInstruction : SingleCodeInstruction
    {
        public static readonly PrnInstruction Instance = new PrnInstruction();

        private PrnInstruction() : base("PRN", 0x05) {}
    }
}
