using VCCSharp.OpCodes.Tests.Model;

namespace VCCSharp.OpCodes.Tests;

public class Tests
{
    private OldCpu _old;
    private NewCpu _new;

    private MemoryTester _memOld;
    private MemoryTester _memNew;

    [SetUp]
    public void Setup()
    {
        var seeds = new Seeds(10);

        _memOld = new MemoryTester(seeds);
        _memNew = new MemoryTester(seeds);

        byte CC = Rnd.B();
        ushort PC = Rnd.W();
        ushort S = Rnd.W();
        byte DP = Rnd.B();
        ushort D = Rnd.W();
        ushort X = Rnd.W();

        _old = new OldCpu(_memOld) { CC = CC, PC_REG = PC, S_REG = S, DPA = DP, D_REG = D, X_REG = X };
        _new = new NewCpu(_memNew) { CC = CC, PC = PC, S = S, DP = DP, D = D, X = X };
    }

    private void Verify()
    {
        Assert.That(_old.CC, Is.EqualTo(_new.CC));
        Assert.That(_old.PC_REG, Is.EqualTo(_new.PC));
        Assert.That(_old.S_REG, Is.EqualTo(_new.S));
        Assert.That(_old.DPA, Is.EqualTo(_new.DP));
        Assert.That(_old.X_REG, Is.EqualTo(_new.X));
        Assert.That(_old.Cycles, Is.EqualTo(_new.Cycles));
    }

    [TestCase(0xCC)]
    [TestCase(0xFC)]
    public void TestOpcode(byte opcode)
    {
        _old.Exec(opcode);
        _new.Exec(opcode);

        Verify();
    }


    //[TestCase(0x19)]
    //[TestCase(0x1A)]
    //[TestCase(0x1D)]
    //[TestCase(0x0E)]
    //[TestCase(0x12)]
    //[TestCase(0x16)]
    //[TestCase(0x17)]
    //[TestCase(0x1C)]
    //[TestCase(0x20)]
    //[TestCase(0x21)]
    //[TestCase(0x22)]
    //[TestCase(0x23)]
    //[TestCase(0x24)]
    //[TestCase(0x25)]
    //[TestCase(0x26)]
    //[TestCase(0x27)]
    //[TestCase(0x28)]
    //[TestCase(0x29)]
    //[TestCase(0x2A)]
    //[TestCase(0x2B)]
    //[TestCase(0x2C)]
    //[TestCase(0x2D)]
    //[TestCase(0x2E)]
    //[TestCase(0x2F)]
    //[TestCase(0x39)]
    //[TestCase(0x3A)]
    //[TestCase(0x3D)]
    //[TestCase(0x40)]
    //[TestCase(0x43)]
    //[TestCase(0x44)]
    //[TestCase(0x46)]
    //[TestCase(0x47)]
    //[TestCase(0x48)]
    //[TestCase(0x49)]
    //[TestCase(0x4A)]
    //[TestCase(0x4C)]
    //[TestCase(0x4D)]
    //[TestCase(0x4F)]
    //[TestCase(0x50)]
    //[TestCase(0x53)]
    //[TestCase(0x54)]
    //[TestCase(0x56)]
    //[TestCase(0x57)]
    //[TestCase(0x58)]
    //[TestCase(0x59)]
    //[TestCase(0x5A)]
    //[TestCase(0x5C)]
    //[TestCase(0x5D)]
    //[TestCase(0x5F)]
    //[TestCase(0x70)]
    //[TestCase(0x73)]
    //[TestCase(0x74)]
    //[TestCase(0x76)]
    //[TestCase(0x77)]
    //[TestCase(0x78)]
    //[TestCase(0x79)]
    //[TestCase(0x7A)]
    //[TestCase(0x7C)]
    //[TestCase(0x7D)]
    //[TestCase(0x7E)]
    //[TestCase(0x7F)]
    //[TestCase(0x80)]
    //[TestCase(0x81)]
    //[TestCase(0x82)]
    //[TestCase(0x83)]
    //[TestCase(0x84)]
    //[TestCase(0x85)]
    //[TestCase(0x86)]
    //[TestCase(0x88)]
    //[TestCase(0x89)]
    //[TestCase(0x8A)]
    //[TestCase(0x8B)]
    //[TestCase(0x8C)]
    //[TestCase(0x8D)]
    //[TestCase(0x8E)]
    //[TestCase(0xB0)]
    //[TestCase(0xB1)]
    //[TestCase(0xB2)]
    //[TestCase(0xB3)]
    //[TestCase(0xB4)]
    //[TestCase(0xB5)]
    //[TestCase(0xB6)]
    //[TestCase(0xB7)]
    //[TestCase(0xB8)]
    //[TestCase(0xB9)]
    //[TestCase(0xBA)]
    //[TestCase(0xBB)]
    //[TestCase(0xBC)]
    //[TestCase(0xBD)]
    //[TestCase(0xBE)]
    //[TestCase(0xBF)]
    //[TestCase(0xC0)]
    //[TestCase(0xC1)]
    //[TestCase(0xC2)]
    //[TestCase(0xC3)]
    //[TestCase(0xC4)]
    //[TestCase(0xC5)]
    //[TestCase(0xC6)]
    //[TestCase(0xC8)]
    //[TestCase(0xC9)]
    //[TestCase(0xCA)]
    //[TestCase(0xCB)]
    //[TestCase(0xCE)]
    //[TestCase(0xF0)]
    //[TestCase(0xF1)]
    //[TestCase(0xF2)]
    //[TestCase(0xF3)]
    //[TestCase(0xF4)]
    //[TestCase(0xF5)]
    //[TestCase(0xF6)]
    //[TestCase(0xF7)]
    //[TestCase(0xF8)]
    //[TestCase(0xF9)]
    //[TestCase(0xFA)]
    //[TestCase(0xFB)]
    //[TestCase(0xFD)]
    //[TestCase(0xFE)]
    //[TestCase(0xFF)]
}