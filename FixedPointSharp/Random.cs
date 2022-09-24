using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Deterministic.FixedPoint {
    [StructLayout(LayoutKind.Explicit, Size = SIZE)]
    public struct Random {
        public const int SIZE = 4;

        [FieldOffset(0)]
        public uint state;
        
        /// <summary>
        /// Seed must be non-zero
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Random(uint seed)
        {
            state = seed;
            NextState();
        }

        /// <summary>
        /// Seed must be non-zero
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetState(uint seed)
        {
            state = seed;
            NextState();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private uint NextState() {
            var t  = state;
            state ^= state << 13;
            state ^= state >> 17;
            state ^= state << 5;
            return t;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool NextBool()
        {
            return (NextState() & 1) == 1;
        }

        /// <summary>Returns value in range [-2147483647, 2147483647]</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int NextInt()
        {
            return (int)NextState() ^ -2147483648;
        }
        
        /// <summary>Returns value in range [0, max]</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int NextInt(int max)
        {
            return (int)((NextState() * (ulong)max) >> 32);
        }
        
        /// <summary>Returns value in range [min, max].</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int NextInt(int min, int max)
        {
            var range = (uint)(max - min);
            return (int)(NextState() * (ulong)range >> 32) + min;
        }
        
        /// <summary>Returns value in range [0, 1]</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Fixed NextFp() {
            return new Fixed(NextInt(0, 65535));
        }
        
        /// <summary>Returns vector with all components in range [0, 1]</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Fixed2 NextFp2() {
            return new Fixed2(NextFp(), NextFp());
        }
        
        /// <summary>Returns vector with all components in range [0, 1]</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Fixed3 NextFp3() {
            return new Fixed3(NextFp(), NextFp(), NextFp());
        }
        
        /// <summary>Returns vector with all components in range [0, 1]</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Fixed4 NextFp4() {
            return new Fixed4(NextFp(), NextFp(), NextFp(), NextFp());
        }


        /// <summary>Returns value in range [0, max]</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Fixed NextFp(Fixed max) {
            return NextFp() * max;
        }
        
        /// <summary>Returns vector with all components in range [0, max]</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Fixed2 NextFp2(Fixed2 max) {
            return NextFp2() * max;
        }
        
        /// <summary>Returns vector with all components in range [0, max]</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Fixed3 NextFp3(Fixed3 max) {
            return NextFp3() * max;
        }
        
        /// <summary>Returns vector with all components in range [0, max]</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Fixed4 NextFp4(Fixed4 max) {
            return NextFp4() * max;
        }

        /// <summary>Returns value in range [min, max]</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Fixed NextFp(Fixed min, Fixed max) {
            return NextFp() * (max - min) + min;
        }

        /// <summary>Returns vector with all components in range [min, max]</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Fixed2 NextFp2(Fixed2 min, Fixed2 max) {
            return NextFp2() * (max - min) + min;
        }
        
        /// <summary>Returns vector with all components in range [min, max]</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Fixed3 NextFp3(Fixed3 min, Fixed3 max) {
            return NextFp3() * (max - min) + min;
        }
        
        /// <summary>Returns vector with all components in range [min, max]</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Fixed4 NextFp4(Fixed4 min, Fixed4 max) {
            return NextFp4() * (max - min) + min;
        }
        
        /// <summary>Returns a normalized 2D direction</summary>
        public Fixed2 NextDirection2D() {
            var angle = NextFp() * Fixed.pi * Fixed._2;
            FixedMath.SinCos(angle, out var sin, out var cos);
            return new Fixed2(sin,cos);
        }
        
        /// <summary>Returns a normalized 3D direction</summary>
        public Fixed3 NextDirection3D() {
            var z = NextFp(Fixed._2) - Fixed._1;
            var r = FixedMath.Sqrt(FixedMath.Max(Fixed._1 - z * z, Fixed._0));
            var angle = NextFp(Fixed.pi2);
            FixedMath.SinCos(angle, out var sin, out var cos);
            return new Fixed3(cos * r, sin * r, z);
        }
    }
}