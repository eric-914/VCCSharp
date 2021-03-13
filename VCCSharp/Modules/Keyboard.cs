using VCCSharp.Libraries;
using VCCSharp.Models;

namespace VCCSharp.Modules
{
    public interface IKeyboard
    {
        unsafe KeyboardState* GetKeyboardState();
        void vccKeyboardHandleKeyDown(char key, char scanCode);
        void vccKeyboardHandleKeyUp(char key, char scanCode);
        void vccKeyboardBuildRuntimeTable(byte keyMapIndex);
        void SetPaste(bool flag);
    }

    public class Keyboard : IKeyboard
    {
        public unsafe KeyboardState* GetKeyboardState()
        {
            return Library.Keyboard.GetKeyBoardState();
        }

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
            unsafe
            {
                KeyboardState* keyboardState = GetKeyboardState();

                keyboardState->Pasting = flag ? Define.TRUE : Define.FALSE;
            }
        }
    }
}
