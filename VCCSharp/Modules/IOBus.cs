using VCCSharp.Libraries;

namespace VCCSharp.Modules
{
    // ReSharper disable once InconsistentNaming
    public interface IIOBus
    {
        byte port_read(ushort address);
        void port_write(byte data, ushort address);
    }

    // ReSharper disable once InconsistentNaming
    public class IOBus : IIOBus
    {
        public byte port_read(ushort address)
        {
            return Library.IOBus.port_read(address);
        }

        public void port_write(byte data, ushort address)
        {
            Library.IOBus.port_write(data, address);
        }
    }
}
