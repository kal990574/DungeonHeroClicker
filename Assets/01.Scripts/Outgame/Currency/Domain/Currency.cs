using System;
using _01.Scripts.Core.Utils;

namespace _01.Scripts.Outgame.Currency.Domain
{
    public readonly struct Currency : IEquatable<Currency>, IComparable<Currency>
    {
        public readonly BigNumber Value;

        public static readonly Currency Zero = new Currency(BigNumber.Zero);

        public Currency(BigNumber value)
        {
            Value = value > BigNumber.Zero ? value : BigNumber.Zero;
        }

        public static Currency operator +(Currency a, Currency b)
        {
            return new Currency(a.Value + b.Value);
        }

        public static Currency operator -(Currency a, Currency b)
        {
            return new Currency(a.Value - b.Value);
        }

        public static bool operator ==(Currency a, Currency b) => a.Value == b.Value;
        public static bool operator !=(Currency a, Currency b) => a.Value != b.Value;
        public static bool operator <(Currency a, Currency b) => a.Value < b.Value;
        public static bool operator >(Currency a, Currency b) => a.Value > b.Value;
        public static bool operator <=(Currency a, Currency b) => a.Value <= b.Value;
        public static bool operator >=(Currency a, Currency b) => a.Value >= b.Value;

        public static implicit operator Currency(BigNumber value) => new Currency(value);
        public static implicit operator BigNumber(Currency currency) => currency.Value;

        public bool Equals(Currency other) => Value == other.Value;
        public int CompareTo(Currency other) => Value.CompareTo(other.Value);

        public override bool Equals(object obj) => obj is Currency other && Equals(other);
        public override int GetHashCode() => Value.GetHashCode();
        public override string ToString() => Value.ToString();
    }
}