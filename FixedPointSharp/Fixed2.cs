using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Deterministic.FixedPoint {
    [Serializable]
    [StructLayout(LayoutKind.Explicit, Size = SIZE)]
    public struct Fixed2 : IEquatable<Fixed2> {
        public const int SIZE = 16;

        public static readonly Fixed2 left      = new Fixed2(-Fixed._1,       Fixed._0);
        public static readonly Fixed2 right     = new Fixed2(Fixed._1,        Fixed._0);
        public static readonly Fixed2 up        = new Fixed2(Fixed._0,        Fixed._1);
        public static readonly Fixed2 down      = new Fixed2(Fixed._0,        -Fixed._1);
        public static readonly Fixed2 one       = new Fixed2(Fixed._1,        Fixed._1);
        public static readonly Fixed2 minus_one = new Fixed2(Fixed.minus_one, Fixed.minus_one);
        public static readonly Fixed2 zero      = new Fixed2(Fixed._0,        Fixed._0);

        [FieldOffset(0)]
        public Fixed x;

        [FieldOffset(8)]
        public Fixed y;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Fixed2(Fixed x, Fixed y) {
            this.x.value = x.value;
            this.y.value = y.value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed2 operator +(Fixed2 a, Fixed2 b) {
            a.x.value += b.x.value;
            a.y.value += b.y.value;
            return a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed2 operator -(Fixed2 a, Fixed2 b) {
            a.x.value -= b.x.value;
            a.y.value -= b.y.value;

            return a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed2 operator -(Fixed2 a) {
            a.x.value = -a.x.value;
            a.y.value = -a.y.value;

            return a;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed2 operator *(Fixed2 a, Fixed2 b) {
            a.x.value = (a.x.value * b.x.value) >> FixedLut.PRECISION;
            a.y.value = (a.y.value * b.y.value) >> FixedLut.PRECISION;

            return a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed2 operator *(Fixed2 a, Fixed b) {
            a.x.value = (a.x.value * b.value) >> FixedLut.PRECISION;
            a.y.value = (a.y.value * b.value) >> FixedLut.PRECISION;

            return a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed2 operator *(Fixed b, Fixed2 a) {
            a.x.value = (a.x.value * b.value) >> FixedLut.PRECISION;
            a.y.value = (a.y.value * b.value) >> FixedLut.PRECISION;

            return a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed2 operator /(Fixed2 a, Fixed2 b) {
            a.x.value = (a.x.value << FixedLut.PRECISION) / b.x.value;
            a.y.value = (a.y.value << FixedLut.PRECISION) / b.y.value;

            return a;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed2 operator /(Fixed2 a, Fixed b) {
            a.x.value = (a.x.value << FixedLut.PRECISION) / b.value;
            a.y.value = (a.y.value << FixedLut.PRECISION) / b.value;

            return a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed2 operator /(Fixed b, Fixed2 a) {
            a.x.value = (a.x.value << FixedLut.PRECISION) / b.value;
            a.y.value = (a.y.value << FixedLut.PRECISION) / b.value;

            return a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Fixed2 a, Fixed2 b) {
            return a.x.value == b.x.value && a.y.value == b.y.value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Fixed2 a, Fixed2 b) {
            return a.x.value != b.x.value || a.y.value != b.y.value;
        }

        public override bool Equals(object obj) {
            return obj is Fixed2 other && this == other;
        }

        public bool Equals(Fixed2 other) {
            return this == other;
        }

        public override int GetHashCode() {
            unchecked {
                return (x.GetHashCode() * 397) ^ y.GetHashCode();
            }
        }

        public override string ToString() {
            return $"({x}, {y})";
        }

        public class EqualityComparer : IEqualityComparer<Fixed2> {
            public static readonly EqualityComparer instance = new EqualityComparer();

            private EqualityComparer() { }

            bool IEqualityComparer<Fixed2>.Equals(Fixed2 x, Fixed2 y) {
                return x == y;
            }

            int IEqualityComparer<Fixed2>.GetHashCode(Fixed2 obj) {
                return obj.GetHashCode();
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Fixed2 Normalize() {
            return FixedMath.Normalize(this);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Fixed Magnitude() {
            return FixedMath.Magnitude(this);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Fixed MagnitudeSqr() {
            return FixedMath.MagnitudeSqr(this);
        }
    }
}