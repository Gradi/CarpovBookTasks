using System;

namespace StateMachinesBuilder
{
    public readonly struct Option<T>
    {
        private readonly T _value;

        public readonly bool HasValue;

        public T Value
        {
            get
            {
                if (!HasValue)
                    throw new InvalidOperationException($"Option<{typeof(T)}>: Access to \"{nameof(Value)}\" property when \"{nameof(HasValue)}\" property equals to false.");
                return _value;
            }
        }

        private Option(bool hasValue, T value)
        {
            HasValue = hasValue;
            _value = value;
        }

        public static Option<T> WithValue(T value) => new Option<T>(true, value);

        public static Option<T> Empty() => new Option<T>(false, default);

        public static implicit operator Option<T>(T value) => WithValue(value);
    }
}
