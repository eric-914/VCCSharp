using VCCSharp.Configuration.Models;

namespace VCCSharp.Models.Configuration;

/// <summary>
/// * Supports hard drives and CompactFlash devices
/// * On-board CompactFlash socket
/// * 40 pin IDE connector
/// * 64K of user programmable FLASH
/// * Supports NitrOS-9 and Disk BASIC (using HDB-DOS)
/// * Comes with FLASH programming software and test utilities under Disk BASIC
/// * Real-time clock
/// </summary>
/// <see href="https://www.cocopedia.com/wiki/index.php/Cloud-9_SuperIDE_Interface"/>
internal class SuperIDEConfiguration : ISuperIDEConfiguration
{
    public string FilePath { get; set; } = "";
}
