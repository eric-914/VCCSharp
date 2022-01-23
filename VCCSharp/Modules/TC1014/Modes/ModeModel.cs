using VCCSharp.IoC;

namespace VCCSharp.Modules.TC1014.Modes
{
    public class ModeModel
    {
        public IModules Modules { get; set; }

        public BytePointer BytePointer { get; set; }
        public ShortPointer ShortPointer { get; set; }
    }
}
