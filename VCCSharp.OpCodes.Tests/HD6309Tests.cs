using VCCSharp.OpCodes.Tests.Model;
using VCCSharp.OpCodes.Tests.Model.HD6309;
using VCCSharp.OpCodes.Tests.Model.HD6309.O;
using VCCSharp.OpCodes.Tests.Model.HD6309.N;

namespace VCCSharp.OpCodes.Tests;

public class HD6309Tests
{
    #region TestOpCode 
    private void TestOpCode(byte opcode, Action<byte, OldOpcodes, NewOpcodes> exec)
    {
        var seeds = new Seeds(20);
        var state = new TestState();

        var memOld = new MemoryTester(seeds);
        var memNew = new MemoryTester(seeds);

        var _old = new OldOpcodes(memOld) { CC = state.CC, PC_REG = state.PC, S_REG = state.S, U_REG = state.U, DPA = state.DP, D_REG = state.D, X_REG = state.X, Y_REG = state.Y, MD = state.MD , W_REG = state.W, V_REG = state.V};
        var _new = new NewOpcodes(memNew) { CC = state.CC, PC = state.PC, S = state.S, U = state.U, DP = state.DP, D = state.D, X = state.X, Y = state.Y, MD = state.MD, W = state.W, V = state.V };

        exec(opcode, _old, _new);

        string message = $@"
byte opcode=0x{opcode:x}; 
var seeds = new Seeds {{{string.Join(", ", seeds)}}};
var state = new TestState {{ CC=0x{state.CC:x}, PC=0x{state.PC:x}, S=0x{state.S:x}, U=0x{state.U:x}, DP=0x{state.DP:x}, D=0x{state.D:x}, X=0x{state.X:x}, Y=0x{state.Y:x}, MD=0x{state.MD:x}, W=0x{state.W:x}, V=0x{state.V:x} }};
";

        Assert.That(_old.CC, Is.EqualTo(_new.CC), message);
        Assert.That(_old.PC_REG, Is.EqualTo(_new.PC), message);
        Assert.That(_old.S_REG, Is.EqualTo(_new.S), message);
        Assert.That(_old.U_REG, Is.EqualTo(_new.U), message);
        Assert.That(_old.DPA, Is.EqualTo(_new.DP), message);
        Assert.That(_old.X_REG, Is.EqualTo(_new.X), message);
        Assert.That(_old.Y_REG, Is.EqualTo(_new.Y), message);
        Assert.That(_old.Cycles, Is.EqualTo(_new.Cycles), message);

        Assert.That(_old.MD, Is.EqualTo(_new.MD), message);
        Assert.That(_old.W_REG, Is.EqualTo(_new.W), message);
        Assert.That(_old.V_REG, Is.EqualTo(_new.V), message);
    }
    #endregion

    [Test]
    public void OneOffTest()
    {
        byte opcode = 0x9f;
        var seeds = new Seeds { 139, 87, 180, 137, 14, 117, 229, 20, 208, 227, 139, 71, 104, 105, 131, 79, 34, 255, 33, 244 };
        var state = new TestState { CC = 0xd8, PC = 0x2952, S = 0x3008, U = 0x2d99, DP = 0x13, D = 0xbd8e, X = 0xbd22, Y = 0xb3dd };

        var memOld = new MemoryTester(seeds);
        var memNew = new MemoryTester(seeds);

        var _old = new OldOpcodes(memOld) { CC = state.CC, PC_REG = state.PC, S_REG = state.S, U_REG = state.U, DPA = state.DP, D_REG = state.D, X_REG = state.X, Y_REG = state.Y };
        var _new = new NewOpcodes(memNew) { CC = state.CC, PC = state.PC, S = state.S, U = state.U, DP = state.DP, D = state.D, X = state.X, Y = state.Y };

        _old.Exec2(opcode);
        _new.Exec2(opcode);

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
