using VCCSharp.IoC;

namespace VCCSharp.Modules.TC1014.Modes
{
    public class ModeModel
    {
        private MemoryPointer _bytePointer;

        public IModules Modules { get; set; }

        public MemoryPointer BytePointer
        {
            get => _bytePointer;
            set
            {
                _bytePointer = value;
                ShortPointer = new WideMemoryPointer(value);
            }
        }

        public WideMemoryPointer ShortPointer { get; private set; }
    }
}
