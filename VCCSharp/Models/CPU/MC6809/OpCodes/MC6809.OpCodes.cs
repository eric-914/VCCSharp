namespace VCCSharp.Models.CPU.MC6809;

// ReSharper disable once InconsistentNaming
public partial class MC6809
{
    private byte _temp8;
    private ushort _temp16;
    private uint _temp32;

    private byte _postByte;
    private ushort _postWord;

    #region Jump Vectors

    private static Action[] _jumpVectors = new Action[256];
    private static Action[] _jumpVectors0x10 = new Action[256]; //--Page 2 instructions
    private static Action[] _jumpVectors0x11 = new Action[256]; //--Page 3 instructions

    private void InitializeJmpVectors()
    {
        _jumpVectors = JumpVectors();
        _jumpVectors0x10 = JumpVectors0x10();
        _jumpVectors0x11 = JumpVectors0x11();
    }

    #endregion

    private byte[] _codes = new byte[256];
    private int _counter = 0;

    private void Exec(byte opCode)
    {
        _jumpVectors[opCode]();

        if (++_counter == 1000)
        {
            var filter = _codes.Select((v, i) => new { v, i = $"{i:x}" }).Where(x => x.v != 0).OrderBy(x => -x.v).ToList();

            System.Diagnostics.Debugger.Break();
        }

        _codes[opCode]++;
    }

    private void Run(byte opcode)
    {
        _codes[opcode] = 0;

        _cycleCounter += OpCodes.Exec(opcode);
    }

    /// <summary>
    /// Invalid Instruction Handler
    /// </summary>
    private static void _____()
    {
    }
}
