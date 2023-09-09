using VCCSharp.OpCodes.Tests.Model;
using VCCSharp.OpCodes.Tests.Model.MC6809;
using VCCSharp.OpCodes.Tests.Model.MC6809.O;
using VCCSharp.OpCodes.Tests.Model.MC6809.N;

namespace VCCSharp.OpCodes.Tests;

public class MC6809Tests
{
    const int iterations = 100;

    #region TestOpCode 
    private void TestOpCode(byte opcode, Action<byte, OldOpcodes, NewOpcodes> exec)
    {
        var seeds = new Seeds(20);
        var state = new TestState();

        var memOld = new MemoryTester(seeds);
        var memNew = new MemoryTester(seeds);

        var _old = new OldOpcodes(memOld) { CC = state.CC, PC_REG = state.PC, S_REG = state.S, U_REG = state.U, DPA = state.DP, D_REG = state.D, X_REG = state.X, Y_REG = state.Y };
        var _new = new NewOpcodes(memNew) { CC = state.CC, PC = state.PC, S = state.S, U = state.U, DP = state.DP, D = state.D, X = state.X, Y = state.Y };

        exec(opcode, _old, _new);

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
    #endregion

    [Test]
    public void TestPage1Opcodes()
    {
        #region Not Tested
        // 0x10/PAGE2 -- Overridden with separate tests
        // 0x11/PAGE3 -- Overridden with separate tests
        // 0x1E/EXG -- Original code is missing functionality, so doesn't match new code
        // 0x1F/TFR -- Original code is missing functionality, so doesn't match new code
        #endregion

        #region More testing required -- Uses interrupts
        // 0x13/SYNC -- IsSyncWaiting/SyncCycle
        // 0x3B/RTI -- IsInInterrupt
        // 0x3C/CWAI -- IsSyncWaiting/SyncCycle
        #endregion

        //--Currently tested
        var opcodes = new byte[] {
            0x00, 0x03, 0x04, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0C, 0x0D, 0x0E, 0x0F,
            0x12, 0x13, 0x16, 0x17, 0x19, 0x1A, 0x1C, 0x1D,
            0x20, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29, 0x2A, 0x2B, 0x2C, 0x2D, 0x2E, 0x2F,
            0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x3F,
            0x40, 0x43, 0x44, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4C, 0x4D, 0x4F,
            0x50, 0x53, 0x54, 0x56, 0x57, 0x58, 0x59, 0x5A, 0x5C, 0x5D, 0x5F,
            0x60, 0x63, 0x64, 0x66, 0x67, 0x68, 0x69, 0x6A, 0x6C, 0x6D, 0x6E, 0x6F,
            0x70, 0x73, 0x74, 0x76, 0x77, 0x78, 0x79, 0x7A, 0x7C, 0x7D, 0x7E, 0x7F,
            0x80, 0x81, 0x82, 0x83, 0x84, 0x85, 0x86, 0x88, 0x89, 0x8A, 0x8B, 0x8C, 0x8D, 0x8E,
            0x90, 0x91, 0x92, 0x93, 0x94, 0x95, 0x96, 0x97, 0x98, 0x99, 0x9A, 0x9B, 0x9C, 0x9D, 0x9E, 0x9F,
            0xA0, 0xA1, 0xA2, 0xA3, 0xA4, 0xA5, 0xA6, 0xA7, 0xA8, 0xA9, 0xAA, 0xAB, 0xAC, 0xAD, 0xAE, 0xAF,
            0xB0, 0xB1, 0xB2, 0xB3, 0xB4, 0xB5, 0xB6, 0xB7, 0xB8, 0xB9, 0xBA, 0xBB, 0xBC, 0xBD, 0xBE, 0xBF,
            0xC0, 0xC1, 0xC2, 0xC3, 0xC4, 0xC5, 0xC6, 0xC8, 0xC9, 0xCA, 0xCB, 0xCC, 0xCE,
            0xD0, 0xD1, 0xD2, 0xD3, 0xD4, 0xD5, 0xD6, 0xD7, 0xD8, 0xD9, 0xDA, 0xDB, 0xDC, 0xDD, 0xDE, 0xDF,
            0xE0, 0xE1, 0xE2, 0xE3, 0xE4, 0xE5, 0xE6, 0xE7, 0xE8, 0xE9, 0xEA, 0xEB, 0xEC, 0xED, 0xEE, 0xEF,
            0xF0, 0xF1, 0xF2, 0xF3, 0xF4, 0xF5, 0xF6, 0xF7, 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF
        };

        foreach (byte opcode in opcodes)
        {
            for (int i = 0; i < iterations; i++)
            {
                TestOpCode(opcode, (op, o, n) => { o.Exec(op); n.Exec(op); });
            }
        }
    }

    [Test]
    public void TestPage2Opcodes()
    {
        var opcodes = new byte[]
        {
            0x21,0x22,0x23,0x24,0x25,0x26,0x27,0x28,0x29,0x2A,0x2B,0x2C,0x2D,0x2E,0x2F,
            0x3F,
            0x83,0x8C,0x8E,
            0x93,0x9C,0x9E,0x9F,
            0xA3,0xAC,0xAE,0xAF,
            0xB3,0xBC,0xBE,0xBF,
            0xCE,
            0xDE,0xDF,
            0xEE,0xEF,
            0xFE,0xFF
        };

        foreach (var opcode in opcodes)
        {
            for (var i = 0; i < iterations; i++)
            {
                TestOpCode(opcode, (op, o, n) => { o.Exec2(op); n.Exec2(op); });
            }
        }
    }

    [Test]
    public void TestPage3Opcodes()
    {
        var opcodes = new byte[]
        {
            0x3F,
            0x83, 0x8C,
            0x93, 0x9C,
            0xA3, 0xAC,
            0xB3, 0xBC
        };

        foreach (var opcode in opcodes)
        {
            for (var i = 0; i < iterations; i++)
            {
                TestOpCode(opcode, (op, o, n) => { o.Exec2(op); n.Exec2(op); });
            }
        }
    }

    [Test]
    public void OneOffTest()
    {
        byte opcode = 0xF3;
        var seeds = new Seeds { 228, 10, 253, 101, 120, 107, 48, 17, 167, 36, 75, 130, 179, 135, 223, 129, 33, 4, 0, 127 };
        var state = new TestState { CC = 0x3e, PC = 0x27c, S = 0xab6f, U = 0xb996, DP = 0xa, D = 0x84f1, X = 0x9db7, Y = 0xc4d7 };

        var memOld = new MemoryTester(seeds);
        var memNew = new MemoryTester(seeds);

        var _old = new OldOpcodes(memOld) { CC = state.CC, PC_REG = state.PC, S_REG = state.S, U_REG = state.U, DPA = state.DP, D_REG = state.D, X_REG = state.X, Y_REG = state.Y };
        var _new = new NewOpcodes(memNew) { CC = state.CC, PC = state.PC, S = state.S, U = state.U, DP = state.DP, D = state.D, X = state.X, Y = state.Y };

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