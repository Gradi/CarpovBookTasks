using System;
using System.Collections.Generic;

namespace DummyVirtualMachine
{
    public class Stack<T> where T : unmanaged
    {
        public Memory<byte> Memory { get; }

        public int Count { get; private set; }

        public unsafe int MaxElements => Memory.Length / sizeof(T);

        public int Available => MaxElements - Count;

        public Stack(Memory<byte> memory)
        {
            Memory = memory;
            Count = 0;
        }

        public unsafe void Push(T value)
        {
            if (Count == MaxElements)
            {
                throw new StackOverflowException("Can't push more elements. Stack has reached it's limit.");
            }

            int offset = Count * sizeof(T);
            fixed (void *target = Memory.Slice(offset, sizeof(T)).Span)
            {
                *( (T*)target ) = value;
            }
            Count += 1;
        }

        public unsafe T Pop()
        {
            if (Count == 0)
            {
                throw new InvalidOperationException("Can't pop element from stack. Stack is empty.");
            }

            Count -= 1;
            int offset = Count * sizeof(T);
            fixed(void *target = Memory.Slice(offset, sizeof(T)).Span)
            {
                return *( (T*)target );
            }
        }

        public bool TryPop(out T result)
        {
            if (Count != 0)
            {
                result = Pop();
                return true;
            }
            result = default;
            return false;
        }

        public IReadOnlyCollection<T> DumpValues()
        {
            var oldCount = Count;
            var results = new List<T>(Count);

            while(TryPop(out var item))
                results.Add(item);

            Count = oldCount;
            return results;
        }

        public void Clear()
        {
            Count = 0;
        }
    }
}
