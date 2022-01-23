using VCCSharp.IoC;

namespace VCCSharp.Modules.TC1014.Modes
{
    public unsafe class ModeModel
    {
        public IModules Modules { get; set; }
        public byte* Memory { get; set; }
    }
}
