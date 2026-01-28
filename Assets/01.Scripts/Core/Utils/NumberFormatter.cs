namespace _01.Scripts.Core.Utils
{
    /// <summary>
    /// K/M/B/T 및 알파벳 순환(aa, ab...) 단위로 숫자를 축약 표시합니다.
    /// </summary>
    public static class NumberFormatter
    {
        private static readonly string[] BaseUnits = { "", "K", "M", "B", "T" };

        public static string Format(BigNumber value)
        {
            if (value.IsZero)
            {
                return "0";
            }

            // 지수 기반으로 단위 계산 (3자리마다 단위 변경).
            long unitIndex = value.Exponent / 3;
            long remainder = value.Exponent % 3;

            // 표시할 숫자 계산.
            double displayValue = value.Mantissa * System.Math.Pow(10, remainder);

            if (unitIndex < 0)
            {
                return displayValue.ToString("F0");
            }

            if (unitIndex < BaseUnits.Length)
            {
                if (unitIndex == 0)
                {
                    return displayValue.ToString("F0");
                }

                return $"{displayValue:F1}{BaseUnits[unitIndex]}";
            }

            // T 이후: 알파벳 순환.
            int alphabetIndex = (int)(unitIndex - BaseUnits.Length + 1);
            string suffix = GetAlphabetSuffix(alphabetIndex);
            return $"{displayValue:F1}{suffix}";
        }

        public static string Format(double value)
        {
            if (value < 1000)
            {
                return value.ToString("F0");
            }

            int unitIndex = 0;
            while (value >= 1000 && unitIndex < BaseUnits.Length - 1)
            {
                value /= 1000;
                unitIndex++;
            }

            // T 이후: 알파벳 순환 (aa, ab, ac...).
            if (unitIndex >= BaseUnits.Length - 1 && value >= 1000)
            {
                int alphabetIndex = 0;
                while (value >= 1000)
                {
                    value /= 1000;
                    alphabetIndex++;
                }

                string suffix = GetAlphabetSuffix(alphabetIndex);
                return $"{value:F1}{suffix}";
            }

            return $"{value:F1}{BaseUnits[unitIndex]}";
        }

        public static string Format(float value)
        {
            return Format((double)value);
        }

        public static string Format(int value)
        {
            return Format((double)value);
        }

        public static string Format(long value)
        {
            return Format((double)value);
        }

        private static string GetAlphabetSuffix(int index)
        {
            // 0 -> aa, 1 -> ab, ... 25 -> az, 26 -> ba, 27 -> bb...
            char first = (char)('a' + (index / 26));
            char second = (char)('a' + (index % 26));
            return $"{first}{second}";
        }
    }
}