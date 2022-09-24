using System;
using Deterministic;
using Deterministic.FixedPoint;
using FluentAssertions;
using NUnit.Framework;

namespace UnitTests {
    public class FP_fixmathTests {

        [Test]
        public void CountLeadingZerosTest() {
            FixedMath.CountLeadingZeroes(5435345).Should().Be(9);
            FixedMath.CountLeadingZeroes(4).Should().Be(29);
        }
        
        [Test]
        public void ExpTest() {
            var from  = -5f;
            var to    = 5f;
            var delta = 0.001f;

            for (float v = from; v < to; v += delta) {
                var correctAnswer = (float)Math.Exp(v);
                var parsedFp      = Fixed.ParseUnsafe(v);
                var answer        = FixedMath.Exp(parsedFp);
                answer.AsFloat.Should().BeApproximately(correctAnswer, 0.01f);
            }
            
            from  = 5f;
            to    = 5.33f;
            delta = 0.001f;

            for (float v = from; v < to; v += delta) {
                var correctAnswer = (float)Math.Exp(v);
                var parsedFp      = Fixed.ParseUnsafe(v);
                var answer        = FixedMath.Exp(parsedFp);
                answer.AsFloat.Should().BeApproximately(correctAnswer, 1f);
            }
        }

        [Test]
        public void Atan_2Test() {
            var from  = -1f;
            var to    = 1f;
            var delta = 0.001f;

            for (float v = from; v < to; v += delta) {
                var correctAnswer = (float)Math.Atan(v);
                var parsedFp      = Fixed.ParseUnsafe(v);
                var answer        = FixedMath.AtanApproximated(parsedFp);
                answer.AsFloat.Should().BeApproximately(correctAnswer, 0.01f);
            }
        }

        [Test]
        public void AtanTest() {
            var from  = -1f;
            var to    = 1f;
            var delta = 0.001f;

            for (float v = from; v < to; v += delta) {
                var correctAnswer = (float)Math.Atan(v);
                var parsedFp      = Fixed.ParseUnsafe(v);
                var answer        = FixedMath.Atan(parsedFp);
                answer.AsFloat.Should().BeApproximately(correctAnswer, 0.001f);
            }
        }

        [Test]
        public void Atan2Test() {
            var from1 = 0.1f;
            var to1   = 10f;
            var from2 = 0.1f;
            var to2   = 10f;
            var delta = 0.01f;

            for (float x = from1; x < to1; x += delta) {
                for (float y = from2; y < to2; y += delta) {
                    var correctAnswer = (float) Math.Atan2(x, y);
                    var parsedFp1 = Fixed.ParseUnsafe(x);
                    var parsedFp2 = Fixed.ParseUnsafe(y);
                    var answer = FixedMath.Atan2(parsedFp1, parsedFp2);
                    answer.AsFloat.Should().BeApproximately(correctAnswer, 0.01f);
                }
            }
        }

        [Test]
        public void TanTest() {
            for (long i = Fixed.minus_one.value; i <= Fixed._1.value; i++) {
                Fixed val;
                val.value = i;
                var dValue = val.AsDouble;
                var answer = FixedMath.Tan(val);
                answer.AsDouble.Should().BeApproximately(Math.Tan(dValue), 0.001d);
            }
        }

        [Test]
        public void AcosTest() {
            for (long i = Fixed.minus_one.value; i <= Fixed._1.value; i++) {
                Fixed val;
                val.value = i;
                var dValue = val.AsDouble;
                var answer = FixedMath.Acos(val);
                answer.AsDouble.Should().BeApproximately(Math.Acos(dValue), 0.0001d);
            }
        }

        [Test]
        public void AsinTest() {
            for (long i = Fixed.minus_one.value; i <= Fixed._1.value; i++) {
                Fixed val;
                val.value = i;
                var dValue = val.AsDouble;
                var answer = FixedMath.Asin(val);
                answer.AsDouble.Should().BeApproximately(Math.Asin(dValue), 0.0001d);
            }
        }

        [Test]
        public void CosTest() {
            for (long i = -Fixed.pi.value*2; i <= Fixed.pi.value*2; i++) {
                Fixed val;
                val.value = i;
                var dValue = val.AsDouble;
                var answer = FixedMath.Cos(val);
                answer.AsDouble.Should().BeApproximately(Math.Cos(dValue), 0.001d);
            }
        }

        [Test]
        public void SinTest() {
            for (long i = -Fixed.pi.value*2; i <= Fixed.pi.value*2; i++) {
                Fixed val;
                val.value = i;
                var dValue = val.AsDouble;
                var answer = FixedMath.Sin(val);
                answer.AsDouble.Should().BeApproximately(Math.Sin(dValue), 0.001d);
            }
        }

        [Test]
        public void SinCosTest() {
            for (long i = -Fixed.pi.value*2; i <= Fixed.pi.value*2; i++) {
                Fixed val;
                val.value = i;
                var dValue = val.AsDouble;
                
                FixedMath.SinCos(val, out var sin, out var cos);
                sin.AsDouble.Should().BeApproximately(Math.Sin(dValue), 0.001d);
                cos.AsDouble.Should().BeApproximately(Math.Cos(dValue), 0.001d);
            }
        }

        [Test]
        public void RcpTest() {
            var value = Fixed._0_25;
            var result = FixedMath.Rcp(value);
            result.Should().Be(Fixed._4);
            
            value = Fixed._4;
            result = FixedMath.Rcp(value);
            result.Should().Be(Fixed._0_25);
        }

        [Test]
        public void SqrtTest() {
            var from  = 0.1f;
            var to    = 65000;
            var delta = 0.1f;

            for (float v = from; v < to; v += delta) {
                var correct = (float)Math.Sqrt(v);
                var parsedFp   = Fixed.ParseUnsafe(v);
                var answer = FixedMath.Sqrt(parsedFp);
                answer.AsFloat.Should().BeApproximately(correct, 0.01f);
            }
        }

        [Test]
        public void FloorTest() {
            var value  = Fixed._0_25;
            var result = FixedMath.Floor(value);
            result.Should().Be(Fixed._0);

            result = FixedMath.Floor(-value);
            result.Should().Be(-Fixed._1);
        }

        [Test]
        public void CeilTest() {
            var value  = Fixed._0_25;
            var result = FixedMath.Ceil(value);
            result.Should().Be(Fixed._1);

            result = FixedMath.Ceil(-Fixed._4 - Fixed._0_25);
            result.Should().Be(-Fixed._4);
        }

        [Test]
        public void RoundToIntTest() {
            var value  = Fixed._5 + Fixed._0_25;
            var result = FixedMath.RoundToInt(value);
            result.Should().Be(5);

            result = FixedMath.RoundToInt(value + Fixed._0_33);
            result.Should().Be(6);

            result = FixedMath.RoundToInt(value + Fixed._0_25);
            result.Should().Be(6);
        }

        [Test]
        public void MinTest() {
            var value1 = Fixed._0_25;
            var value2 = Fixed._0_33;
            var result = FixedMath.Min(value1, value2);
            result.Should().Be(value1);

            result = FixedMath.Min(-value1, -value2);
            result.Should().Be(-value2);
        }

        [Test]
        public void MaxTest() {
            var value1 = Fixed._0_25;
            var value2 = Fixed._0_33;
            var result = FixedMath.Max(value1, value2);
            result.Should().Be(value2);

            result = FixedMath.Max(-value1, -value2);
            result.Should().Be(-value1);
        }

        [Test]
        public void AbsTest() {
            var from  = -5;
            var to    = 5;
            var delta = 0.1f;

            for (float v = from; v < to; v += delta) {
                var correctAnswer = Math.Abs(v);
                var parsedFp           = Fixed.ParseUnsafe(v);
                var answer = FixedMath.Abs(parsedFp);
                
                answer.AsFloat.Should().BeApproximately(correctAnswer, 0.1f);
            }
        }

        [Test]
        public void ClampTest() {
            var from  = -5;
            var to    = 5;
            var delta = 0.1f;

            for (float v = from; v < to; v += delta) {
                var correctAnswer = Math.Clamp(v, -3, 3);
                var parsedFp      = Fixed.ParseUnsafe(v);
                var answer = FixedMath.Clamp(parsedFp, -Fixed._3, Fixed._3);
                answer.AsFloat.Should().BeApproximately(correctAnswer, 0.1f);
            }
        }


        [Test]
        public void LerpTest() {
            var result = FixedMath.Lerp(Fixed._2, Fixed._4, Fixed._0_25);
            result.Should().Be(Fixed._2 + Fixed._0_50);

            result = FixedMath.Lerp(Fixed._2, Fixed._4, Fixed._0);
            result.Should().Be(Fixed._2);

            result = FixedMath.Lerp(Fixed._2, Fixed._4, Fixed._1);
            result.Should().Be(Fixed._4);

            result = FixedMath.Lerp(Fixed._2, Fixed._4, Fixed._0_50);
            result.Should().Be(Fixed._3);
        }

        [Test]
        public void MulTest() {
            Fixed a = 5;

            var result1 = a * Fixed._0_01;
            result1.AsFloat.Should().BeApproximately(0.05f, 0.01f);
            
            var result2 = Fixed._0_01 * a;
            result2.AsFloat.Should().BeApproximately(0.05f, 0.01f);

            var result3 = Fixed._0_01 * Fixed._0_01;
            result3.AsFloat.Should().BeApproximately(0.001f, 0.002f);
        }

        [Test]
        public void SignTest() {
            var from  = -5;
            var to    = 5;
            var delta = 0.12f;

            for (float v = from; v < to; v += delta) {
                var correctAnswer = Math.Sign(v);
                var parsedFp      = Fixed.ParseUnsafe(v);
                var answer        = FixedMath.Sign(parsedFp);
                answer.AsFloat.Should().BeApproximately(correctAnswer, 0.1f);
            }
        }

        [Test]
        public void IsOppositeSignTest() {
            var result = FixedMath.IsOppositeSign(Fixed._0_25, -Fixed._0_20);
            result.Should().Be(true);

            result = FixedMath.IsOppositeSign(Fixed._0_25, Fixed._0_20);
            result.Should().Be(false);

            result = FixedMath.IsOppositeSign(-Fixed._0_25, -Fixed._0_20);
            result.Should().Be(false);
        }
    }
}