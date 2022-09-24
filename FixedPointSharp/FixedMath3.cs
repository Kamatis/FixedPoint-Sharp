using System.Runtime.CompilerServices;

namespace Deterministic.FixedPoint
{
    public partial struct FixedMath
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed Sum(Fixed3 v) {
            return new Fixed(v.x.value + v.y.value + v.z.value);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed3 Abs(Fixed3 v)
        {
            return new Fixed3(Abs(v.x), Abs(v.y), Abs(v.z));
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed Dot(Fixed3 a, Fixed3 b)
        {
            return new Fixed(((a.x.value * b.x.value) >> FixedLut.PRECISION) + ((a.y.value * b.y.value) >> FixedLut.PRECISION) + ((a.z.value * b.z.value) >> FixedLut.PRECISION));
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed3 Cross(Fixed3 a, Fixed3 b)
        {
            Fixed3 r;

            r.x = a.y * b.z - a.z * b.y;
            r.y = a.z * b.x - a.x * b.z;
            r.z = a.x * b.y - a.y * b.x;
            
            return r;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed Distance(Fixed3 p1, Fixed3 p2)
        {
            Fixed3 v;
            v.x.value = p1.x.value - p2.x.value;
            v.y.value = p1.y.value - p2.y.value;
            v.z.value = p1.z.value - p2.z.value;

            return Magnitude(v);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed DistanceSqr(Fixed3 p1, Fixed3 p2)
        {
            Fixed3 v;
            v.x.value = p1.x.value - p2.x.value;
            v.y.value = p1.y.value - p2.y.value;
            v.z.value = p1.z.value - p2.z.value;

            return MagnitudeSqr(v);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed3 Lerp(Fixed3 from, Fixed3 to, Fixed t)
        {
            t = Clamp01(t);
            return new Fixed3(LerpUnclamped(from.x, to.x, t), LerpUnclamped(from.y, to.y, t), LerpUnclamped(from.z, to.z, t));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed3 LerpUnclamped(Fixed3 from, Fixed3 to, Fixed t)
        {
            return new Fixed3(LerpUnclamped(from.x, to.x, t), LerpUnclamped(from.y, to.y, t), LerpUnclamped(from.z, to.z, t));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed3 Reflect(Fixed3 vector, Fixed3 normal)
        {
            var num = -Fixed._2 * Dot(normal, vector);
            return new Fixed3(num * normal.x + vector.x, num * normal.y + vector.y, num * normal.z + vector.z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed3 Project(Fixed3 vector, Fixed3 normal)
        {
            var sqrMag = MagnitudeSqr(normal);
            if (sqrMag < Fixed.epsilon)
                return Fixed3.zero;

            var dot = Dot(vector, normal);
            return normal * dot / sqrMag;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed3 ProjectOnPlane(Fixed3 vector, Fixed3 planeNormal)
        {
            var sqrMag = MagnitudeSqr(planeNormal);
            if (sqrMag < Fixed.epsilon)
                return vector;

            var dot = Dot(vector, planeNormal);
            return vector - planeNormal * dot / sqrMag;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed3 MoveTowards(Fixed3 current, Fixed3 target, Fixed maxDelta)
        {
            var v = target - current;
            var sqrMagnitude = MagnitudeSqr(v);
            if (v == Fixed3.zero || maxDelta >= Fixed._0 && sqrMagnitude <= maxDelta * maxDelta)
                return target;

            var magnitude = Sqrt(sqrMagnitude);
            return current + v / magnitude * maxDelta;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed Angle(Fixed3 a, Fixed3 b)
        {
            var sqr = MagnitudeSqr(a) * MagnitudeSqr(b);
            var n = Sqrt(sqr);

            if (n < Fixed.epsilon)
            {
                return Fixed._0;
            }

            return Acos(Clamp(Dot(a, b) / n, Fixed.minus_one, Fixed._1)) * Fixed.rad2deg;
        }

        public static Fixed AngleSigned(Fixed3 a, Fixed3 b, Fixed3 axis)
        {
            var angle = Angle(a, b);
            long num2 = ((a.y.value * b.z.value) >> FixedLut.PRECISION) - ((a.z.value * b.y.value) >> FixedLut.PRECISION);
            long num3 = ((a.z.value * b.x.value) >> FixedLut.PRECISION) - ((a.x.value * b.z.value) >> FixedLut.PRECISION);
            long num4 = ((a.x.value * b.y.value) >> FixedLut.PRECISION) - ((a.y.value * b.x.value) >> FixedLut.PRECISION);
            var sign = (((axis.x.value * num2) >> FixedLut.PRECISION) +
                        ((axis.y.value * num3) >> FixedLut.PRECISION) +
                        ((axis.z.value * num4) >> FixedLut.PRECISION)) < 0
                ? Fixed.minus_one
                : Fixed._1;
            return angle * sign;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed Radians(Fixed3 a, Fixed3 b)
        {
            return Acos(Clamp(Dot(Normalize(a), Normalize(b)), Fixed.minus_one, Fixed._1));
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed RadiansSkipNormalize(Fixed3 a, Fixed3 b)
        {
            return Acos(Clamp(Dot(a, b), Fixed.minus_one, Fixed._1));
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed MagnitudeSqr(Fixed3 v)
        {
            v.x.value =
                ((v.x.value * v.x.value) >> FixedLut.PRECISION) +
                ((v.y.value * v.y.value) >> FixedLut.PRECISION) +
                ((v.z.value * v.z.value) >> FixedLut.PRECISION);

            return v.x;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed Magnitude(Fixed3 v)
        {
            v.x.value =
                ((v.x.value * v.x.value) >> FixedLut.PRECISION) +
                ((v.y.value * v.y.value) >> FixedLut.PRECISION) +
                ((v.z.value * v.z.value) >> FixedLut.PRECISION);
            
            return Sqrt(v.x);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed3 MagnitudeClamp(Fixed3 v, Fixed length)
        {
            var sqrMagnitude = MagnitudeSqr(v);
            if (sqrMagnitude <= length * length)
                return v;

            var magnitude = Sqrt(sqrMagnitude);
            var normalized = v / magnitude;
            return normalized * length;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed3 MagnitudeSet(Fixed3 v, Fixed length)
        {
            return Normalize(v) * length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed3 Min(Fixed3 a, Fixed3 b)
        {
            return new Fixed3(Min(a.x, b.x), Min(a.y, b.y), Min(a.z, b.z));
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed3 Max(Fixed3 a, Fixed3 b)
        {
            return new Fixed3(Max(a.x, b.x), Max(a.y, b.y), Max(a.z, b.z));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed3 Normalize(Fixed3 v)
        {
            if (v == Fixed3.zero)
                return Fixed3.zero;
            
            return v /  Magnitude(v);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed3 Normalize(Fixed3 v, out Fixed magnitude)
        {
            if (v == Fixed3.zero)
            {
                magnitude = Fixed._0;
                return Fixed3.zero;
            }

            magnitude = Magnitude(v);
            return v / magnitude;
        }
    }
}