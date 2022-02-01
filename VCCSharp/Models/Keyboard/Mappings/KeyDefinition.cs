using System.Windows.Input;

namespace VCCSharp.Models.Keyboard.Mappings
{
    public interface IKey
    {
        byte ASCII { get; }
        Key Key { get; }
        byte DIK { get; }
        char ScanCode { get; }
        bool Shift { get; }
        bool Control { get; }
        string Text { get; }
    }

    public class KeyDefinition : IKey
    {
        public byte ASCII { get; set; }
        public Key Key { get; set; }
        public byte DIK { get; set; }
        public char ScanCode { get; set; }
        public bool Shift { get; set; }
        public bool Control { get; set; }
        public string Text { get; set; }
    }
}
