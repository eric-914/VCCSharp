using System.Windows.Input;

namespace VCCSharp.Models.Keyboard.Mappings
{
    public interface IKey
    {
        Key Key { get; }
        byte DIK { get; }
        byte ScanCode { get; }
        bool Shift { get; }
        string Text { get; }
        char Character { get; }

        bool IsMappable { get; }
    }

    public class KeyDefinition : IKey
    {
        public byte ASCII { get; set; } //--Not used.  Mostly for reference.
        public Key Key { get; set; }
        public byte DIK { get; set; }
        public byte ScanCode { get; set; }
        public bool Shift { get; set; }
        public string Text { get; set; }
        public char Character { get; set; }

        public bool IsMappable { get; set; }
        public bool IsAscii { get; set; }
    }
}
