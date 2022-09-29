using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Deterministic.FixedPoint
{
    /// <summary>
    /// Based from Unity's Quaternion
    /// https://github.com/Unity-Technologies/UnityCsReference/blob/master/Runtime/Export/Math/Quaternion.cs
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = SIZE)]
    public struct FixedQuaternion : IEquatable<FixedQuaternion>
    {
        public const int SIZE = 32;

        [FieldOffset(0)]
        public Fixed x;

        [FieldOffset(8)]
        public Fixed y;

        [FieldOffset(16)]
        public Fixed z;

        [FieldOffset(24)]
        public Fixed w;

        public Fixed this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                switch (index)
                {
                    case 0: return x;
                    case 1: return y;
                    case 2: return z;
                    case 3: return w;
                    default:
                        throw new IndexOutOfRangeException("Invalid quaternion index!");
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                switch (index)
                {
                    case 0: x = value; break;
                    case 1: y = value; break;
                    case 2: z = value; break;
                    case 3: w = value; break;
                    default:
                        throw new IndexOutOfRangeException("Invalid quaternion index!");
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FixedQuaternion(Fixed x, Fixed y, Fixed z, Fixed w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Set(Fixed newX, Fixed newY, Fixed newZ, Fixed newW)
        {
            x = newX;
            y = newY;
            z = newZ;
            w = newW;
        }

        static readonly FixedQuaternion identityQuaternion = new FixedQuaternion(Fixed._0, Fixed._0, Fixed._0, Fixed._1);

        public static FixedQuaternion identity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return identityQuaternion;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FixedQuaternion operator *(FixedQuaternion lhs, FixedQuaternion rhs)
        {
            return new FixedQuaternion(
                lhs.w * rhs.x + lhs.x * rhs.w + lhs.y * rhs.z - lhs.z * rhs.y,
                lhs.w * rhs.y + lhs.y * rhs.w + lhs.z * rhs.x - lhs.x * rhs.z,
                lhs.w * rhs.z + lhs.z * rhs.w + lhs.x * rhs.y - lhs.y * rhs.x,
                lhs.w * rhs.w - lhs.x * rhs.x - lhs.y * rhs.y - lhs.z * rhs.z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed3 operator *(FixedQuaternion rotation, Fixed3 point)
        {
            Fixed x = rotation.x * Fixed._2;
            Fixed y = rotation.y * Fixed._2;
            Fixed z = rotation.z * Fixed._2;
            Fixed xx = rotation.x * x;
            Fixed yy = rotation.y * y;
            Fixed zz = rotation.z * z;
            Fixed xy = rotation.x * y;
            Fixed xz = rotation.x * z;
            Fixed yz = rotation.y * z;
            Fixed wx = rotation.w * x;
            Fixed wy = rotation.w * y;
            Fixed wz = rotation.w * z;

            Fixed3 res;
            res.x = (Fixed._1 - (yy + zz)) * point.x + (xy - wz) * point.y + (xz + wy) * point.z;
            res.y = (xy + wz) * point.x + (Fixed._1 - (xx + zz)) * point.y + (yz - wx) * point.z;
            res.z = (xz - wy) * point.x + (yz + wx) * point.y + (Fixed._1 - (xx + yy)) * point.z;
            return res;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsEqualUsingDot(Fixed dot)
        {
            return dot > Fixed._1 - Fixed.epsilon;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(FixedQuaternion lhs, FixedQuaternion rhs)
        {
            return IsEqualUsingDot(Dot(lhs, rhs));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(FixedQuaternion lhs, FixedQuaternion rhs)
        {
            return !(lhs == rhs);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed Dot(FixedQuaternion a, FixedQuaternion b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetLookRotation(Fixed3 view)
        {
            Fixed3 up = Fixed3.up;
            SetLookRotation(view, up);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetLookRotation(Fixed3 view, Fixed3 up)
        {
            this = LookRotation(view, up);
        }

        // from http://answers.unity3d.com/questions/467614/what-is-the-source-code-of-quaternionlookrotation.html
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static FixedQuaternion LookRotation(Fixed3 forward, Fixed3 up)
        {

            forward = FixedMath.Normalize(forward);
            Fixed3 right = FixedMath.Normalize(FixedMath.Cross(up, forward));
            up = FixedMath.Cross(forward, right);
            var m00 = right.x;
            var m01 = right.y;
            var m02 = right.z;
            var m10 = up.x;
            var m11 = up.y;
            var m12 = up.z;
            var m20 = forward.x;
            var m21 = forward.y;
            var m22 = forward.z;


            Fixed num8 = (m00 + m11) + m22;
            var quaternion = new FixedQuaternion();
            if (num8 > Fixed._0)
            {
                var num = FixedMath.Sqrt(num8 + Fixed._1);
                quaternion.w = num * Fixed._0_50;
                num = Fixed._0_50 / num;
                quaternion.x = (m12 - m21) * num;
                quaternion.y = (m20 - m02) * num;
                quaternion.z = (m01 - m10) * num;
                return quaternion;
            }
            if ((m00 >= m11) && (m00 >= m22))
            {
                var num7 = FixedMath.Sqrt(((Fixed._1 + m00) - m11) - m22);
                var num4 = Fixed._0_50 / num7;
                quaternion.x = Fixed._0_50 * num7;
                quaternion.y = (m01 + m10) * num4;
                quaternion.z = (m02 + m20) * num4;
                quaternion.w = (m12 - m21) * num4;
                return quaternion;
            }
            if (m11 > m22)
            {
                var num6 = FixedMath.Sqrt(((Fixed._1 + m11) - m00) - m22);
                var num3 = Fixed._0_50 / num6;
                quaternion.x = (m10 + m01) * num3;
                quaternion.y = Fixed._0_50 * num6;
                quaternion.z = (m21 + m12) * num3;
                quaternion.w = (m20 - m02) * num3;
                return quaternion;
            }
            var num5 = FixedMath.Sqrt(((Fixed._1 + m22) - m00) - m11);
            var num2 = Fixed._0_50 / num5;
            quaternion.x = (m20 + m02) * num2;
            quaternion.y = (m21 + m12) * num2;
            quaternion.z = Fixed._0_50 * num5;
            quaternion.w = (m01 - m10) * num2;

            return quaternion;
        }

        /// <summary>
        /// Geths the angle in degrees between two rotation <paramref name="a"/> and <paramref name="b"/>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed Angle(FixedQuaternion a, FixedQuaternion b)
        {
            Fixed dot = FixedMath.Min(FixedMath.Abs(Dot(a, b)), Fixed._1);
            return IsEqualUsingDot(dot) ? Fixed._0 : FixedMath.Acos(dot) * Fixed._2 * Fixed.rad2deg;
        }

#if DEBUG
        public
#endif
        static Fixed3 Internal_MakePositive(Fixed3 euler)
        {
            // TODO: Make static for -0.0001f
            Fixed negativeFlip = Fixed.minus_one / Fixed._100 / Fixed._100 * Fixed.rad2deg;
            Fixed positiveFlip = Fixed._360 + negativeFlip;

            if (euler.x < negativeFlip)
            {
                euler.x += Fixed._360;
            }
            else if (euler.x > positiveFlip)
            {
                euler.x -= Fixed._360;
            }

            if (euler.y < negativeFlip)
            {
                euler.y += Fixed._360;
            }
            else if (euler.y > positiveFlip)
            {
                euler.y -= Fixed._360;
            }

            if (euler.z < negativeFlip)
            {
                euler.z += Fixed._360;
            }
            else if (euler.z > positiveFlip)
            {
                euler.z -= Fixed._360;
            }

            return euler;
        }

        public Fixed3 eulerAngles
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Internal_MakePositive(Internal_ToEulerRad(this) * Fixed.rad2deg); }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set { this = Internal_FromEulerRad(value * Fixed.deg2rad); }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FixedQuaternion Normalize(FixedQuaternion q)
        {
            Fixed mag = FixedMath.Sqrt(Dot(q, q));
            if(mag < Fixed.epsilon)
            {
                return FixedQuaternion.identity;
            }

            return new FixedQuaternion(q.x / mag, q.y / mag, q.z / mag, q.w / mag);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Normalize()
        {
            this = Normalize(this);
        }

        public FixedQuaternion normalized
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return Normalize(this);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            return x.GetHashCode() ^ (y.GetHashCode() << 2) ^ (z.GetHashCode() >> 2) ^ (w.GetHashCode() >> 1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object other)
        {
            if(!(other is FixedQuaternion))
            {
                return false;
            }

            return Equals((FixedQuaternion)other);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(FixedQuaternion other)
        {
            return x.Equals(other.x) && y.Equals(other.y) && z.Equals(other.z) && w.Equals(other.w);
        }

        public override string ToString()
        {
            return $"({x}, {y}, {z}, {w})";
        }

        // converted from https://en.wikipedia.org/wiki/Conversion_between_quaternions_and_Euler_angles
#if DEBUG
        public
#endif
        static Fixed3 Internal_ToEulerRad(FixedQuaternion fq)
        {
            Fixed3 angles;

            // x-axis rotation
            Fixed sinr_cosp = 2 * (fq.w * fq.x + fq.y * fq.z);
            Fixed cosr_cosp = 1 - 2 * (fq.x * fq.x + fq.y * fq.y);
            angles.x = FixedMath.Atan2(sinr_cosp, cosr_cosp);

            // y-axis rotation
            Fixed sinp = 2 * (fq.w * fq.y - fq.z * fq.x);
            if (FixedMath.Abs(sinp) >= Fixed._1)
            {
                angles.y = FixedMath.SetSameSign(Fixed.pi / 2, sinp);
            }
            else
            {
                angles.y = FixedMath.Asin(sinp);
            }

            // z-axis rotation
            Fixed siny_cosp = 2 * (fq.w * fq.z + fq.x * fq.y);
            Fixed cosy_cosp = 1 - 2 * (fq.y * fq.y + fq.z * fq.z);
            angles.z = FixedMath.Atan2(siny_cosp, cosy_cosp);

            return angles;
        }

        // converted from https://en.wikipedia.org/wiki/Conversion_between_quaternions_and_Euler_angles
#if DEBUG
        public
#endif
        static FixedQuaternion Internal_FromEulerRad(Fixed3 euler)
        {
            Fixed cy = FixedMath.Cos(euler.z * Fixed._0_50);
            Fixed sy = FixedMath.Sin(euler.z * Fixed._0_50);
            Fixed cp = FixedMath.Cos(euler.y * Fixed._0_50);
            Fixed sp = FixedMath.Sin(euler.y * Fixed._0_50);
            Fixed cr = FixedMath.Cos(euler.x * Fixed._0_50);
            Fixed sr = FixedMath.Sin(euler.x * Fixed._0_50);

            FixedQuaternion q;
            q.w = cr * cp * cy + sr * sp * sy;
            q.x = sr * cp * cy - cr * sp * sy;
            q.y = cr * sp * cy + sr * cp * sy;
            q.z = cr * cp * sy - sr * sp * cy;

            return q;
        }

        
    }
}
