namespace DummyVirtualMachine.Extensions
{
    public static class MachineExtensions
    {
        public static bool IsValidPointer(this Machine machine, int pointer)
        {
            return pointer >= 0 && pointer < machine.Memory.Length;
        }
    }
}
