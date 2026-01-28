using System;

namespace _01.Scripts.Core.Utils
{
    /// <summary>
    /// 클리커 게임용 대용량 숫자 구조체.
    /// 과학적 표기법 기반: value = Mantissa × 10^Exponent
    /// </summary>
    [Serializable]
    public struct BigNumber : IComparable<BigNumber>, IEquatable<BigNumber>
    {
        public double Mantissa;
        public long Exponent;

        // 정규화 범위: 1.0 <= |Mantissa| < 10.0 (0 제외).
        private const double MinMantissa = 1.0;
        private const double MaxMantissa = 10.0;
        private const double Epsilon = 1e-10;

        public static readonly BigNumber Zero = new BigNumber(0, 0);
        public static readonly BigNumber One = new BigNumber(1, 0);

        public BigNumber(double mantissa, long exponent)
        {
            Mantissa = mantissa;
            Exponent = exponent;
            this = Normalize(this);
        }

        public BigNumber(double value)
        {
            if (value == 0)
            {
                Mantissa = 0;
                Exponent = 0;
                return;
            }

            Exponent = (long)Math.Floor(Math.Log10(Math.Abs(value)));
            Mantissa = value / Math.Pow(10, Exponent);
            this = Normalize(this);
        }

        public BigNumber(long value) : this((double)value) { }
        public BigNumber(int value) : this((double)value) { }

        #region Normalization

        private static BigNumber Normalize(BigNumber num)
        {
            if (Math.Abs(num.Mantissa) < Epsilon)
            {
                return Zero;
            }

            while (Math.Abs(num.Mantissa) >= MaxMantissa)
            {
                num.Mantissa /= 10.0;
                num.Exponent++;
            }

            while (Math.Abs(num.Mantissa) < MinMantissa && num.Mantissa != 0)
            {
                num.Mantissa *= 10.0;
                num.Exponent--;
            }

            return num;
        }

        #endregion

        #region Arithmetic Operators

        public static BigNumber operator +(BigNumber a, BigNumber b)
        {
            if (a.Mantissa == 0) return b;
            if (b.Mantissa == 0) return a;

            // 지수 차이가 크면 작은 쪽은 무시 (정밀도 한계).
            long expDiff = a.Exponent - b.Exponent;

            if (expDiff > 15) return a;
            if (expDiff < -15) return b;

            // 지수를 맞춰서 더하기.
            if (a.Exponent > b.Exponent)
            {
                double adjustedB = b.Mantissa * Math.Pow(10, b.Exponent - a.Exponent);
                return new BigNumber(a.Mantissa + adjustedB, a.Exponent);
            }
            else
            {
                double adjustedA = a.Mantissa * Math.Pow(10, a.Exponent - b.Exponent);
                return new BigNumber(adjustedA + b.Mantissa, b.Exponent);
            }
        }

        public static BigNumber operator -(BigNumber a, BigNumber b)
        {
            return a + new BigNumber(-b.Mantissa, b.Exponent);
        }

        public static BigNumber operator *(BigNumber a, BigNumber b)
        {
            return new BigNumber(a.Mantissa * b.Mantissa, a.Exponent + b.Exponent);
        }

        public static BigNumber operator /(BigNumber a, BigNumber b)
        {
            if (Math.Abs(b.Mantissa) < Epsilon)
            {
                throw new DivideByZeroException("BigNumber division by zero");
            }

            return new BigNumber(a.Mantissa / b.Mantissa, a.Exponent - b.Exponent);
        }

        public static BigNumber operator *(BigNumber a, double b)
        {
            return a * new BigNumber(b);
        }

        public static BigNumber operator *(double a, BigNumber b)
        {
            return new BigNumber(a) * b;
        }

        public static BigNumber operator /(BigNumber a, double b)
        {
            return a / new BigNumber(b);
        }

        #endregion

        #region Comparison Operators

        public int CompareTo(BigNumber other)
        {
            // 부호 비교.
            int signA = Math.Sign(Mantissa);
            int signB = Math.Sign(other.Mantissa);

            if (signA != signB)
            {
                return signA.CompareTo(signB);
            }

            // 같은 부호일 때 지수 비교.
            if (Exponent != other.Exponent)
            {
                int expComparison = Exponent.CompareTo(other.Exponent);
                return signA >= 0 ? expComparison : -expComparison;
            }

            // 지수가 같으면 가수 비교.
            return Mantissa.CompareTo(other.Mantissa);
        }

        public bool Equals(BigNumber other)
        {
            return Math.Abs(Mantissa - other.Mantissa) < Epsilon && Exponent == other.Exponent;
        }

        public override bool Equals(object obj)
        {
            return obj is BigNumber other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Mantissa, Exponent);
        }

        public static bool operator ==(BigNumber a, BigNumber b) => a.Equals(b);
        public static bool operator !=(BigNumber a, BigNumber b) => !a.Equals(b);
        public static bool operator <(BigNumber a, BigNumber b) => a.CompareTo(b) < 0;
        public static bool operator >(BigNumber a, BigNumber b) => a.CompareTo(b) > 0;
        public static bool operator <=(BigNumber a, BigNumber b) => a.CompareTo(b) <= 0;
        public static bool operator >=(BigNumber a, BigNumber b) => a.CompareTo(b) >= 0;

        #endregion

        #region Implicit Conversions

        public static implicit operator BigNumber(int value) => new BigNumber(value);
        public static implicit operator BigNumber(long value) => new BigNumber(value);
        public static implicit operator BigNumber(double value) => new BigNumber(value);

        #endregion

        #region Explicit Conversions

        public double ToDouble()
        {
            if (Exponent > 308) return double.PositiveInfinity;
            if (Exponent < -308) return 0;

            return Mantissa * Math.Pow(10, Exponent);
        }

        public long ToLong()
        {
            double value = ToDouble();
            if (value > long.MaxValue) return long.MaxValue;
            if (value < long.MinValue) return long.MinValue;
            return (long)value;
        }

        public int ToInt()
        {
            double value = ToDouble();
            if (value > int.MaxValue) return int.MaxValue;
            if (value < int.MinValue) return int.MinValue;
            return (int)value;
        }

        #endregion

        #region Utility Methods

        public bool IsZero => Math.Abs(Mantissa) < Epsilon;

        public static BigNumber Max(BigNumber a, BigNumber b)
        {
            return a >= b ? a : b;
        }

        public static BigNumber Min(BigNumber a, BigNumber b)
        {
            return a <= b ? a : b;
        }

        public static BigNumber Floor(BigNumber value)
        {
            if (value.Exponent < 0)
            {
                return Zero;
            }

            double fullValue = value.ToDouble();
            return new BigNumber(Math.Floor(fullValue));
        }

        public static BigNumber Ceiling(BigNumber value)
        {
            if (value.Exponent < 0)
            {
                return value.Mantissa > 0 ? One : Zero;
            }

            double fullValue = value.ToDouble();
            return new BigNumber(Math.Ceiling(fullValue));
        }

        #endregion

        #region String Formatting

        public override string ToString()
        {
            return NumberFormatter.Format(this);
        }

        public string ToExactString()
        {
            if (IsZero) return "0";
            return $"{Mantissa:F2}e{Exponent}";
        }

        #endregion
    }
}