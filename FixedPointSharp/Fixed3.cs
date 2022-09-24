using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Deterministic.FixedPoint {
    [Serializable]
    [StructLayout(LayoutKind.Explicit, Size = SIZE)]
    public struct Fixed3 : IEquatable<Fixed3> {
        public const int SIZE = 24;

        public static readonly Fixed3 left      = new Fixed3(-Fixed._1,       Fixed._0,        Fixed._0);
        public static readonly Fixed3 right     = new Fixed3(Fixed._1,        Fixed._0,        Fixed._0);
        public static readonly Fixed3 up        = new Fixed3(Fixed._0,        Fixed._1,        Fixed._0);
        public static readonly Fixed3 down      = new Fixed3(Fixed._0,        Fixed.minus_one, Fixed._0);
        public static readonly Fixed3 forward   = new Fixed3(Fixed._0,        Fixed._0,        Fixed._1);
        public static readonly Fixed3 backward  = new Fixed3(Fixed._0,        Fixed._0,        Fixed.minus_one);
        public static readonly Fixed3 one       = new Fixed3(Fixed._1,        Fixed._1,        Fixed._1);
        public static readonly Fixed3 minus_one = new Fixed3(Fixed.minus_one, Fixed.minus_one, Fixed.minus_one);
        public static readonly Fixed3 zero      = new Fixed3(Fixed._0,        Fixed._0,        Fixed._0);

        [FieldOffset(0)]
        public Fixed x;

        [FieldOffset(8)]
        public Fixed y;

        [FieldOffset(16)]
        public Fixed z;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Fixed3(Fixed x, Fixed y, Fixed z) {
            this.x.value = x.value;
            this.y.value = y.value;
            this.z.value = z.value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Fixed3(Fixed2 xy, Fixed z) {
            x.value      = xy.x.value;
            y.value      = xy.y.value;
            this.z.value = z.value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed3 operator +(Fixed3 a, Fixed3 b) {
            a.x.value += b.x.value;
            a.y.value += b.y.value;
            a.z.value += b.z.value;

            return a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed3 operator -(Fixed3 a, Fixed3 b) {
            a.x.value -= b.x.value;
            a.y.value -= b.y.value;
            a.z.value -= b.z.value;

            return a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed3 operator -(Fixed3 a) {
            a.x.value = -a.x.value;
            a.y.value = -a.y.value;
            a.z.value = -a.z.value;

            return a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed3 operator *(Fixed3 a, Fixed3 b) {
            a.x.value = (a.x.value * b.x.value) >> FixedLut.PRECISION;
            a.y.value = (a.y.value * b.y.value) >> FixedLut.PRECISION;
            a.z.value = (a.z.value * b.z.value) >> FixedLut.PRECISION;

            return a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed3 operator *(Fixed3 a, Fixed b) {
            a.x.value = (a.x.value * b.value) >> FixedLut.PRECISION;
            a.y.value = (a.y.value * b.value) >> FixedLut.PRECISION;
            a.z.value = (a.z.value * b.value) >> FixedLut.PRECISION;

            return a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed3 operator *(Fixed b, Fixed3 a) {
            a.x.value = (a.x.value * b.value) >> FixedLut.PRECISION;
            a.y.value = (a.y.value * b.value) >> FixedLut.PRECISION;
            a.z.value = (a.z.value * b.value) >> FixedLut.PRECISION;

            return a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed3 operator /(Fixed3 a, Fixed3 b) {
            a.x.value = (a.x.value << FixedLut.PRECISION) / b.x.value;
            a.y.value = (a.y.value << FixedLut.PRECISION) / b.y.value;
            a.z.value = (a.z.value << FixedLut.PRECISION) / b.z.value;

            return a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed3 operator /(Fixed3 a, Fixed b) {
            a.x.value = (a.x.value << FixedLut.PRECISION) / b.value;
            a.y.value = (a.y.value << FixedLut.PRECISION) / b.value;
            a.z.value = (a.z.value << FixedLut.PRECISION) / b.value;

            return a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed3 operator /(Fixed b, Fixed3 a) {
            a.x.value = (a.x.value << FixedLut.PRECISION) / b.value;
            a.y.value = (a.y.value << FixedLut.PRECISION) / b.value;
            a.z.value = (a.z.value << FixedLut.PRECISION) / b.value;

            return a;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Fixed3 a, Fixed3 b) {
            return a.x.value == b.x.value && a.y.value == b.y.value && a.z.value == b.z.value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Fixed3 a, Fixed3 b) {
            return a.x.value != b.x.value || a.y.value != b.y.value || a.z.value != b.z.value;
        }

        public bool Equals(Fixed3 other) {
            return x.Equals(other.x) && y.Equals(other.y) && z.Equals(other.z);
        }

        public override bool Equals(object obj) {
            return obj is Fixed3 other && this == other;
        }

        public override int GetHashCode() {
            unchecked {
                var hashCode = x.GetHashCode();
                hashCode = (hashCode * 397) ^ y.GetHashCode();
                hashCode = (hashCode * 397) ^ z.GetHashCode();
                return hashCode;
            }
        }

        public override string ToString() {
            return $"({x}, {y}, {z})";
        }

        public class EqualityComparer : IEqualityComparer<Fixed3> {
            public static readonly EqualityComparer instance = new EqualityComparer();

            private EqualityComparer() { }

            bool IEqualityComparer<Fixed3>.Equals(Fixed3 x, Fixed3 y) {
                return x == y;
            }

            int IEqualityComparer<Fixed3>.GetHashCode(Fixed3 obj) {
                return obj.GetHashCode();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Fixed3 Normalize() {
            return FixedMath.Normalize(this);
        }
    }
}