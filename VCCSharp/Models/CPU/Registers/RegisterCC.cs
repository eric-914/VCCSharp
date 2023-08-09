using VCCSharp.Enums;

namespace VCCSharp.Models.CPU.Registers
{
    /// <summary>
    /// CONDITION CODE REGISTER (CC)
    /// </summary>
    public class RegisterCC
    {
        private readonly byte _E = 1 << (byte)CCFlagMasks.E;
        private readonly byte _F = 1 << (byte)CCFlagMasks.F;
        private readonly byte _H = 1 << (byte)CCFlagMasks.H;
        private readonly byte _I = 1 << (byte)CCFlagMasks.I;
        private readonly byte _N = 1 << (byte)CCFlagMasks.N;
        private readonly byte _Z = 1 << (byte)CCFlagMasks.Z;
        private readonly byte _V = 1 << (byte)CCFlagMasks.V;
        private readonly byte _C = 1 << (byte)CCFlagMasks.C;

        public byte bits;

        public bool E { get { return Get(_E); } set { Set(_E, value); } }
        public bool F { get { return Get(_F); } set { Set(_F, value); } }
        public bool H { get { return Get(_H); } set { Set(_H, value); } }
        public bool I { get { return Get(_I); } set { Set(_I, value); } }
        public bool N { get { return Get(_N); } set { Set(_N, value); } }
        public bool Z { get { return Get(_Z); } set { Set(_Z, value); } }
        public bool V { get { return Get(_V); } set { Set(_V, value); } }
        public bool C { get { return Get(_C); } set { Set(_C, value); } }

        private bool Get(byte mask)
        {
            return (bits & mask) != 0;
        }

        private void Set(byte mask, bool value)
        {
            if (value)
                bits |= mask;
            else
                bits &= (byte)~mask;
        }
    }
}
