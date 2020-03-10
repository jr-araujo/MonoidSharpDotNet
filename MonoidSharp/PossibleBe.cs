using System;

namespace MonoidSharp
{
    public struct PossibleBe<T> : IEquatable<PossibleBe<T>>
    {
        private readonly InternalPossibleBe _internalPossibleValue;

        private PossibleBe(T value)
        {
            _internalPossibleValue = value == null ? null : new InternalPossibleBe(value);
        }

        public static PossibleBe<T> None => new PossibleBe<T>();

        public bool HasNoValue => !HasValue;

        public bool HasValue => _internalPossibleValue != null;

        public T Value
        {
            get
            {
                if (HasValue)
                    return _internalPossibleValue.Value;

                throw new ArgumentNullException("It doesn't have value to use !");
            }
        }

        public static implicit operator PossibleBe<T>(T value)
        {
            return new PossibleBe<T>(value);
        }

        public static bool operator !=(PossibleBe<T> possibleBe, T value)
        {
            return !(possibleBe == value);
        }

        public static bool operator !=(PossibleBe<T> firstPossibleBe, PossibleBe<T> secondPossibleBe)
        {
            return !(firstPossibleBe == secondPossibleBe);
        }

        public static bool operator ==(PossibleBe<T> firstPossibleBe, PossibleBe<T> secondPossibleBe)
        {
            return firstPossibleBe.Equals(secondPossibleBe);
        }

        public static bool operator ==(PossibleBe<T> possibleBe, T value)
        {
            if (value is PossibleBe<T>)
                return possibleBe.Equals(value);

            if (possibleBe.HasNoValue)
                return false;

            return possibleBe.Value.Equals(value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;

            if (ReferenceEquals(this, (PossibleBe<T>)obj))
                return true;

            if (((PossibleBe<T>)obj).GetType() != GetType())
                return false;

            return Equals((PossibleBe<T>)obj);
        }

        public bool Equals(PossibleBe<T> other)
        {
            if (HasNoValue && other.HasNoValue)
                return true;

            if (HasNoValue || other.HasNoValue)
                return true;

            return _internalPossibleValue.Value.Equals(other._internalPossibleValue.Value);
        }

        public override int GetHashCode()
        {
            if (HasNoValue)
                return -1;

            return _internalPossibleValue.Value.GetHashCode();
        }

        public override string ToString()
        {
            if (HasNoValue)
                return "Without value";

            return Value.ToString();
        }

        private class InternalPossibleBe
        {
            internal readonly T Value;

            public InternalPossibleBe(T internalValue)
            {
                Value = internalValue;
            }
        }
    }
}