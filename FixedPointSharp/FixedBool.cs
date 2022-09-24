using System.Runtime.InteropServices;

namespace Deterministic.FixedPoint {
    [StructLayout(LayoutKind.Explicit, Size = SIZE)]
    // ReSharper disable once InconsistentNaming
    public struct FixedBool {
        public const int SIZE = 2;
        
        [FieldOffset(0)]
        public ushort Value;

        public static implicit operator bool(FixedBool b)
        {
            return b.Value > 0;
        }

        public static implicit operator FixedBool(bool b)
        {
            FixedBool fbool;
            fbool.Value = (ushort) (b ? 1 : 0);
            return fbool;
        }

        public bool Equals(FixedBool other) {
            return Value == other.Value;
        }

        public override bool Equals(object obj) {
            return obj is FixedBool other && Equals(other);
        }
        
        public static bool operator ==(FixedBool left, FixedBool right) {
            return left.Value == right.Value;
        }

        public static bool operator !=(FixedBool left, FixedBool right) {
            return left.Value != right.Value;
        }

        public override int GetHashCode() {
            return Value;
        }

        public override string ToString() {
            return ((bool) this).ToString();
        }
    }
}