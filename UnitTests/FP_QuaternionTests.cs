using Deterministic.FixedPoint;
using FluentAssertions;
using NUnit.Framework;

namespace UnitTests
{
    class FP_QuaternionTests
    {
        [Test]
        public void AngleTest()
        {
            // (x,y,z) = (25,50,95)
            var q1 = new FixedQuaternion()
            {
                x = Fixed.ParseUnsafe(0.43673f),
                y = Fixed.ParseUnsafe(0.13412f),
                z = Fixed.ParseUnsafe(0.59056f),
                w = Fixed.ParseUnsafe(0.66522f)
            };

            //(x,y,z) = (180,90,35)
            var q2 = new FixedQuaternion()
            {
                x = Fixed.ParseUnsafe(0.67438f),
                y = Fixed.ParseUnsafe(-0.21263f),
                z = Fixed.ParseUnsafe(-0.67438f),
                w = Fixed.ParseUnsafe(0.21263f)
            };

            var a = FixedQuaternion.Angle(q1, q2);
            a.AsFloat.Should().BeApproximately(178.9479f, 0.01f);
        }

        [Test]
        public void DotTest()
        {
            // (x,y,z) = (25,50,95)
            var q1 = new FixedQuaternion()
            {
                x = Fixed.ParseUnsafe(0.43673f),
                y = Fixed.ParseUnsafe(0.13412f),
                z = Fixed.ParseUnsafe(0.59056f),
                w = Fixed.ParseUnsafe(0.66522f)
            };

            //(x,y,z) = (180,90,35)
            var q2 = new FixedQuaternion()
            {
                x = Fixed.ParseUnsafe(0.67438f),
                y = Fixed.ParseUnsafe(-0.21263f),
                z = Fixed.ParseUnsafe(-0.67438f),
                w = Fixed.ParseUnsafe(0.21263f)
            };

            var d = FixedQuaternion.Dot(q1, q2);
            d.AsFloat.Should().BeApproximately(0.009181505f, 0.001f);
        }

        [Test]
        public void NormalizeTest()
        {
            var q = new FixedQuaternion()
            {
                x = Fixed.ParseUnsafe(0.4082179f),
                y = Fixed.ParseUnsafe(-0.2345697f),
                z = Fixed.ParseUnsafe(0.1093816f),
                w = Fixed.ParseUnsafe(0.8754261f)
            };

            var nq = FixedQuaternion.Normalize(q);

            nq.x.AsFloat.Should().BeApproximately(0.40822f, 0.001f);
            nq.y.AsFloat.Should().BeApproximately(-0.23457f, 0.001f);
            nq.z.AsFloat.Should().BeApproximately(0.10938f, 0.001f);
            nq.w.AsFloat.Should().BeApproximately(0.87543f, 0.001f);
        }

        [Test]
        public void Internal_MakePositiveTest()
        {
            Fixed3 e = new Fixed3()
            {
                x = Fixed.Parse(360),
                y = Fixed.Parse(-30),
                z = Fixed.Parse(-360)
            };

            var positive = FixedQuaternion.Internal_MakePositive(e);
            positive.x.AsFloat.Should().BeApproximately(0f, 0.01f);
            positive.y.AsFloat.Should().BeApproximately(330f, 0.01f);
            positive.z.AsFloat.Should().BeApproximately(0f, 0.01f);
        }

        /// <summary>
        /// Converted to degrees so that we can check clearly
        /// </summary>
        [Test]
        public void Internal_ToEulerRadTest()
        {
            var fifty = Fixed.Parse(50);
            var threethirty = Fixed.Parse(330);

            var q = new FixedQuaternion()
            {
                x = Fixed.ParseUnsafe(0.4082179f),
                y = Fixed.ParseUnsafe(-0.2345697f),
                z = Fixed.ParseUnsafe(0.1093816f),
                w = Fixed.ParseUnsafe(0.8754261f)
            };

            var e = FixedQuaternion.Internal_ToEulerRad(q) * Fixed.rad2deg;
            e.x.AsFloat.Should().BeApproximately(50f, 0.01f);
            e.y.AsFloat.Should().BeApproximately(-30f, 0.01f);
            e.z.AsFloat.Should().BeApproximately(0f, 0.01f);
        }

        /// <summary>
        /// Converted to radians so that we can check from the previous test
        /// </summary>
        [Test]
        public void Internal_FromEulerRadTest()
        {
            var e = new Fixed3(Fixed.Parse(50), Fixed.Parse(-30), Fixed._0) * Fixed.deg2rad;
            var q = FixedQuaternion.Internal_FromEulerRad(e);

            q.x.AsFloat.Should().BeApproximately(0.4082179f, 0.001f);
            q.y.AsFloat.Should().BeApproximately(-0.2345697f, 0.001f);
            q.z.AsFloat.Should().BeApproximately(0.1093816f, 0.001f);
            q.w.AsFloat.Should().BeApproximately(0.8754261f, 0.001f);
        }
    }
}
