using System;

namespace StateMachinesBuilder
{
    public sealed class Nothing
    {
        private Nothing() { throw new InvalidOperationException("Can't create instance of nothing."); }
    }
}
