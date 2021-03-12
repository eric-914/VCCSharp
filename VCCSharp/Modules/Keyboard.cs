using VCCSharp.Libraries;

namespace VCCSharp.Modules
{
    public interface IKeyboard
    {
        void vccKeyboardHandleKeyDown(char key, char scanCode);
        void vccKeyboardHandleKeyUp(char key, char scanCode);
        void vccKeyboardBuildRuntimeTable(byte keyMapIndex);
        void SetPaste(int flag);
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

        public void SetPaste(int flag)
        {
            Library.Keyboard.SetPaste(flag);
        }
    }
}
