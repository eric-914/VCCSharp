using VCCSharp.Enums;
using VCCSharp.Modules.TCC1014.Masks;

namespace VCCSharp.Modules.TCC1014;

/// <summary>
/// Memory Management Unit (MMU)
/// </summary>
// ReSharper disable InconsistentNaming
public class MMU
{
    private readonly MemoryConfigurations _memConfiguration = new();
    private readonly MemorySizeStates _stateSwitch = new();
    private readonly RamMask _ramMask = new();

    public byte Task { get; set; }	    // $FF91 bit 0
    public bool Enabled { get; set; }	// $FF90 bit 6
    public byte State { get; set; }	    // Composite variable handles MmuTask and MmuEnabled
    public ushort Prefix { get; set; }

    public BytePointer RAM { get; } = new();
    public BytePointer ROM { get; } = new();
    public BytePointer IRB { get; } = new();    //--Internal ROM buffer

    public BytePointer[] Pages { get; } = new BytePointer[1024];
    public ushort[] PageOffsets { get; } = new ushort[1024];

    public ushort[,] Registers { get; } = new ushort[4, 8];	// $FFA0 - $FFAF

    public MemorySizes CurrentRamConfiguration { get; private set; } = MemorySizes._512K;

    public void Initialize()
    {
        Reset();

        Registers.Initialize();
        Pages.Initialize();
        PageOffsets.Initialize();
    }

    public void Reset()
    {
        Task = 0;
        Prefix = 0;
        State = 0;
        Enabled = false;
        CurrentRamConfiguration = MemorySizes._512K;

        ResetRegisters(_stateSwitch[CurrentRamConfiguration]);
    }

    public void Reset(MemorySizes ramSizeOption)
    {
        CurrentRamConfiguration = ramSizeOption;

        uint ramSize = _memConfiguration[ramSizeOption];
        
        RAM.Reset(ramSize);
        IRB.Reset(0x8000);
    }

    public void ResetPages()
    {
        for (int index = 0; index < 1024; index++)
        {
            Pages[index] = RAM.GetBytePointer((index & _ramMask[CurrentRamConfiguration]) * 0x2000);
            PageOffsets[index] = 1;
        }
    }

    private void ResetRegisters(byte offset)
    {
        //ushort[,] MmuRegisters = new ushort[4, 8];

        for (ushort index1 = 0; index1 < 8; index1++)
        {
            for (ushort index2 = 0; index2 < 4; index2++)
            {
                Registers[index2, index1] = (ushort)(index1 + offset);
            }
        }

        //for (int index = 0; index < 32; index++)
        //{
        //    instance->MmuRegisters[index] = MmuRegisters[index >> 3, index & 7];
        //}
    }

    public void SetRegister(byte register, byte data)
    {
        byte bankRegister = (byte)(register & 7);
        byte task = (byte)((register & 8) == 0 ? 0 : 1);

        //gime.c returns what was written so I can get away with this
        Registers[task, bankRegister] = (ushort)(Prefix | (data & _ramMask[CurrentRamConfiguration]));
    }

    public void SetEnabled(bool flag)
    {
        Enabled = flag;
        State = (byte)((Enabled ? 1 : 0) << 1 | Task);
    }
}
