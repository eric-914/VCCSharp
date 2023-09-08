using VCCSharp.OpCodes.Tests.Model;

namespace VCCSharp.OpCodes.Tests;

public class Tests
{
    [TestCase(0x00)]
    [TestCase(0x03)]
    [TestCase(0x04)]
    [TestCase(0x06)]
    [TestCase(0x07)]
    [TestCase(0x08)]
    [TestCase(0x09)]
    [TestCase(0x0A)]
    [TestCase(0x0C)]
    [TestCase(0x0D)]
    [TestCase(0x0E)]
    [TestCase(0x0F)]
    [TestCase(0x12)]
    [TestCase(0x16)]
    [TestCase(0x17)]
    [TestCase(0x19)]
    [TestCase(0x1A)]
    [TestCase(0x1C)]
    [TestCase(0x1D)]
    [TestCase(0x20)]
    [TestCase(0x21)]
    [TestCase(0x22)]
    [TestCase(0x23)]
    [TestCase(0x24)]
    [TestCase(0x25)]
    [TestCase(0x26)]
    [TestCase(0x27)]
    [TestCase(0x28)]
    [TestCase(0x29)]
    [TestCase(0x2A)]
    [TestCase(0x2B)]
    [TestCase(0x2C)]
    [TestCase(0x2D)]
    [TestCase(0x2E)]
    [TestCase(0x2F)]
    [TestCase(0x30)]
    [TestCase(0x31)]
    [TestCase(0x32)]
    [TestCase(0x33)]
    [TestCase(0x34)]
    [TestCase(0x35)]
    [TestCase(0x36)]
    [TestCase(0x37)]
    [TestCase(0x39)]
    [TestCase(0x3A)]
    [TestCase(0x3B)]
    [TestCase(0x3C)]
    [TestCase(0x3D)]
    [TestCase(0x3F)]
    [TestCase(0x40)]
    [TestCase(0x43)]
    [TestCase(0x44)]
    [TestCase(0x46)]
    [TestCase(0x47)]
    [TestCase(0x48)]
    [TestCase(0x49)]
    [TestCase(0x4A)]
    [TestCase(0x4C)]
    [TestCase(0x4D)]
    [TestCase(0x4F)]
    [TestCase(0x50)]
    [TestCase(0x53)]
    [TestCase(0x54)]
    [TestCase(0x56)]
    [TestCase(0x57)]
    [TestCase(0x58)]
    [TestCase(0x59)]
    [TestCase(0x5A)]
    [TestCase(0x5C)]
    [TestCase(0x5D)]
    [TestCase(0x5F)]
    [TestCase(0x60)]
    [TestCase(0x63)]
    [TestCase(0x64)]
    [TestCase(0x66)]
    [TestCase(0x67)]
    [TestCase(0x68)]
    [TestCase(0x69)]
    [TestCase(0x6A)]
    [TestCase(0x6C)]
    [TestCase(0x6D)]
    [TestCase(0x6E)]
    [TestCase(0x6F)]
    [TestCase(0x70)]
    [TestCase(0x73)]
    [TestCase(0x74)]
    [TestCase(0x76)]
    [TestCase(0x77)]
    [TestCase(0x78)]
    [TestCase(0x79)]
    [TestCase(0x7A)]
    [TestCase(0x7C)]
    [TestCase(0x7D)]
    [TestCase(0x7E)]
    [TestCase(0x7F)]
    [TestCase(0x80)]
    [TestCase(0x81)]
    [TestCase(0x82)]
    [TestCase(0x83)]
    [TestCase(0x84)]
    [TestCase(0x85)]
    [TestCase(0x86)]
    [TestCase(0x88)]
    [TestCase(0x89)]
    [TestCase(0x8A)]
    [TestCase(0x8B)]
    [TestCase(0x8C)]
    [TestCase(0x8D)]
    [TestCase(0x8E)]
    [TestCase(0x90)]
    [TestCase(0x91)]
    [TestCase(0x92)]
    [TestCase(0x93)]
    [TestCase(0x94)]
    [TestCase(0x95)]
    [TestCase(0x96)]
    [TestCase(0x97)]
    [TestCase(0x98)]
    [TestCase(0x99)]
    [TestCase(0x9A)]
    [TestCase(0x9B)]
    [TestCase(0x9C)]
    [TestCase(0x9D)]
    [TestCase(0x9E)]
    [TestCase(0x9F)]
    [TestCase(0xA0)]
    [TestCase(0xA1)]
    [TestCase(0xA2)]
    [TestCase(0xA3)]
    [TestCase(0xA4)]
    [TestCase(0xA5)]
    [TestCase(0xA6)]
    [TestCase(0xA7)]
    [TestCase(0xA8)]
    [TestCase(0xA9)]
    [TestCase(0xAA)]
    [TestCase(0xAB)]
    [TestCase(0xAC)]
    [TestCase(0xAD)]
    [TestCase(0xAE)]
    [TestCase(0xAF)]
    [TestCase(0xB0)]
    [TestCase(0xB1)]
    [TestCase(0xB2)]
    [TestCase(0xB3)]
    [TestCase(0xB4)]
    [TestCase(0xB5)]
    [TestCase(0xB6)]
    [TestCase(0xB7)]
    [TestCase(0xB8)]
    [TestCase(0xB9)]
    [TestCase(0xBA)]
    [TestCase(0xBB)]
    [TestCase(0xBC)]
    [TestCase(0xBD)]
    [TestCase(0xBE)]
    [TestCase(0xBF)]
    [TestCase(0xC0)]
    [TestCase(0xC1)]
    [TestCase(0xC2)]
    [TestCase(0xC3)]
    [TestCase(0xC4)]
    [TestCase(0xC5)]
    [TestCase(0xC6)]
    [TestCase(0xC8)]
    [TestCase(0xC9)]
    [TestCase(0xCA)]
    [TestCase(0xCB)]
    [TestCase(0xCC)]
    [TestCase(0xCE)]
    [TestCase(0xD0)]
    [TestCase(0xD1)]
    [TestCase(0xD2)]
    [TestCase(0xD3)]
    [TestCase(0xD4)]
    [TestCase(0xD5)]
    [TestCase(0xD6)]
    [TestCase(0xD7)]
    [TestCase(0xD8)]
    [TestCase(0xD9)]
    [TestCase(0xDA)]
    [TestCase(0xDB)]
    [TestCase(0xDC)]
    [TestCase(0xDD)]
    [TestCase(0xDE)]
    [TestCase(0xDF)]
    [TestCase(0xE0)]
    [TestCase(0xE1)]
    [TestCase(0xE2)]
    [TestCase(0xE3)]
    [TestCase(0xE4)]
    [TestCase(0xE5)]
    [TestCase(0xE6)]
    [TestCase(0xE7)]
    [TestCase(0xE8)]
    [TestCase(0xE9)]
    [TestCase(0xEA)]
    [TestCase(0xEB)]
    [TestCase(0xEC)]
    [TestCase(0xED)]
    [TestCase(0xEE)]
    [TestCase(0xEF)]
    [TestCase(0xF0)]
    [TestCase(0xF1)]
    [TestCase(0xF2)]
    [TestCase(0xF3)]
    [TestCase(0xF4)]
    [TestCase(0xF5)]
    [TestCase(0xF6)]
    [TestCase(0xF7)]
    [TestCase(0xF8)]
    [TestCase(0xF9)]
    [TestCase(0xFA)]
    [TestCase(0xFB)]
    [TestCase(0xFC)]
    [TestCase(0xFD)]
    [TestCase(0xFE)]
    [TestCase(0xFF)]
    public void TestOpcode(byte opcode)
    {
        for (int i = 0; i < 20; i++)
        {
            var seeds = new Seeds(20);
            var state = new TestState();

            var memOld = new MemoryTester(seeds);
            var memNew = new MemoryTester(seeds);

            var _old = new OldCpu(memOld) { CC = state.CC, PC_REG = state.PC, S_REG = state.S, U_REG = state.U, DPA = state.DP, D_REG = state.D, X_REG = state.X, Y_REG = state.Y };
            var _new = new NewCpu(memNew) { CC = state.CC, PC = state.PC, S = state.S, U = state.U, DP = state.DP, D = state.D, X = state.X, Y = state.Y };

            _old.Exec(opcode);
            _new.Exec(opcode);

            string message = $@"
byte opcode=0x{opcode:x}; 
var seeds = new Seeds {{{string.Join(", ", seeds)}}};
var state = new TestState {{ CC=0x{state.CC:x}, PC=0x{state.PC:x}, S=0x{state.S:x}, U=0x{state.U:x}, DP=0x{state.DP:x}, D=0x{state.D:x}, X=0x{state.X:x}, Y=0x{state.Y:x} }};
";

            Assert.That(_old.CC, Is.EqualTo(_new.CC), message);
            Assert.That(_old.PC_REG, Is.EqualTo(_new.PC), message);
            Assert.That(_old.S_REG, Is.EqualTo(_new.S), message);
            Assert.That(_old.U_REG, Is.EqualTo(_new.U), message);
            Assert.That(_old.DPA, Is.EqualTo(_new.DP), message);
            Assert.That(_old.X_REG, Is.EqualTo(_new.X), message);
            Assert.That(_old.Y_REG, Is.EqualTo(_new.Y), message);
            Assert.That(_old.Cycles, Is.EqualTo(_new.Cycles), message);
        }
    }

    [Test]
    public void OneOffTest()
    {
        byte opcode = 0xe2;
        var seeds = new Seeds { 214, 167, 7, 180, 194, 78, 130, 11, 4, 213, 96, 103, 1, 30, 50, 161, 79, 101, 143, 194 };
        var state = new TestState { CC = 0x65, PC = 0x99fc, S = 0x2406, U = 0x1f5e, DP = 0x64, D = 0x2534, X = 0xa27a, Y = 0xeaee };

        var memOld = new MemoryTester(seeds);
        var memNew = new MemoryTester(seeds);

        var _old = new OldCpu(memOld) { CC = state.CC, PC_REG = state.PC, S_REG = state.S, U_REG = state.U, DPA = state.DP, D_REG = state.D, X_REG = state.X, Y_REG = state.Y };
        var _new = new NewCpu(memNew) { CC = state.CC, PC = state.PC, S = state.S, U = state.U, DP = state.DP, D = state.D, X = state.X, Y = state.Y };

        _old.Exec(opcode);
        _new.Exec(opcode);

        Assert.That(_old.CC, Is.EqualTo(_new.CC));
        Assert.That(_old.PC_REG, Is.EqualTo(_new.PC));
        Assert.That(_old.S_REG, Is.EqualTo(_new.S));
        Assert.That(_old.U_REG, Is.EqualTo(_new.U));
        Assert.That(_old.DPA, Is.EqualTo(_new.DP));
        Assert.That(_old.X_REG, Is.EqualTo(_new.X));
        Assert.That(_old.Y_REG, Is.EqualTo(_new.Y));
        Assert.That(_old.Cycles, Is.EqualTo(_new.Cycles));
    }
}