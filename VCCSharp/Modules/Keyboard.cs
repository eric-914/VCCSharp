using VCCSharp.Libraries;
using VCCSharp.Models;

namespace VCCSharp.Modules
{
    public interface IKeyboard
    {
        void vccKeyboardHandleKeyDown(char key, char scanCode);
        void vccKeyboardHandleKeyUp(char key, char scanCode);
        void vccKeyboardBuildRuntimeTable(byte keyMapIndex);
        void SetPaste(bool flag);
    }

    public class Keyboard : IKeyboard
    {
        public void vccKeyboardHandleKeyDown(char key, char scanCode)
        {
            Library.Keyboard.vccKeyboardHandleKeyDown(key, scanCode);
        }

        public void vccKeyboardHandleKeyUp(char key, char scanCode)
        {
            Library.Keyboard.vccKeyboardHandleKeyUp(key, scanCode);
        }

        public void vccKeyboardBuildRuntimeTable(byte keyMapIndex)
        {
            Library.Keyboard.vccKeyboardBuildRuntimeTable(keyMapIndex);
        }

        public void SetPaste(bool flag)
        {
            Library.Keyboard.SetPaste(flag ? Define.TRUE : Define.FALSE);
        }
    }
}
