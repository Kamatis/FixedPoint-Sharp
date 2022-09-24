using System.Runtime.CompilerServices;

namespace Deterministic.FixedPoint {
    public partial struct FixedMath
    { 
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed Sum(Fixed4 v) {
            return new Fixed(v.x.value + v.y.value + v.z.value + v.w.value);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed4 Abs(Fixed4 v)
        {
            return new Fixed4(Abs(v.x), Abs(v.y), Abs(v.z), Abs(v.w));
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed Dot(Fixed4 a, Fixed4 b) {
            return new Fixed(((a.x.value * b.x.value) >> FixedLut.PRECISION) + ((a.y.value * b.y.value) >> FixedLut.PRECISION) + ((a.z.value * b.z.value) >> FixedLut.PRECISION) +
                          ((a.w.value * b.w.value) >> FixedLut.PRECISION));
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed4 Normalize(Fixed4 v)
        {
            if (v == Fixed4.zero)
                return Fixed4.zero;
            
            return v /  Magnitude(v);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed4 Normalize(Fixed4 v, out Fixed magnitude)
        {
            if (v == Fixed4.zero)
            {
                magnitude = Fixed._0;
                return Fixed4.zero;
            }

            magnitude = Magnitude(v);
            return v / magnitude;
        }
        
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed Distance(Fixed4 p1, Fixed4 p2)
        {
            Fixed4 v;
            v.x.value = p1.x.value - p2.x.value;
            v.y.value = p1.y.value - p2.y.value;
            v.z.value = p1.z.value - p2.z.value;
            v.w.value = p1.w.value - p2.w.value;

            return Magnitude(v);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed DistanceSqr(Fixed4 p1, Fixed4 p2)
        {
            Fixed4 v;
            v.x.value = p1.x.value - p2.x.value;
            v.y.value = p1.y.value - p2.y.value;
            v.z.value = p1.z.value - p2.z.value;
            v.w.value = p1.w.value - p2.w.value;

            return MagnitudeSqr(v);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed MagnitudeSqr(Fixed4 v)
        {
            v.x.value =
                ((v.x.value * v.x.value) >> FixedLut.PRECISION) +
                ((v.y.value * v.y.value) >> FixedLut.PRECISION) +
                ((v.z.value * v.z.value) >> FixedLut.PRECISION) +
                ((v.w.value * v.w.value) >> FixedLut.PRECISION);

            return v.x;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed Magnitude(Fixed4 v)
        {
            v.x.value =
                ((v.x.value * v.x.value) >> FixedLut.PRECISION) +
                ((v.y.value * v.y.value) >> FixedLut.PRECISION) +
                ((v.z.value * v.z.value) >> FixedLut.PRECISION) +
                ((v.w.value * v.w.value) >> FixedLut.PRECISION);
            
            return Sqrt(v.x);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed4 MagnitudeClamp(Fixed4 v, Fixed length)
        {
            var sqrMagnitude = MagnitudeSqr(v);
            if (sqrMagnitude <= length * length)
                return v;

            var magnitude  = Sqrt(sqrMagnitude);
            var normalized = v / magnitude;
            return normalized * length;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed4 MagnitudeSet(Fixed4 v, Fixed length)
        {
            return Normalize(v) * length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed4 Min(Fixed4 a, Fixed4 b)
        {
            return new Fixed4(Min(a.x, b.x), Min(a.y, b.y), Min(a.z, b.z), Min(a.w, b.w));
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed4 Max(Fixed4 a, Fixed4 b)
        {
            return new Fixed4(Max(a.x, b.x), Max(a.y, b.y), Max(a.z, b.z), Max(a.w, b.w));
        }
    }
}