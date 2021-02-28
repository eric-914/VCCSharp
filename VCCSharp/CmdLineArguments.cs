using System.Runtime.InteropServices;

namespace VCCSharp
{
    public struct CmdLineArguments
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string QLoadFile;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string IniFile;
    }
}
