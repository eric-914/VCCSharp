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
        char Character { get; }

        bool IsMappable { get; }
    }

    public class KeyDefinition : IKey
    {
        public byte ASCII { get; set; } //--Not directly used.  More an index.
        public Key Key { get; set; }
        public byte DIK { get; set; }
        public char ScanCode { get; set; }
        public bool Shift { get; set; }
        public bool Control { get; set; }
        public string Text { get; set; }
        public char Character { get; set; }

        public bool IsMappable { get; set; }
        //private const string ForDisplay = " ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890-=[]\\;',./`";
        
        //{
        //    return ForDisplay.Contains(Character);
        //}
    }
}
