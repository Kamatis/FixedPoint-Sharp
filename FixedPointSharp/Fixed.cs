using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Deterministic.FixedPoint {
    [Serializable]
    [StructLayout(LayoutKind.Explicit, Size = SIZE)]
    public struct Fixed : IEquatable<Fixed>, IComparable<Fixed> {
        public const int SIZE = 8;

        public static readonly Fixed max        = new Fixed(long.MaxValue);
        public static readonly Fixed min        = new Fixed(long.MinValue);
        public static readonly Fixed usable_max = new Fixed(2147483648L);
        public static readonly Fixed usable_min = -usable_max;

        public static readonly Fixed _0   = 0;
        public static readonly Fixed _1   = 1;
        public static readonly Fixed _2   = 2;
        public static readonly Fixed _3   = 3;
        public static readonly Fixed _4   = 4;
        public static readonly Fixed _5   = 5;
        public static readonly Fixed _6   = 6;
        public static readonly Fixed _7   = 7;
        public static readonly Fixed _8   = 8;
        public static readonly Fixed _9   = 9;
        public static readonly Fixed _10  = 10;
        public static readonly Fixed _99  = 99;
        public static readonly Fixed _100 = 100;
        public static readonly Fixed _200 = 200;

        public static readonly Fixed _0_01 = _1 / _100;
        public static readonly Fixed _0_02 = _0_01 * 2;
        public static readonly Fixed _0_03 = _0_01 * 3;
        public static readonly Fixed _0_04 = _0_01 * 4;
        public static readonly Fixed _0_05 = _0_01 * 5;
        public static readonly Fixed _0_10 = _1 / 10;
        public static readonly Fixed _0_20 = _0_10 * 2;
        public static readonly Fixed _0_25 = _1 / 4;
        public static readonly Fixed _0_33 = _1 / 3;
        public static readonly Fixed _0_50 = _1 / 2;
        public static readonly Fixed _0_75 = _1 - _0_25;
        public static readonly Fixed _0_95 = _1 - _0_05;
        public static readonly Fixed _0_99 = _1 - _0_01;
        public static readonly Fixed _1_01 = _1 + _0_01;
        public static readonly Fixed _1_10 = _1 + _0_10;
        public static readonly Fixed _1_50 = _1 + _0_50;

        public static readonly Fixed minus_one   = -1;
        public static readonly Fixed pi          = new Fixed(205887L);
        public static readonly Fixed pi2         = pi * 2;
        public static readonly Fixed pi_quarter  = pi * _0_25;
        public static readonly Fixed pi_half     = pi * _0_50;
        public static readonly Fixed one_div_pi2 = 1 / pi2;
        public static readonly Fixed deg2rad     = new Fixed(1143L);
        public static readonly Fixed rad2deg     = new Fixed(3754936L);
        public static readonly Fixed epsilon     = new Fixed(1);
        public static readonly Fixed e           = new Fixed(178145L);

        [FieldOffset(0)]
        public long value;

        public long   AsLong          => value >> FixedLut.PRECISION;
        public int    AsInt           => (int) (value >> FixedLut.PRECISION);
        public float  AsFloat         => value / 65536f;
        public float  AsFloatRounded  => (float) Math.Round(value / 65536f, 5);
        public double AsDouble        => value / 65536d;
        public double AsDoubleRounded => Math.Round(value / 65536d, 5);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal Fixed(long v) {
            value = v;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed operator -(Fixed a) {
            a.value = -a.value;
            return a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed operator +(Fixed a) {
            a.value = +a.value;
            return a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed operator +(Fixed a, Fixed b) {
            a.value += b.value;
            return a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed operator +(Fixed a, int b) {
            a.value += (long) b << FixedLut.PRECISION;
            return a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed operator +(int a, Fixed b) {
            b.value = ((long) a << FixedLut.PRECISION) + b.value;
            return b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed operator -(Fixed a, Fixed b) {
            a.value -= b.value;
            return a;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed operator -(Fixed a, int b) {
            a.value -= (long) b << FixedLut.PRECISION;
            return a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed operator -(int a, Fixed b) {
            b.value = ((long) a << FixedLut.PRECISION) - b.value;
            return b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed operator *(Fixed a, Fixed b) {
            a.value = a.value * b.value >> FixedLut.PRECISION;
            return a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed operator *(Fixed a, int b) {
            a.value *= b;
            return a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed operator *(int a, Fixed b) {
            b.value *= a;
            return b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed operator /(Fixed a, Fixed b) {
            a.value = (a.value << FixedLut.PRECISION) / b.value;
            return a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed operator /(Fixed a, int b) {
            a.value /= b;
            return a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed operator /(int a, Fixed b) {
            b.value = ((long) a << 32) / b.value;
            return b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed operator %(Fixed a, Fixed b) {
            a.value %= b.value;
            return a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed operator %(Fixed a, int b) {
            a.value %= (long) b << FixedLut.PRECISION;
            return a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed operator %(int a, Fixed b) {
            b.value = ((long) a << FixedLut.PRECISION) % b.value;
            return b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(Fixed a, Fixed b) {
            return a.value < b.value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(Fixed a, int b) {
            return a.value < (long) b << FixedLut.PRECISION;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(int a, Fixed b) {
            return (long) a << FixedLut.PRECISION < b.value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(Fixed a, Fixed b) {
            return a.value <= b.value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(Fixed a, int b) {
            return a.value <= (long) b << FixedLut.PRECISION;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(int a, Fixed b) {
            return (long) a << FixedLut.PRECISION <= b.value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(Fixed a, Fixed b) {
            return a.value > b.value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(Fixed a, int b) {
            return a.value > (long) b << FixedLut.PRECISION;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(int a, Fixed b) {
            return (long) a << FixedLut.PRECISION > b.value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(Fixed a, Fixed b) {
            return a.value >= b.value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(Fixed a, int b) {
            return a.value >= (long) b << FixedLut.PRECISION;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(int a, Fixed b) {
            return (long) a << FixedLut.PRECISION >= b.value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Fixed a, Fixed b) {
            return a.value == b.value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Fixed a, int b) {
            return a.value == (long) b << FixedLut.PRECISION;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(int a, Fixed b) {
            return (long) a << FixedLut.PRECISION == b.value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Fixed a, Fixed b) {
            return a.value != b.value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Fixed a, int b) {
            return a.value != (long) b << FixedLut.PRECISION;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(int a, Fixed b) {
            return (long) a << FixedLut.PRECISION != b.value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Fixed(int value) {
            Fixed f;
            f.value = (long) value << FixedLut.PRECISION;
            return f;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator int(Fixed value) {
            return (int) (value.value >> FixedLut.PRECISION);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator long(Fixed value) {
            return value.value >> FixedLut.PRECISION;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator float(Fixed value) {
            return value.value / 65536f;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator double(Fixed value) {
            return value.value / 65536d;
        }

        public int CompareTo(Fixed other) {
            return value.CompareTo(other.value);
        }

        public bool Equals(Fixed other) {
            return value == other.value;
        }

        public override bool Equals(object obj) {
            return obj is Fixed other && this == other;
        }

        public override int GetHashCode() {
            return value.GetHashCode();
        }

        public override string ToString() {
            var corrected = Math.Round(AsDouble, 5);
            return corrected.ToString("F5", CultureInfo.InvariantCulture);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed ParseRaw(long value) {
            return new Fixed(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed Parse(long value) {
            return new Fixed(value << FixedLut.PRECISION);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed ParseUnsafe(float value) {
            return new Fixed((long) (value * FixedLut.ONE + 0.5f * (value < 0 ? -1 : 1)));
        }

        public static Fixed ParseUnsafe(string value) {
            var doubleValue = double.Parse(value, CultureInfo.InvariantCulture);
            var longValue   = (long) (doubleValue * FixedLut.ONE + 0.5d * (doubleValue < 0 ? -1 : 1));
            return new Fixed(longValue);
        }

        /// <summary>
        /// Deterministically parses FP value out of a string
        /// </summary>
        /// <param name="value">Trimmed string to parse</param>
        public static Fixed Parse(string value) {
            if (string.IsNullOrEmpty(value))
            {
                return _0;
            }
            
            bool negative;

            var startIndex = 0;
            if (negative = (value[0] == '-')) {
                startIndex = 1;
            }

            var pointIndex = value.IndexOf('.');
            if (pointIndex < startIndex) {
                if (startIndex == 0) {
                    return ParseInteger(value);
                }

                return -ParseInteger(value.Substring(startIndex, value.Length - startIndex));

            }

            var result = new Fixed();
            
            if (pointIndex > startIndex) {
                var integerString = value.Substring(startIndex, pointIndex - startIndex);
                result += ParseInteger(integerString);
            }


            if (pointIndex == value.Length - 1) {
                return negative ? -result : result;
            }

            var fractionString = value.Substring(pointIndex + 1, value.Length - pointIndex - 1);
            if (fractionString.Length > 0) {
                result += ParseFractions(fractionString);
            }

            return negative ? -result : result;
        }
        
        private static Fixed ParseInteger(string format) {
            return Parse(long.Parse(format, CultureInfo.InvariantCulture));
        }

        private static Fixed ParseFractions(string format) {
            format = format.Length < 5 ? format.PadRight(5, '0') : format.Substring(0, 5);
            return ParseRaw(long.Parse(format, CultureInfo.InvariantCulture) * 65536 / 100000);
        }

        public class Comparer : IComparer<Fixed> {
            public static readonly Comparer instance = new Comparer();

            private Comparer() { }

            int IComparer<Fixed>.Compare(Fixed x, Fixed y) {
                return x.value.CompareTo(y.value);
            }
        }

        public class EqualityComparer : IEqualityComparer<Fixed> {
            public static readonly EqualityComparer instance = new EqualityComparer();

            private EqualityComparer() { }

            bool IEqualityComparer<Fixed>.Equals(Fixed x, Fixed y) {
                return x.value == y.value;
            }

            int IEqualityComparer<Fixed>.GetHashCode(Fixed num) {
                return num.value.GetHashCode();
            }
        }
    }
}