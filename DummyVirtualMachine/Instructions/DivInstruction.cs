namespace DummyVirtualMachine.Instructions
{
    public class DivInstruction : SingleCodeInstruction
    {
        public static readonly DivInstruction Instance = new DivInstruction();

        private DivInstruction() : base("DIV", 0x0B) {}
    }
}
