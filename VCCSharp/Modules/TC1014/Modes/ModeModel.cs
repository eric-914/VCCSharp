using VCCSharp.IoC;

namespace VCCSharp.Modules.TC1014.Modes
{
    public class ModeModel
    {
        public IModules Modules { get; }

        public BytePointer BytePointer { get; }
        public ShortPointer ShortPointer { get; }

        public ModeModel(BytePointer ram, IModules modules)
        {
            BytePointer = ram;
            ShortPointer = new ShortPointer(ram);
            Modules = modules;
        }
    }
}
