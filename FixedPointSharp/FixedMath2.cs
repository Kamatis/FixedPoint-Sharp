using System.Runtime.CompilerServices;

namespace Deterministic.FixedPoint
{
    public partial struct FixedMath
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed Sum(Fixed2 v) {
            return new Fixed(v.x.value + v.y.value);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed2 Min(Fixed2 a, Fixed2 b)
        {
            var ret  = a.x.value < b.x.value ? a.x : b.x;
            var ret1 = a.y.value < b.y.value ? a.y : b.y;

            return new Fixed2(ret, ret1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed2 Max(Fixed2 a, Fixed2 b)
        {
            var ret  = a.x.value > b.x.value ? a.x : b.x;
            var ret1 = a.y.value > b.y.value ? a.y : b.y;
            return new Fixed2(ret, ret1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed Dot(Fixed2 a, Fixed2 b)
        {
            a.x.value = ((a.x.value * b.x.value) >> FixedLut.PRECISION) + ((a.y.value * b.y.value) >> FixedLut.PRECISION);
            return a.x;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed Cross(Fixed2 a, Fixed2 b) {
            a.x.value = (a.x.value * b.y.value >> FixedLut.PRECISION) - (a.y.value * b.x.value >> FixedLut.PRECISION);
            return a.x;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed2 Cross(Fixed2 a, Fixed s)
        {
            return new Fixed2(s * a.y, -s * a.x);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed2 Cross(Fixed s, Fixed2 a) {
            Fixed2 result;
            result.x.value = -s.value * a.y.value >> FixedLut.PRECISION;
            result.y.value = s.value * a.x.value >> FixedLut.PRECISION;
            return result;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed2 Clamp(Fixed2 num, Fixed2 min, Fixed2 max)
        {
            Fixed2 r;

            if (num.x.value < min.x.value)
            {
                r.x = min.x;
            }
            else {
                r.x = num.x.value > max.x.value ? max.x : num.x;
            }

            if (num.y.value < min.y.value)
            {
                r.y = min.y;
            }
            else {
                r.y = num.y.value > max.y.value ? max.y : num.y;
            }

            return r;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed2 ClampMagnitude(Fixed2 v, Fixed length)
        {
            Fixed2 value = v;
            Fixed r;

            r.value =
                ((value.x.value * value.x.value) >> FixedLut.PRECISION) +
                ((value.y.value * value.y.value) >> FixedLut.PRECISION);
            if (r.value <= ((length.value * length.value) >> FixedLut.PRECISION))
            {
            }
            else
            {
                Fixed2 v1 = value;
                Fixed m = default;
                Fixed r2;

                r2.value =
                    ((v1.x.value * v1.x.value) >> FixedLut.PRECISION) +
                    ((v1.y.value * v1.y.value) >> FixedLut.PRECISION);
                Fixed r1;

                if (r2.value == 0)
                {
                    r1.value = 0;
                }
                else
                {
                    var b = (r2.value >> 1) + 1L;
                    var c = (b + (r2.value / b)) >> 1;

                    while (c < b)
                    {
                        b = c;
                        c = (b + (r2.value / b)) >> 1;
                    }

                    r1.value = b << (FixedLut.PRECISION >> 1);
                }

                m = r1;

                if (m.value <= Fixed.epsilon.value)
                {
                    v1 = default;
                }
                else
                {
                    v1.x.value = ((v1.x.value << FixedLut.PRECISION) / m.value);
                    v1.y.value = ((v1.y.value << FixedLut.PRECISION) / m.value);
                }

                value = v1 * length;
            }

            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed Magnitude(Fixed2 v)
        {
            Fixed r;

            r.value =
                ((v.x.value * v.x.value) >> FixedLut.PRECISION) +
                ((v.y.value * v.y.value) >> FixedLut.PRECISION);
            Fixed r1;

            if (r.value == 0)
            {
                r1.value = 0;
            }
            else
            {
                var b = (r.value >> 1) + 1L;
                var c = (b + (r.value / b)) >> 1;

                while (c < b)
                {
                    b = c;
                    c = (b + (r.value / b)) >> 1;
                }

                r1.value = b << (FixedLut.PRECISION >> 1);
            }

            return r1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed MagnitudeSqr(Fixed2 v)
        {
            v.x.value =
                ((v.x.value * v.x.value) >> FixedLut.PRECISION) +
                ((v.y.value * v.y.value) >> FixedLut.PRECISION);

            return v.x;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed Distance(Fixed2 a, Fixed2 b)
        {
            Fixed2 v;

            v.x.value = a.x.value - b.x.value;
            v.y.value = a.y.value - b.y.value;

            Fixed r;

            r.value =
                ((v.x.value * v.x.value) >> FixedLut.PRECISION) +
                ((v.y.value * v.y.value) >> FixedLut.PRECISION);
            Fixed r1;

            if (r.value == 0)
            {
                r1.value = 0;
            }
            else
            {
                var b1 = (r.value >> 1) + 1L;
                var c  = (b1 + (r.value / b1)) >> 1;

                while (c < b1)
                {
                    b1 = c;
                    c  = (b1 + (r.value / b1)) >> 1;
                }

                r1.value = b1 << (FixedLut.PRECISION >> 1);
            }

            return r1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed DistanceSqr(Fixed2 a, Fixed2 b)
        {
            var x = a.x.value - b.x.value;
            var z = a.y.value - b.y.value;

            a.x.value = ((x * x) >> FixedLut.PRECISION) + ((z * z) >> FixedLut.PRECISION);
            return a.x;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed2 Normalize(Fixed2 v)
        {
            Fixed2 v1 = v;
            Fixed m = default;
            Fixed r;

            r.value =
                ((v1.x.value * v1.x.value) >> FixedLut.PRECISION) +
                ((v1.y.value * v1.y.value) >> FixedLut.PRECISION);
            Fixed r1;

            if (r.value == 0)
            {
                r1.value = 0;
            }
            else
            {
                var b = (r.value >> 1) + 1L;
                var c = (b + (r.value / b)) >> 1;

                while (c < b)
                {
                    b = c;
                    c = (b + (r.value / b)) >> 1;
                }

                r1.value = b << (FixedLut.PRECISION >> 1);
            }

            m = r1;

            if (m.value <= Fixed.epsilon.value)
            {
                v1 = default;
            }
            else
            {
                v1.x.value = ((v1.x.value << FixedLut.PRECISION) / m.value);
                v1.y.value = ((v1.y.value << FixedLut.PRECISION) / m.value);
            }

            return v1;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed2 Normalize(Fixed2 v, out Fixed magnitude)
        {
            if (v == Fixed2.zero)
            {
                magnitude = Fixed._0;
                return Fixed2.zero;
            }

            magnitude = Magnitude(v);
            return v / magnitude;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed2 Lerp(Fixed2 from, Fixed2 to, Fixed t) {
            t = Clamp01(t);
            return new Fixed2(LerpUnclamped(from.x, to.x, t), LerpUnclamped(from.y, to.y, t));
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed2 LerpUnclamped(Fixed2 from, Fixed2 to, Fixed t)
        {
            return new Fixed2(LerpUnclamped(from.x, to.x, t), LerpUnclamped(from.y, to.y, t));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed Angle(Fixed2 a, Fixed2 b)
        {
            Fixed2 v = a;
            Fixed m = default;
            Fixed r2;

            r2.value =
                ((v.x.value * v.x.value) >> FixedLut.PRECISION) +
                ((v.y.value * v.y.value) >> FixedLut.PRECISION);
            Fixed r1;

            if (r2.value == 0)
            {
                r1.value = 0;
            }
            else
            {
                var b2 = (r2.value >> 1) + 1L;
                var c  = (b2 + (r2.value / b2)) >> 1;

                while (c < b2)
                {
                    b2 = c;
                    c  = (b2 + (r2.value / b2)) >> 1;
                }

                r1.value = b2 << (FixedLut.PRECISION >> 1);
            }

            m = r1;

            if (m.value <= Fixed.epsilon.value)
            {
                v = default;
            }
            else
            {
                v.x.value = ((v.x.value << FixedLut.PRECISION) / m.value);
                v.y.value = ((v.y.value << FixedLut.PRECISION) / m.value);
            }

            Fixed2 v1 = b;
            Fixed m1 = default;
            Fixed r3;

            r3.value =
                ((v1.x.value * v1.x.value) >> FixedLut.PRECISION) +
                ((v1.y.value * v1.y.value) >> FixedLut.PRECISION);
            Fixed r4;

            if (r3.value == 0)
            {
                r4.value = 0;
            }
            else
            {
                var b3 = (r3.value >> 1) + 1L;
                var c1 = (b3 + (r3.value / b3)) >> 1;

                while (c1 < b3)
                {
                    b3 = c1;
                    c1 = (b3 + (r3.value / b3)) >> 1;
                }

                r4.value = b3 << (FixedLut.PRECISION >> 1);
            }

            m1 = r4;

            if (m1.value <= Fixed.epsilon.value)
            {
                v1 = default;
            }
            else
            {
                v1.x.value = ((v1.x.value << FixedLut.PRECISION) / m1.value);
                v1.y.value = ((v1.y.value << FixedLut.PRECISION) / m1.value);
            }

            Fixed2 a1 = v;
            Fixed2 b1 = v1;
            var x = ((a1.x.value * b1.x.value) >> FixedLut.PRECISION);
            var z = ((a1.y.value * b1.y.value) >> FixedLut.PRECISION);

            Fixed r;

            r.value = x + z;
            var dot = r;
            Fixed min = -Fixed._1;
            Fixed max = +Fixed._1;
            Fixed ret;
            if (dot.value < min.value)
            {
                ret = min;
            }
            else {
                ret = dot.value > max.value ? max : dot;
            }

            return new Fixed(FixedLut.acos(ret.value)) * Fixed.rad2deg;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed Radians(Fixed2 a, Fixed2 b)
        {
            Fixed2 v = a;
            Fixed m = default;
            Fixed r2;

            r2.value =
                ((v.x.value * v.x.value) >> FixedLut.PRECISION) +
                ((v.y.value * v.y.value) >> FixedLut.PRECISION);
            Fixed r1;

            if (r2.value == 0)
            {
                r1.value = 0;
            }
            else
            {
                var b2 = (r2.value >> 1) + 1L;
                var c  = (b2 + (r2.value / b2)) >> 1;

                while (c < b2)
                {
                    b2 = c;
                    c  = (b2 + (r2.value / b2)) >> 1;
                }

                r1.value = b2 << (FixedLut.PRECISION >> 1);
            }

            m = r1;

            if (m.value <= Fixed.epsilon.value)
            {
                v = default;
            }
            else
            {
                v.x.value = ((v.x.value << FixedLut.PRECISION) / m.value);
                v.y.value = ((v.y.value << FixedLut.PRECISION) / m.value);
            }

            Fixed2 v1 = b;
            Fixed m1 = default;
            Fixed r3;

            r3.value =
                ((v1.x.value * v1.x.value) >> FixedLut.PRECISION) +
                ((v1.y.value * v1.y.value) >> FixedLut.PRECISION);
            Fixed r4;

            if (r3.value == 0)
            {
                r4.value = 0;
            }
            else
            {
                var b3 = (r3.value >> 1) + 1L;
                var c1 = (b3 + (r3.value / b3)) >> 1;

                while (c1 < b3)
                {
                    b3 = c1;
                    c1 = (b3 + (r3.value / b3)) >> 1;
                }

                r4.value = b3 << (FixedLut.PRECISION >> 1);
            }

            m1 = r4;

            if (m1.value <= Fixed.epsilon.value)
            {
                v1 = default;
            }
            else
            {
                v1.x.value = ((v1.x.value << FixedLut.PRECISION) / m1.value);
                v1.y.value = ((v1.y.value << FixedLut.PRECISION) / m1.value);
            }

            Fixed2 a1 = v;
            Fixed2 b1 = v1;
            var x = ((a1.x.value * b1.x.value) >> FixedLut.PRECISION);
            var z = ((a1.y.value * b1.y.value) >> FixedLut.PRECISION);

            Fixed r;

            r.value = x + z;
            var dot = r;
            Fixed min = -Fixed._1;
            Fixed max = +Fixed._1;
            Fixed ret;
            if (dot.value < min.value)
            {
                ret = min;
            }
            else
            {
                if (dot.value > max.value)
                {
                    ret = max;
                }
                else
                {
                    ret = dot;
                }
            }

            return new Fixed(FixedLut.acos(ret.value));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed RadiansSigned(Fixed2 a, Fixed2 b)
        {
            Fixed2 v = a;
            Fixed m = default;
            Fixed r2;

            r2.value =
                ((v.x.value * v.x.value) >> FixedLut.PRECISION) +
                ((v.y.value * v.y.value) >> FixedLut.PRECISION);
            Fixed r1;

            if (r2.value == 0)
            {
                r1.value = 0;
            }
            else
            {
                var b2 = (r2.value >> 1) + 1L;
                var c  = (b2 + (r2.value / b2)) >> 1;

                while (c < b2)
                {
                    b2 = c;
                    c  = (b2 + (r2.value / b2)) >> 1;
                }

                r1.value = b2 << (FixedLut.PRECISION >> 1);
            }

            m = r1;

            if (m.value <= Fixed.epsilon.value)
            {
                v = default;
            }
            else
            {
                v.x.value = ((v.x.value << FixedLut.PRECISION) / m.value);
                v.y.value = ((v.y.value << FixedLut.PRECISION) / m.value);
            }

            Fixed2 v1 = b;
            Fixed m1 = default;
            Fixed r3;

            r3.value =
                ((v1.x.value * v1.x.value) >> FixedLut.PRECISION) +
                ((v1.y.value * v1.y.value) >> FixedLut.PRECISION);
            Fixed r4;

            if (r3.value == 0)
            {
                r4.value = 0;
            }
            else
            {
                var b3 = (r3.value >> 1) + 1L;
                var c1 = (b3 + (r3.value / b3)) >> 1;

                while (c1 < b3)
                {
                    b3 = c1;
                    c1 = (b3 + (r3.value / b3)) >> 1;
                }

                r4.value = b3 << (FixedLut.PRECISION >> 1);
            }

            m1 = r4;

            if (m1.value <= Fixed.epsilon.value)
            {
                v1 = default;
            }
            else
            {
                v1.x.value = ((v1.x.value << FixedLut.PRECISION) / m1.value);
                v1.y.value = ((v1.y.value << FixedLut.PRECISION) / m1.value);
            }

            Fixed2 a1 = v;
            Fixed2 b1 = v1;
            var x = ((a1.x.value * b1.x.value) >> FixedLut.PRECISION);
            var z = ((a1.y.value * b1.y.value) >> FixedLut.PRECISION);

            Fixed r;

            r.value = x + z;
            var dot = r;
            Fixed min = -Fixed._1;
            Fixed max = +Fixed._1;
            Fixed ret;
            if (dot.value < min.value)
            {
                ret = min;
            }
            else
            {
                if (dot.value > max.value)
                {
                    ret = max;
                }
                else
                {
                    ret = dot;
                }
            }

            var rad  = new Fixed(FixedLut.acos(ret.value));
            var sign = ((a.x * b.y - a.y * b.x).value <  FixedLut.ZERO) ? Fixed.minus_one : Fixed._1;

            return rad * sign;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed RadiansSkipNormalize(Fixed2 a, Fixed2 b)
        {
            Fixed2 a1 = a;
            Fixed2 b1 = b;
            var x = ((a1.x.value * b1.x.value) >> FixedLut.PRECISION);
            var z = ((a1.y.value * b1.y.value) >> FixedLut.PRECISION);

            Fixed r;

            r.value = x + z;
            var dot = r;
            Fixed min = -Fixed._1;
            Fixed max = +Fixed._1;
            Fixed ret;
            if (dot.value < min.value)
            {
                ret = min;
            }
            else {
                ret = dot.value > max.value ? max : dot;
            }

            return new Fixed(FixedLut.acos(ret.value));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed RadiansSignedSkipNormalize(Fixed2 a, Fixed2 b)
        {
            Fixed2 a1 = a;
            Fixed2 b1 = b;
            var x = ((a1.x.value * b1.x.value) >> FixedLut.PRECISION);
            var z = ((a1.y.value * b1.y.value) >> FixedLut.PRECISION);

            Fixed r;

            r.value = x + z;
            var dot = r;
            Fixed min = -Fixed._1;
            Fixed max = +Fixed._1;
            Fixed ret;
            if (dot.value < min.value)
            {
                ret = min;
            }
            else
            {
                if (dot.value > max.value)
                {
                    ret = max;
                }
                else
                {
                    ret = dot;
                }
            }

            var rad  = new Fixed(FixedLut.acos(ret.value));
            var sign = ((a.x * b.y - a.y * b.x).value < FixedLut.ZERO) ? Fixed.minus_one : Fixed._1;

            return rad * sign;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed2 Reflect(Fixed2 vector, Fixed2 normal)
        {
            Fixed dot = (vector.x * normal.x) + (vector.y * normal.y);

            Fixed2 result;

            result.x = vector.x - ((Fixed._2 * dot) * normal.x);
            result.y = vector.y - ((Fixed._2 * dot) * normal.y);

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed2 Rotate(Fixed2 vector, Fixed angle)
        {
            Fixed2 vector1 = vector;
            var cs = Cos(angle);
            var sn = Sin(angle);

            var px = (vector1.x * cs) - (vector1.y * sn);
            var pz = (vector1.x * sn) + (vector1.y * cs);

            vector1.x = px;
            vector1.y = pz;

            return vector1;
        }
    }
}