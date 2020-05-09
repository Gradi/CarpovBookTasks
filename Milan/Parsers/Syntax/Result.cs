using System;
using System.Collections.Generic;

namespace Milan.Parsers.Syntax
{
    public readonly struct Result<T>
    {
        private readonly T _value;

        public bool HasValue { get; }

        public string Error { get; }

        public T Value
        {
            get
            {
                if (!HasValue)
                    throw new InvalidOperationException($"Result<{typeof(T)}>: Access to \"{nameof(Value)}\" property when \"{nameof(HasValue)}\" equals to false.");
                return _value;
            }
        }

        private Result(bool hasValue, T value, string? error)
        {
            HasValue = hasValue;
            _value = value;
            Error = error ?? string.Empty;
        }

        public override bool Equals(object obj)
        {
            if (obj is Result<T> other)
            {
                if (!HasValue && !other.HasValue)
                    return true;
                if (HasValue || other.HasValue)
                    return false;
                return EqualityComparer<T>.Default.Equals(Value, other.Value);
            }
            if (obj is T otherT && HasValue)
            {
                return EqualityComparer<T>.Default.Equals(Value, otherT);
            }
            return false;
        }

        public override string ToString() => HasValue ? (Value?.ToString() ?? string.Empty) : string.Empty;

        public override int GetHashCode() => HasValue ? (Value?.GetHashCode() ?? 0) : 0;

        public static Result<T> Empty(string? error = null) => new Result<T>(false, default!, error);

        public static Result<T> FromOther<TOther>(Result<TOther> otherEmptyResult)
        {
            if (otherEmptyResult.HasValue)
                throw new InvalidOperationException($"Can't convert \"Result<{typeof(TOther)}>\" with value to \"Result<{typeof(T)}>\" without value.");
            return Empty(otherEmptyResult.Error);
        }

        public static Result<T> FromValue(T value) => new Result<T>(true, value, null!);

        public static implicit operator Result<T>(T value) => FromValue(value);
    }
}
