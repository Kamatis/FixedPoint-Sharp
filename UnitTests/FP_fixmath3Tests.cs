using Deterministic.FixedPoint;
using FluentAssertions;
using NUnit.Framework;

namespace UnitTests
{
    public class FP_fixmath3Tests
    {
        [Test]
        public void NormalizationTest()
        {
            var originalVector = new Fixed3(Fixed._5, Fixed._0, Fixed._0);
            var modifiedVector = FixedMath.Normalize(originalVector);

            modifiedVector.Should().Be(new Fixed3(Fixed._1, Fixed._0, Fixed._0));
        }
        
        [Test]
        public void MagnitudeTest()
        {
            var originalVector = new Fixed3(Fixed._5, Fixed._0, Fixed._0);
            var magnitude = FixedMath.Magnitude(originalVector);

            magnitude.Should().Be(Fixed._5);
        }   
        
        [Test]
        public void MagnitudeSqrTest()
        {
            var originalVector = new Fixed3(Fixed._5, Fixed._0, Fixed._0);
            var magnitude      = FixedMath.MagnitudeSqr(originalVector);

            magnitude.Should().Be(Fixed._5 * Fixed._5);
        }

        [Test]
        public void MagnitudeClampTest()
        {
            var originalVector = new Fixed3(Fixed._5, Fixed._0, Fixed._0);
            var clampedVector      = FixedMath.MagnitudeClamp(originalVector, Fixed._1_10);

            clampedVector.x.AsFloat.Should().BeApproximately(1.10f, 0.01f);
            clampedVector.y.AsFloat.Should().Be(0f);
            clampedVector.z.AsFloat.Should().Be(0f);
        }
        
        [Test]
        public void DotTest()
        {
            var vector1 = new Fixed3(Fixed._5, Fixed._0, Fixed._0);
            var vector2 = new Fixed3(Fixed._5, Fixed._0, Fixed._0);
            var dot = FixedMath.Dot(vector1, vector2);

            dot.Should().Be(Fixed._5 * Fixed._5);
            
            vector1 = new Fixed3(Fixed._1, Fixed._5, Fixed._4);
            vector2 = new Fixed3(Fixed._2, Fixed._0, Fixed._1);
            dot     = FixedMath.Dot(vector1, vector2);

            dot.Should().Be(Fixed._6);
            
            vector1 = new Fixed3(Fixed._0_10, Fixed._0_75,   Fixed._0_10);
            vector2 = new Fixed3(Fixed._0_50+Fixed._0_10, Fixed._0_20, Fixed._0_33);
            dot     = FixedMath.Dot(vector1, vector2);

            var str = $"{vector1.x.AsFloat},{vector1.y.value},{vector1.z.value} | {vector2} {dot}";

            dot.AsFloat.Should().BeApproximately(0.243f, 0.01f);
        }

        [Test]
        public void AngleTest()
        {
            var vector1 = new Fixed3(Fixed._1, Fixed._5, Fixed._4);
            var vector2 = new Fixed3(Fixed._2, Fixed._0, Fixed._1);
            var angle     = FixedMath.Angle(vector1, vector2);

            angle.AsInt.Should().Be(65);
            
            vector1 = new Fixed3(Fixed._2, Fixed._1,   Fixed._1);
            vector2 = new Fixed3(Fixed._2, Fixed._0, Fixed._1);
            angle = FixedMath.Angle(vector1, vector2);

            angle.AsInt.Should().Be(24);
        }

        [Test]
        public void AngleSignedTest()
        {
            var vector1 = new Fixed3(Fixed._1, Fixed._5,   Fixed._4);
            var vector2 = new Fixed3(Fixed._2, Fixed._0, Fixed._1);
            var angle   = FixedMath.AngleSigned(vector1, vector2, Fixed3.up);

            angle.AsInt.Should().Be(65);
            
            vector1 = new Fixed3(-Fixed._2, Fixed._1,   Fixed._1);
            vector2 = new Fixed3(Fixed._2, Fixed._1, Fixed._1);
            angle   = FixedMath.AngleSigned(vector1, vector2, Fixed3.up);

            angle.AsFloat.Should().BeApproximately(109.47f, 0.1f);
        }
        
        [Test]
        public void RadiansTest()
        {
            var vector1 = new Fixed3(Fixed._1, Fixed._5,   Fixed._4);
            var vector2 = new Fixed3(Fixed._2, Fixed._0, Fixed._1);
            var angle   = FixedMath.Radians(vector1, vector2);

            angle.AsInt.Should().Be(1);
            
            vector1 = new Fixed3(Fixed._2, Fixed._1,   Fixed._1);
            vector2 = new Fixed3(Fixed._2, Fixed._0, Fixed._1);
            angle   = FixedMath.Radians(vector1, vector2);
            
            angle.AsFloat.Should().BeApproximately(0.42f, 0.01f);
        }

        [Test]
        public void CrossTest()
        {
            var vector1 = new Fixed3(Fixed._1, Fixed._5,   Fixed._4);
            var vector2 = new Fixed3(Fixed._2, Fixed._0, Fixed._1);
            var cross   = FixedMath.Cross(vector1, vector2);

            cross.Should().Be(new Fixed3(Fixed._5, Fixed._7, -Fixed._10));
        }

        [Test]
        public void ReflectTest()
        {
            var vector     = new Fixed3(Fixed._5,  Fixed._0,   Fixed._5);
            var normal     = new Fixed3(-Fixed._1, Fixed._0, Fixed._0);
            var reflection = FixedMath.Reflect(vector, normal);

            reflection.Should().Be(new Fixed3(-Fixed._5, Fixed._0, Fixed._5));
        }
        
        [Test]
        public void ProjectTest()
        {
            var vector     = new Fixed3(Fixed._5,  Fixed._0, Fixed._5);
            var normal     = new Fixed3(-Fixed._1, Fixed._0, Fixed._0);
            var projection = FixedMath.Project(vector, normal);

            projection.Should().Be(new Fixed3(Fixed._5, Fixed._0, Fixed._0));
        }

        [Test]
        public void ProjectOnPlaneTest()
        {
            var vector     = new Fixed3(Fixed._5,  Fixed._1, Fixed._5);
            var normal     = new Fixed3(-Fixed._1, Fixed._0, Fixed._0);
            var projection = FixedMath.ProjectOnPlane(vector, normal);

            projection.Should().Be(new Fixed3(Fixed._0, Fixed._1, Fixed._5));
        }

        [Test]
        public void LerpTest()
        {
            var @from     = new Fixed3(Fixed._5,  Fixed._0,   Fixed._5);
            var to     = new Fixed3(Fixed._0, Fixed._0, Fixed._0);
            var lerped = FixedMath.Lerp(@from, to, Fixed._0_50);

            lerped.Should().Be(new Fixed3(Fixed._2+Fixed._0_50, Fixed._0, Fixed._2 +Fixed._0_50));
        }

        [Test]
        public void MoveTowardsTest()
        {
            var current = Fixed3.one;
            var target = new Fixed3(Fixed._5, Fixed._1, Fixed._1);

            var step1 = FixedMath.MoveTowards(current, target, Fixed._1);
            step1.Should().Be(new Fixed3(Fixed._2, Fixed._1, Fixed._1));
            
            var step2 = FixedMath.MoveTowards(current, target, Fixed._10);
            step2.Should().Be(new Fixed3(Fixed._5, Fixed._1, Fixed._1));
        }
    }
}