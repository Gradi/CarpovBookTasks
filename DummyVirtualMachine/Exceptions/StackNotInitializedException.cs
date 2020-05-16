using System;
using IF = DummyVirtualMachine.Instructions.InstructionFactory;

namespace DummyVirtualMachine.Exceptions
{
    public class StackNotInitializedException : Exception
    {
        public StackNotInitializedException() : base($"Stack is not initialized. Ensure that generated code contains '{IF.Stk(0, 0).Mnemonic}' code. " +
                                                     "Or it's size big enough.") {}
    }
}
