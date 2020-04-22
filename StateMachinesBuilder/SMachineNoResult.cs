using System;
using System.Collections.Generic;

namespace StateMachinesBuilder
{
    public abstract class SMachine<TIn> : SMachine<TIn, Nothing>
    {
        public new void Run(IEnumerable<TIn> inputs)
        {
            using var enumerator = base.Run(inputs).GetEnumerator();
            while(enumerator.MoveNext()) ;
        }

        protected sealed override void YieldResult(Nothing outResult) => throw new InvalidOperationException("State machine without return result can't yield results.");
    }
}
