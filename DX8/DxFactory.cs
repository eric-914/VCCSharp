using DX8.Models;

namespace DX8
{
    public interface IDxFactory
    {
        IDxDraw CreateDxDraw();
        IDxSound CreateDxSound();
        IDxInput CreateDxInput();
    }

    public class DxFactory : IDxFactory
    {
        internal DxFactory() { }

        public IDxDraw CreateDxDraw() => new DxDraw();
        public IDxSound CreateDxSound() => new DxSound();
        public IDxInput CreateDxInput() => new DxInput();

        public static IDxFactory Instance => new DxFactory();
    }
}
