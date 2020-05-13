namespace DummyVirtualMachine.Instructions
{
    public class ModInstruction : SingleCodeInstruction
    {
        public static readonly ModInstruction Instance = new ModInstruction();

        private ModInstruction() : base("MOD", 0x0C) {}
    }
}
