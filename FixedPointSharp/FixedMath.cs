using System.Runtime.CompilerServices;

namespace Deterministic.FixedPoint {
    public partial struct FixedMath {
        private static readonly Fixed _atan2Number1 = new Fixed(-883);
        private static readonly Fixed _atan2Number2 = new Fixed(3767);
        private static readonly Fixed _atan2Number3 = new Fixed(7945);
        private static readonly Fixed _atan2Number4 = new Fixed(12821);
        private static readonly Fixed _atan2Number5 = new Fixed(21822);
        private static readonly Fixed _atan2Number6 = new Fixed(65536);
        private static readonly Fixed _atan2Number7 = new Fixed(102943);
        private static readonly Fixed _atan2Number8 = new Fixed(205887);
        private static readonly Fixed _atanApproximatedNumber1 = new Fixed(16036);
        private static readonly Fixed _atanApproximatedNumber2 = new Fixed(4345);
        private static readonly byte[] _bsrLookup = {0, 9, 1, 10, 13, 21, 2, 29, 11, 14, 16, 18, 22, 25, 3, 30, 8, 12, 20, 28, 15, 17, 24, 7, 19, 27, 23, 6, 26, 5, 4, 31};

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int BitScanReverse(uint num) {
            num |= num >> 1;
            num |= num >> 2;
            num |= num >> 4;
            num |= num >> 8;
            num |= num >> 16;
            return _bsrLookup[(num * 0x07C4ACDDU) >> 27];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CountLeadingZeroes(uint num) {
            return num == 0 ? 32 : BitScanReverse(num) ^ 31;
        }

        /// <param name="num">Angle in radians</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed Sin(Fixed num) {
            num.value %= Fixed.pi2.value;
            num       *= Fixed.one_div_pi2;
            var raw = FixedLut.sin(num.value);
            Fixed result;
            result.value = raw;
            return result;
        }

        /// <param name="num">Angle in radians</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed Cos(Fixed num) {
            num.value %= Fixed.pi2.value;
            num       *= Fixed.one_div_pi2;
            return new Fixed(FixedLut.cos(num.value));
        }

        /// <param name="num">Angle in radians</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed Tan(Fixed num) {
            num.value %= Fixed.pi2.value;
            num       *= Fixed.one_div_pi2;
            return new Fixed(FixedLut.tan(num.value));
        }

        /// <param name="num">Cos [-1, 1]</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed Acos(Fixed num) {
            return new Fixed(FixedLut.acos(num.value));
        }

        /// <param name="num">Sin [-1, 1]</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed Asin(Fixed num) {
            return new Fixed(FixedLut.asin(num.value));
        }

        /// <param name="num">Tan</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed Atan(Fixed num) {
            return Atan2(num, Fixed._1);
        }

        /// <param name="num">Tan [-1, 1]</param>
        /// Max error ~0.0015
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed AtanApproximated(Fixed num) {
            var absX = Abs(num);
            return Fixed.pi_quarter * num - num * (absX - Fixed._1) * (_atanApproximatedNumber1 + _atanApproximatedNumber2 * absX);
        }

        /// <param name="x">Denominator</param>
        /// <param name="y">Numerator</param>
        public static Fixed Atan2(Fixed y, Fixed x) {
            var absX = Abs(x);
            var absY = Abs(y);
            var t3   = absX;
            var t1   = absY;
            var t0   = Max(t3, t1);
            t1 = Min(t3, t1);
            t3 = Fixed._1 / t0;
            t3 = t1 * t3;
            var t4 = t3 * t3;
            t0 = _atan2Number1;
            t0 = t0 * t4 + _atan2Number2;
            t0 = t0 * t4 - _atan2Number3;
            t0 = t0 * t4 + _atan2Number4;
            t0 = t0 * t4 - _atan2Number5;
            t0 = t0 * t4 + _atan2Number6;
            t3 = t0 * t3;
            t3 = absY > absX ? _atan2Number7 - t3 : t3;
            t3 = x < Fixed._0 ? _atan2Number8 - t3 : t3;
            t3 = y < Fixed._0 ? -t3 : t3;
            return t3;
        }

        /// <param name="num">Angle in radians</param>
        public static void SinCos(Fixed num, out Fixed sin, out Fixed cos) {
            num.value %= Fixed.pi2.value;
            num       *= Fixed.one_div_pi2;
            FixedLut.sin_cos(num.value, out var sinVal, out var cosVal);
            sin.value = sinVal;
            cos.value = cosVal;
        }

        public static Fixed Rcp(Fixed num) {
            //(fp.1 << 16)
            return new Fixed(4294967296 / num.value);
        }
        
        public static Fixed Rsqrt(Fixed num) {
            //(fp.1 << 16)
            return new Fixed(4294967296 / Sqrt(num).value);
        }

        public static Fixed Sqrt(Fixed num) {
            Fixed r;

            if (num.value == 0) {
                r.value = 0;
            }
            else {
                var b = (num.value >> 1) + 1L;
                var c = (b + (num.value / b)) >> 1;

                while (c < b) {
                    b = c;
                    c = (b + (num.value / b)) >> 1;
                }

                r.value = b << (FixedLut.PRECISION >> 1);
            }

            return r;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed Floor(Fixed num) {
            num.value = num.value >> FixedLut.PRECISION << FixedLut.PRECISION;
            return num;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed Ceil(Fixed num) {
            var fractions = num.value & 0x000000000000FFFFL;

            if (fractions == 0) {
                return num;
            }

            num.value = num.value >> FixedLut.PRECISION << FixedLut.PRECISION;
            num.value += FixedLut.ONE;
            return num;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed Fractions(Fixed num) {
            return new Fixed(num.value & 0x000000000000FFFFL);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int RoundToInt(Fixed num) {
            var fraction = num.value & 0x000000000000FFFFL;

            if (fraction >= FixedLut.HALF) {
                return num.AsInt + 1;
            }

            return num.AsInt;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed Min(Fixed a, Fixed b) {
            return a.value < b.value ? a : b;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Min(int a, int b) {
            return a < b ? a : b;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Min(long a, long b) {
            return a < b ? a : b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed Max(Fixed a, Fixed b) {
            return a.value > b.value ? a : b;
        }
                
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Max(int a, int b) {
            return a > b ? a : b;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Max(long a, long b) {
            return a > b ? a : b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed Abs(Fixed num) {
            return new Fixed(num.value < 0 ? -num.value : num.value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed Clamp(Fixed num, Fixed min, Fixed max) {
            if (num.value < min.value) {
                return min;
            }

            if (num.value > max.value) {
                return max;
            }

            return num;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Clamp(int num, int min, int max) {
            return num < min ? min : num > max ? max : num;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Clamp(long num, long min, long max) {
            return num < min ? min : num > max ? max : num;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed Clamp01(Fixed num) {
            if (num.value < 0) {
                return Fixed._0;
            }

            return num.value > Fixed._1.value ? Fixed._1 : num;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed Lerp(Fixed from, Fixed to, Fixed t) {
            t = Clamp01(t);
            return from + (to - from) * t;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FixedBool Lerp(FixedBool from, FixedBool to, Fixed t) {
            return t.value > Fixed._0_50.value ? to : from;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed Repeat(Fixed value, Fixed length) {
            return Clamp(value - Floor(value / length) * length, 0, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed LerpAngle(Fixed from, Fixed to, Fixed t) {
            var num = Repeat(to - from, Fixed.pi2);
            return Lerp(from, from + (num > Fixed.pi ? num - Fixed.pi2 : num), t);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed NormalizeRadians(Fixed angle) {
            angle.value %= FixedLut.PI;
            return angle;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed LerpUnclamped(Fixed from, Fixed to, Fixed t) {
            return from + (to - from) * t;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed Sign(Fixed num) {
            return num.value < FixedLut.ZERO ? Fixed.minus_one : Fixed._1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsOppositeSign(Fixed a, Fixed b) {
            return ((a.value ^ b.value) < 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed SetSameSign(Fixed target, Fixed reference) {
            return IsOppositeSign(target, reference) ? target * Fixed.minus_one : target;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed Pow2(int power) {
            return new Fixed(FixedLut.ONE << power);
        }

        public static Fixed Exp(Fixed num) {
            if (num == Fixed._0) return Fixed._1;
            if (num == Fixed._1) return Fixed.e;
            if (num.value >= 2097152) return Fixed.max;
            if (num.value <= -786432) return Fixed._0;

            var neg      = num.value < 0;
            if (neg) num = -num;

            var result = num + Fixed._1;
            var term   = num;

            for (var i = 2; i < 30; i++) {
                term   *= num / (Fixed)i;
                result += term;

                if (term.value < 500 && ((i > 15) || (term.value < 20)))
                    break;
            }

            if (neg) result = Fixed._1 / result;

            return result;
        }
    }
}