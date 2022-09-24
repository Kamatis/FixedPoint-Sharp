using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Deterministic.FixedPoint {
    [StructLayout(LayoutKind.Explicit, Size = SIZE)]
    public struct Fixed4 : IEquatable<Fixed4> {
        public const int SIZE = 32;

        [FieldOffset(0)]
        public Fixed x;

        [FieldOffset(8)]
        public Fixed y;

        [FieldOffset(16)]
        public Fixed z;

        [FieldOffset(24)]
        public Fixed w;

        public static readonly Fixed4 zero;
        public static readonly Fixed4 one       = new Fixed4 {x = Fixed._1, y        = Fixed._1, z        = Fixed._1, w        = Fixed._1};
        public static readonly Fixed4 minus_one = new Fixed4 {x = Fixed.minus_one, y = Fixed.minus_one, z = Fixed.minus_one, w = Fixed.minus_one};

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Fixed4(Fixed x, Fixed y, Fixed z, Fixed w) {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Fixed4(Fixed2 xy, Fixed2 zw) {
            x = xy.x;
            y = xy.y;
            z = zw.x;
            w = zw.y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Fixed4(Fixed3 v, Fixed w) {
            x      = v.x;
            y      = v.y;
            z      = v.z;
            this.w = w;
        }

        /// <summary>
        /// Initializes fp4 vector with 48.16 fp format long values
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal Fixed4(long x, long y, long z, long w) {
            this.x.value = x;
            this.y.value = y;
            this.z.value = z;
            this.w.value = w;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed4 operator *(Fixed4 lhs, Fixed4 rhs) {
            return new Fixed4((lhs.x.value * rhs.x.value) >> FixedLut.PRECISION, (lhs.y.value * rhs.y.value) >> FixedLut.PRECISION,
                (lhs.z.value * rhs.z.value) >> FixedLut.PRECISION, (lhs.w.value * rhs.w.value) >> FixedLut.PRECISION);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed4 operator *(Fixed4 lhs, Fixed rhs) {
            return new Fixed4(lhs.x * rhs, lhs.y * rhs, lhs.z * rhs, lhs.w * rhs);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed4 operator *(Fixed lhs, Fixed4 rhs) {
            return new Fixed4(lhs * rhs.x, lhs * rhs.y, lhs * rhs.z, lhs * rhs.w);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed4 operator +(Fixed4 lhs, Fixed4 rhs) {
            return new Fixed4(lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z, lhs.w + rhs.w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed4 operator +(Fixed4 lhs, Fixed rhs) {
            return new Fixed4(lhs.x + rhs, lhs.y + rhs, lhs.z + rhs, lhs.w + rhs);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed4 operator +(Fixed lhs, Fixed4 rhs) {
            return new Fixed4(lhs + rhs.x, lhs + rhs.y, lhs + rhs.z, lhs + rhs.w);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed4 operator -(Fixed4 lhs, Fixed4 rhs) {
            return new Fixed4(lhs.x - rhs.x, lhs.y - rhs.y, lhs.z - rhs.z, lhs.w - rhs.w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed4 operator -(Fixed4 lhs, Fixed rhs) {
            return new Fixed4(lhs.x - rhs, lhs.y - rhs, lhs.z - rhs, lhs.w - rhs);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed4 operator -(Fixed lhs, Fixed4 rhs) {
            return new Fixed4(lhs - rhs.x, lhs - rhs.y, lhs - rhs.z, lhs - rhs.w);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed4 operator /(Fixed4 lhs, Fixed4 rhs) {
            return new Fixed4(lhs.x / rhs.x, lhs.y / rhs.y, lhs.z / rhs.z, lhs.w / rhs.w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed4 operator /(Fixed4 lhs, Fixed rhs) {
            return new Fixed4(lhs.x / rhs, lhs.y / rhs, lhs.z / rhs, lhs.w / rhs);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed4 operator /(Fixed lhs, Fixed4 rhs) {
            return new Fixed4(lhs / rhs.x, lhs / rhs.y, lhs / rhs.z, lhs / rhs.w);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed4 operator %(Fixed4 lhs, Fixed4 rhs) {
            return new Fixed4(lhs.x % rhs.x, lhs.y % rhs.y, lhs.z % rhs.z, lhs.w % rhs.w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed4 operator %(Fixed4 lhs, Fixed rhs) {
            return new Fixed4(lhs.x % rhs, lhs.y % rhs, lhs.z % rhs, lhs.w % rhs);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed4 operator %(Fixed lhs, Fixed4 rhs) {
            return new Fixed4(lhs % rhs.x, lhs % rhs.y, lhs % rhs.z, lhs % rhs.w);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Fixed4 a, Fixed4 b) {
            return a.x.value == b.x.value && a.y.value == b.y.value && a.z.value == b.z.value && a.w.value == b.w.value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Fixed4 a, Fixed4 b) {
            return a.x.value != b.x.value || a.y.value != b.y.value || a.z.value != b.z.value || a.w.value != b.w.value;
        }

        public bool Equals(Fixed4 other) {
            return x.Equals(other.x) && y.Equals(other.y) && z.Equals(other.z) && w.Equals(other.w);
        }

        public override bool Equals(object obj) {
            return obj is Fixed4 other && Equals(other);
        }

        public override int GetHashCode() {
            unchecked {
                var hashCode = x.GetHashCode();
                hashCode = (hashCode * 397) ^ y.GetHashCode();
                hashCode = (hashCode * 397) ^ z.GetHashCode();
                hashCode = (hashCode * 397) ^ w.GetHashCode();
                return hashCode;
            }
        }
    }
}