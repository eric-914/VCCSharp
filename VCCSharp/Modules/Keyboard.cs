using VCCSharp.Enums;
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
        void GimeSetKeyboardInterruptState(byte state);
    }

    public class Keyboard : IKeyboard
    {
        public unsafe KeyboardState* GetKeyboardState()
        {
            return Library.Keyboard.GetKeyBoardState();
        }

        public void vccKeyboardHandleKeyDown(char key, char scanCode)
        {
            vccKeyboardHandleKey(key, scanCode, KeyStates.kEventKeyDown);
        }

        public void vccKeyboardHandleKeyUp(char key, char scanCode)
        {
            vccKeyboardHandleKey(key, scanCode, KeyStates.kEventKeyUp);
        }

        public void SetPaste(bool flag)
        {
            unsafe
            {
                KeyboardState* keyboardState = GetKeyboardState();

                keyboardState->Pasting = flag ? Define.TRUE : Define.FALSE;
            }
        }

        public void vccKeyboardHandleKey(char key, char scanCode, KeyStates keyState)
        {
            Library.Keyboard.vccKeyboardHandleKey(key, scanCode, keyState);
        }

        public void vccKeyboardBuildRuntimeTable(byte keyMapIndex)
        {
            Library.Keyboard.vccKeyboardBuildRuntimeTable(keyMapIndex);
        }

        public void GimeSetKeyboardInterruptState(byte state)
        {
            Library.Keyboard.GimeSetKeyboardInterruptState(state);
        }
    }
}
