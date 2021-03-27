using System;
using System.Windows;
using VCCSharp.IoC;
using VCCSharp.Libraries;
using VCCSharp.Models;
using static System.IntPtr;
using HINSTANCE = System.IntPtr;

namespace VCCSharp.Modules
{
    public interface IDirectDraw
    {
        unsafe DirectDrawState* GetDirectDrawState();
        bool InitDirectDraw(HINSTANCE hInstance, HINSTANCE resources);
        void ClearScreen();
        void FullScreenToggle();
        unsafe bool CreateDirectDrawWindow(EmuState* emuState);
        unsafe void SetStatusBarText(string textBuffer, EmuState* emuState);
        unsafe float Static(EmuState* emuState);
        unsafe void DoCls(EmuState* emuState);
        unsafe byte LockScreen(EmuState* emuState);
        unsafe void UnlockScreen(EmuState* emuState);
        byte SetAspect(byte forceAspect);
    }

    public class DirectDraw : IDirectDraw
    {
        private readonly IModules _modules;

        public DirectDraw(IModules modules)
        {
            _modules = modules;
        }

        public unsafe DirectDrawState* GetDirectDrawState()
        {
            return Library.DirectDraw.GetDirectDrawState();
        }

        public bool InitDirectDraw(HINSTANCE hInstance, HINSTANCE resources)
        {
            return Library.DirectDraw.InitDirectDraw(hInstance, resources);
        }

        public void ClearScreen()
        {
            Library.DirectDraw.ClearScreen();
        }

        public unsafe bool CreateDirectDrawWindow(EmuState* emuState)
        {
            return Library.DirectDraw.CreateDirectDrawWindow(emuState) == Define.TRUE;
        }

        public unsafe void SetStatusBarText(string text, EmuState* emuState)
        {
            Library.DirectDraw.SetStatusBarText(text, emuState);
        }

        public unsafe float Static(EmuState* emuState)
        {
            Library.DirectDraw.Static(emuState);

            return _modules.Throttle.CalculateFPS();
        }

        public unsafe void DoCls(EmuState* emuState)
        {
            Library.DirectDraw.DoCls(emuState);
        }

        public unsafe byte LockScreen(EmuState* emuState)
        {
            return Library.DirectDraw.LockScreen(emuState);
        }

        public unsafe void UnlockScreen(EmuState* emuState)
        {
            Library.DirectDraw.UnlockScreen(emuState);
        }

        public byte SetAspect(byte forceAspect)
        {
            return Library.DirectDraw.SetAspect(forceAspect);
        }

        public void FullScreenToggle()
        {
            unsafe
            {
                EmuState* emuState = _modules.Emu.GetEmuState();

                _modules.Audio.PauseAudio(Define.TRUE);

                if (!CreateDirectDrawWindow(emuState))
                {
                    MessageBox.Show("Can't rebuild primary Window", "Error");

                    Environment.Exit(0);
                }

                _modules.Graphics.InvalidateBorder();
                _modules.Callbacks.RefreshDynamicMenu(emuState);

                //TODO: Guess it wants to close other windows/dialogs
                emuState->ConfigDialog = Zero;

                _modules.Audio.PauseAudio(Define.FALSE);
            }
        }
    }
}
