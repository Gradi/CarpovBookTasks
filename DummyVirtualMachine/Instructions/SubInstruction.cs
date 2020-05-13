namespace DummyVirtualMachine.Instructions
{
    public class SubInstruction : SingleCodeInstruction
    {
        public static readonly SubInstruction Instance = new SubInstruction();

        private SubInstruction() : base("SUB", 0x09) {}
    }
}
