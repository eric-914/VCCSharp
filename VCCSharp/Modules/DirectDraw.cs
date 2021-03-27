﻿using System;
using System.Text;
using System.Windows;
using VCCSharp.Enums;
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
        void SetAspect(byte forceAspect);
        unsafe void DisplayFlip(EmuState* emuState);
    }

    public class DirectDraw : IDirectDraw
    {
        private readonly IModules _modules;

        private static int _textX = 0, _textY = 0;
        private static byte _counter = 0, _counter1 = 32, _phase = 1;

        public DirectDraw(IModules modules)
        {
            _modules = modules;
        }

        public unsafe DirectDrawState* GetDirectDrawState()
        {
            return Library.DirectDraw.GetDirectDrawState();
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

        public void SetAspect(byte forceAspect)
        {
            unsafe
            {
                DirectDrawState* instance = GetDirectDrawState();

                instance->ForceAspect = forceAspect;
            }
        }

        public unsafe void DoCls(EmuState* emuState)
        {
            DirectDrawState* instance = GetDirectDrawState();
            GraphicsSurfaces* graphicsSurfaces = _modules.Graphics.GetGraphicsSurfaces();

            if (LockScreen(emuState) == Define.TRUE)
            {
                return;
            }

            switch ((BitDepthStates)(emuState->BitDepth))
            {
                case BitDepthStates.BIT_8:
                    for (int y = 0; y < 480; y++)
                    {
                        for (int x = 0; x < 640; x++)
                        {
                            graphicsSurfaces->pSurface8[x + (y * emuState->SurfacePitch)] = (byte)(instance->Color | 128);
                        }
                    }
                    break;

                case BitDepthStates.BIT_16:
                    for (int y = 0; y < 480; y++)
                    {
                        for (int x = 0; x < 640; x++)
                        {
                            graphicsSurfaces->pSurface16[x + (y * emuState->SurfacePitch)] = instance->Color;
                        }
                    }
                    break;

                case BitDepthStates.BIT_24:
                    for (int y = 0; y < 480; y++)
                    {
                        for (int x = 0; x < 640; x++)
                        {
                            graphicsSurfaces->pSurface8[(x * 3) + (y * emuState->SurfacePitch)] = (byte)((instance->Color & 0xFF0000) >> 16);
                            graphicsSurfaces->pSurface8[(x * 3) + 1 + (y * emuState->SurfacePitch)] = (byte)((instance->Color & 0x00FF00) >> 8);
                            graphicsSurfaces->pSurface8[(x * 3) + 2 + (y * emuState->SurfacePitch)] = (byte)(instance->Color & 0xFF);
                        }
                    }
                    break;

                case BitDepthStates.BIT_32:
                    for (int y = 0; y < 480; y++)
                    {
                        for (int x = 0; x < 640; x++)
                        {
                            graphicsSurfaces->pSurface32[x + (y * emuState->SurfacePitch)] = instance->Color;
                        }
                    }
                    break;
            }

            UnlockScreen(emuState);
        }

        public void ClearScreen()
        {
            unsafe
            {
                GetDirectDrawState()->Color = 0;
            }
        }

        public unsafe float Static(EmuState* emuState)
        {
            Static(emuState, _modules.Graphics.GetGraphicsSurfaces());

            return _modules.Throttle.CalculateFPS();
        }

        private unsafe void Static(EmuState* emuState, GraphicsSurfaces* graphicsSurfaces)
        {
            var random = new Random();

            LockScreen(emuState);

            if (graphicsSurfaces->pSurface32 == null)
            {
                return; //TODO: Seems bad to exit w/out unlocking first
            }

            switch ((BitDepthStates)(emuState->BitDepth))
            {
                case BitDepthStates.BIT_8:
                    byte[] greyScales = { 128, 135, 184, 191 };

                    for (int y = 0; y < 480; y += 2)
                    {
                        for (int x = 0; x < 160; x++)
                        {
                            byte temp = (byte)(random.Next() & 3);

                            graphicsSurfaces->pSurface32[x + (y * emuState->SurfacePitch >> 2)] = (uint)(greyScales[temp] | (greyScales[temp] << 8) | (greyScales[temp] << 16) | (greyScales[temp] << 24));
                            graphicsSurfaces->pSurface32[x + ((y + 1) * emuState->SurfacePitch >> 2)] = (uint)(greyScales[temp] | (greyScales[temp] << 8) | (greyScales[temp] << 16) | (greyScales[temp] << 24));
                        }
                    }
                    break;

                case BitDepthStates.BIT_16:
                    for (int y = 0; y < 480; y += 2)
                    {
                        for (int x = 0; x < 320; x++)
                        {
                            byte temp = (byte)(random.Next() & 31);

                            graphicsSurfaces->pSurface32[x + (y * emuState->SurfacePitch >> 1)] = (uint)(temp | (temp << 6) | (temp << 11) | (temp << 16) | (temp << 22) | (temp << 27));
                            graphicsSurfaces->pSurface32[x + ((y + 1) * emuState->SurfacePitch >> 1)] = (uint)(temp | (temp << 6) | (temp << 11) | (temp << 16) | (temp << 22) | (temp << 27));
                        }
                    }
                    break;

                case BitDepthStates.BIT_24: //--TODO: Don't think this was ever tested
                    for (int y = 0; y < 480; y++)
                    {
                        for (int x = 0; x < 640; x++)
                        {
                            byte temp = (byte)(random.Next() & 255);
                            graphicsSurfaces->pSurface8[(x * 3) + (y * emuState->SurfacePitch)] = temp;
                            graphicsSurfaces->pSurface8[(x * 3) + 1 + (y * emuState->SurfacePitch)] = temp;
                            graphicsSurfaces->pSurface8[(x * 3) + 2 + (y * emuState->SurfacePitch)] = temp;
                        }
                    }
                    break;

                case BitDepthStates.BIT_32:
                    for (int y = 0; y < 480; y++)
                    {
                        for (int x = 0; x < 640; x++)
                        {
                            byte temp = (byte)(random.Next() & 255);

                            graphicsSurfaces->pSurface32[x + (y * emuState->SurfacePitch)] = (uint)(temp | (temp << 8) | (temp << 16));
                        }
                    }
                    break;
            }

            byte color = (byte)(_counter1 << 2);

            ShowStaticMessage((ushort)_textX, (ushort)_textY, (uint)(color << 16 | color << 8 | color));

            _counter++;
            _counter1 += _phase;

            if ((_counter1 == 60) || (_counter1 == 20)) {
                _phase = (byte)(-_phase);
            }

            _counter %= 60; //about 1 seconds

            if (_counter == 0)
            {
                _textX = (ushort)(random.Next() % 580);
                _textY = (ushort)(random.Next() % 470);
            }

            UnlockScreen(emuState);
        }

        public unsafe void UnlockScreen(EmuState* emuState)
        {
            DirectDrawState* instance = GetDirectDrawState();

            if (emuState->FullScreen == Define.TRUE && instance->InfoBand == Define.TRUE) 
            {
                WriteStatusText(Converter.ToString(instance->StatusText));
            }

            UnlockSurface();

            DisplayFlip(emuState);
        }

        public void ShowStaticMessage(ushort x, ushort y, uint color)
        {
            unsafe
            {
                const string message = " Signal Missing! Press F9";
                void* hdc;

                GetSurfaceDC(&hdc);

                _modules.GDI.GDISetBkColor(hdc, 0);
                _modules.GDI.GDISetTextColor(hdc, color);
                _modules.GDI.GDIWriteTextOut(hdc, x, y, message);

                ReleaseSurfaceDC(hdc);
            }
        }

        public void WriteStatusText(string statusText)
        {
            unsafe
            {
                void* hdc;

                var sb = new StringBuilder(statusText.PadRight(134-statusText.Length));

                GetSurfaceDC(&hdc);

                _modules.GDI.GDISetBkColor(hdc, 0); //RGB(0, 0, 0)
                _modules.GDI.GDISetTextColor(hdc, 0xFFFFFF); //RGB(255, 255, 255)
                _modules.GDI.GDITextOut(hdc, 0, 0, sb.ToString(), 132);

                ReleaseSurfaceDC(hdc);
            }
        }

        public unsafe void SetStatusBarText(string text, EmuState* emuState)
        {
            DirectDrawState* instance = GetDirectDrawState();

            if (emuState->FullScreen == Define.FALSE)
            {
                SetStatusBarText(text);
            }
            else
            {
                Converter.ToByteArray(text, instance->StatusText);
            }
        }

        private void SetStatusBarText(string text)
        {
            Library.DirectDraw.SetStatusBarText(text);
        }

        public bool InitDirectDraw(HINSTANCE hInstance, HINSTANCE resources)
        {
            return Library.DirectDraw.InitDirectDraw(hInstance, resources);
        }

        public unsafe bool CreateDirectDrawWindow(EmuState* emuState)
        {
            return Library.DirectDraw.CreateDirectDrawWindow(emuState) == Define.TRUE;
        }
        
        public unsafe byte LockScreen(EmuState* emuState)
        {
            return Library.DirectDraw.LockScreen(emuState);
        }

        public unsafe void DisplayFlip(EmuState* emuState)
        {
            Library.DirectDraw.DisplayFlip(emuState);
        }

        public int UnlockSurface()
        {
            return Library.DirectDraw.UnlockSurface();
        }

        //--TODO: I don't know what HDC is.
        public unsafe void GetSurfaceDC(void** pHdc)
        {
            Library.DirectDraw.GetSurfaceDC(pHdc);
        }

        //--TODO: I don't know what HDC is.
        public unsafe void ReleaseSurfaceDC(void* hdc)
        {
            Library.DirectDraw.ReleaseSurfaceDC(hdc);
        }
    }
}
