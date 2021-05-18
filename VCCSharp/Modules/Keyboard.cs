using VCCSharp.Enums;
using VCCSharp.Libraries;
using VCCSharp.Models;
using VCCSharp.Models.Keyboard;

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
        byte vccKeyboardGetScan(byte column);
        void SetKeyTranslations();
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

        public byte vccKeyboardGetScan(byte column)
        {
            return Library.Keyboard.vccKeyboardGetScan(column);
        }

        public void SetKeyTranslations()
        {
            Library.Keyboard.SetKeyTranslationsCoCo(KeyboardLayout.GetKeyTranslationsCoCo());
            Library.Keyboard.SetKeyTranslationsNatural(KeyboardLayout.GetKeyTranslationsNatural());
            Library.Keyboard.SetKeyTranslationsCompact(KeyboardLayout.GetKeyTranslationsCompact());
            Library.Keyboard.SetKeyTranslationsCustom(KeyboardLayout.GetKeyTranslationsCustom());
        }
    }
}
