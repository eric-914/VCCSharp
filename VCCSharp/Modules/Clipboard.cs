using System.Windows;
using VCCSharp.Libraries;
using VCCSharp.Models;

namespace VCCSharp.Modules
{
    public interface IClipboard
    {
        unsafe ClipboardState* GetClipboardState();
        bool ClipboardEmpty();
        char PeekClipboard();
        void PopClipboard();
        void CopyText();
        void PasteText();
        void PasteBASIC();
        void PasteBASICWithNew();
    }

    public class Clipboard : IClipboard
    {
        public unsafe ClipboardState* GetClipboardState()
        {
            return Library.Clipboard.GetClipboardState();
        }

        public bool ClipboardEmpty()
        {
            return Library.Clipboard.ClipboardEmpty() == Define.TRUE;
        }

        public char PeekClipboard()
        {
            return Library.Clipboard.PeekClipboard();
        }

        public void PopClipboard()
        {
            Library.Clipboard.PopClipboard();
        }

        public void CopyText()
        {
            Library.Clipboard.CopyText();
        }

        public void PasteText()
        {
            unsafe
            {

            }

            Library.Clipboard.PasteText();
        }

        public void PasteBASIC()
        {
            unsafe
            {
                ClipboardState* instance = GetClipboardState();

                instance->CodePaste = Define.TRUE;

                PasteText();

                instance->CodePaste = Define.FALSE;
            }
        }

        public void PasteBASICWithNew()
        {
            const string warning = "Warning: This operation will erase the Coco's BASIC memory before pasting. Continue?";

            var result = MessageBox.Show(warning, "Clipboard", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.No)
            {
                return;
            }

            unsafe
            {
                ClipboardState* instance = GetClipboardState();

                instance->CodePaste = Define.TRUE;
                instance->PasteWithNew = Define.TRUE;

                PasteText();

                instance->CodePaste = Define.FALSE;
                instance->PasteWithNew = Define.FALSE;
            }
        }
    }
}
