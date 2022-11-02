namespace VCCSharp.Modules.TCC1014;

// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo
public enum PortHandlers
{
    PAK,    //--Looks like this is the default target
    PIA0,
    PIA1,
    SAM,
    GIME,
    VECT
}

public enum SAMHandlers
{
    DisplayModelControl,
    DisplayOffset,
    Page_1,
    CPURate,
    MemorySize,
    MapType
}