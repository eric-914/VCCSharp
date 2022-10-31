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

    public MemorySizes CurrentRamConfiguration { get; set; } = MemorySizes._512K;

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

    public void ResetRegisters(byte offset)
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
}
