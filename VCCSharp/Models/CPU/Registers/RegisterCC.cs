using VCCSharp.Enums;

namespace VCCSharp.Models.CPU.Registers
{
    /// <summary>
    /// CONDITION CODE REGISTER (CC)
    /// </summary>
    internal class RegisterCC
    {
        private readonly byte _E = 1 << (byte)CCFlagMasks.E;
        private readonly byte _F = 1 << (byte)CCFlagMasks.F;
        private readonly byte _H = 1 << (byte)CCFlagMasks.H;
        private readonly byte _I = 1 << (byte)CCFlagMasks.I;
        private readonly byte _N = 1 << (byte)CCFlagMasks.N;
        private readonly byte _Z = 1 << (byte)CCFlagMasks.Z;
        private readonly byte _V = 1 << (byte)CCFlagMasks.V;
        private readonly byte _C = 1 << (byte)CCFlagMasks.C;

        internal byte bits { get; set; }

        internal bool E { get { return Get(_E); } set { Set(_E, value); } }
        internal bool F { get { return Get(_F); } set { Set(_F, value); } }
        internal bool H { get { return Get(_H); } set { Set(_H, value); } }
        internal bool I { get { return Get(_I); } set { Set(_I, value); } }
        internal bool N { get { return Get(_N); } set { Set(_N, value); } }
        internal bool Z { get { return Get(_Z); } set { Set(_Z, value); } }
        internal bool V { get { return Get(_V); } set { Set(_V, value); } }
        internal bool C { get { return Get(_C); } set { Set(_C, value); } }

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
