﻿using System;
using System.Runtime.InteropServices;
using System.Security;
using VCCSharp.Models;
using HRESULT = System.IntPtr;
using HWND = System.IntPtr;

namespace VCCSharp.Libraries
{
    [SuppressUnmanagedCodeSecurity]
    public static class Library
    {
        // ReSharper disable once InconsistentNaming
        public const string LIBRARY = "library.dll";

        public static class DirectDraw
        {
            #region IDirectDraw

            [DllImport(LIBRARY)]
            public static extern unsafe int DDCreate(IntPtr* dd);

            [DllImport(LIBRARY)]
            public static extern unsafe int DDGetDisplayMode(IntPtr dd, DDSURFACEDESC* ddsd);

            [DllImport(LIBRARY)]
            public static extern int DDSetCooperativeLevel(IntPtr dd, HWND hWnd, uint value);

            #endregion

            #region IDirectDraw->Create[...]

            [DllImport(LIBRARY)]
            public static extern unsafe int DDCreateSurface(IntPtr dd, IntPtr* surface, DDSURFACEDESC* ddsd);

            [DllImport(LIBRARY)]
            public static extern unsafe int DDCreateBackSurface(IntPtr dd, IntPtr* back, DDSURFACEDESC* ddsd);

            [DllImport(LIBRARY)]
            public static extern unsafe int DDCreateClipper(IntPtr dd, IntPtr* clipper);

            #endregion

            #region IDirectDrawClipper

            [DllImport(LIBRARY)]
            public static extern int DDClipperSetHWnd(IntPtr clipper, HWND hWnd);

            #endregion

            #region IDirectDrawSurface

            [DllImport(LIBRARY)]
            public static extern int DDSurfaceFlip(IntPtr surface);

            [DllImport(LIBRARY)]
            public static extern int DDSurfaceIsLost(IntPtr surface);

            [DllImport(LIBRARY)]
            public static extern void DDSurfaceRestore(IntPtr surface);

            #endregion

            #region IDirectDraw[Back]Surface

            [DllImport(LIBRARY)]
            public static extern unsafe int LockDDBackSurface(IntPtr surface, DDSURFACEDESC* ddsd, uint flags);

            [DllImport(LIBRARY)]
            public static extern int UnlockDDBackSurface(IntPtr surface);

            [DllImport(LIBRARY)]
            public static extern unsafe void GetDDBackSurfaceDC(IntPtr surface, IntPtr* pHdc);

            [DllImport(LIBRARY)]
            public static extern void ReleaseDDBackSurfaceDC(IntPtr surface, IntPtr hdc);

            [DllImport(LIBRARY)]
            public static extern int DDBackSurfaceIsLost(IntPtr back);

            [DllImport(LIBRARY)]
            public static extern void DDBackSurfaceRestore(IntPtr back);

            #endregion

            #region IDirectDrawSurface/IDirectDraw[Back]Surface

            [DllImport(LIBRARY)]
            public static extern unsafe int DDSurfaceBlt(IntPtr surface, IntPtr back, RECT* rcDest, RECT* rcSrc);

            #endregion

            #region IDirectDrawSurface/IDirectDrawClipper

            [DllImport(LIBRARY)]
            public static extern int DDSurfaceSetClipper(IntPtr surface, IntPtr clipper);

            #endregion

            #region DDSURFACEDESC

            [DllImport(LIBRARY)]
            public static extern unsafe DDSURFACEDESC* DDSDCreate();

            [DllImport(LIBRARY)]
            public static extern unsafe uint DDSDGetPitch(DDSURFACEDESC* ddsd);

            [DllImport(LIBRARY)]
            public static extern unsafe int DDSDHasSurface(DDSURFACEDESC* ddsd);

            [DllImport(LIBRARY)]
            public static extern unsafe void* DDSDGetSurface(DDSURFACEDESC* ddsd);

            [DllImport(LIBRARY)]
            public static extern unsafe void DDSDSetdwCaps(DDSURFACEDESC* ddsd, uint value);

            [DllImport(LIBRARY)]
            public static extern unsafe void DDSDSetdwWidth(DDSURFACEDESC* ddsd, uint value);

            [DllImport(LIBRARY)]
            public static extern unsafe void DDSDSetdwHeight(DDSURFACEDESC* ddsd, uint value);

            [DllImport(LIBRARY)]
            public static extern unsafe void DDSDSetdwFlags(DDSURFACEDESC* ddsd, uint value);

            #endregion
        }

        public static class DirectSound
        {
            [DllImport(LIBRARY)]
            public static extern int DirectSoundHasBuffer();

            [DllImport(LIBRARY)]
            public static extern int DirectSoundHasInterface();

            [DllImport(LIBRARY)]
            public static extern int DirectSoundBufferRelease();

            [DllImport(LIBRARY)]
            public static extern int DirectSoundCreateSoundBuffer();

            [DllImport(LIBRARY)]
            public static extern unsafe int DirectSoundInitialize(_GUID* guid);

            [DllImport(LIBRARY)]
            public static extern int DirectSoundInterfaceRelease();

            [DllImport(LIBRARY)]
            public static extern unsafe int DirectSoundLock(ulong buffOffset, ushort length, void** sndPointer1, uint* sndLength1, void** sndPointer2, uint* sndLength2);

            [DllImport(LIBRARY)]
            public static extern unsafe int DirectSoundUnlock(void* sndPointer1, uint sndLength1, void* sndPointer2, uint sndLength2);

            [DllImport(LIBRARY)]
            public static extern int DirectSoundSetCooperativeLevel(HWND hWnd, uint flag);

            [DllImport(LIBRARY)]
            public static extern unsafe void DirectSoundEnumerateSoundCards(void* fn);

            [DllImport(LIBRARY)]
            public static extern void DirectSoundSetCurrentPosition(ulong position);

            [DllImport(LIBRARY)]
            public static extern void DirectSoundSetupFormatDataStructure(ushort bitRate);

            [DllImport(LIBRARY)]
            public static extern void DirectSoundSetupSecondaryBuffer(uint sndBuffLength, uint flags);

            [DllImport(LIBRARY)]
            public static extern void DirectSoundStopAndRelease();

            [DllImport(LIBRARY)]
            public static extern int DirectSoundPlay();

            [DllImport(LIBRARY)]
            public static extern int DirectSoundStop();

            [DllImport(LIBRARY)]
            public static extern unsafe long DirectSoundGetCurrentPosition(ulong* playCursor, ulong* writeCursor);
        }

        public static class Joystick
        {
            [DllImport(LIBRARY)]
            public static extern short EnumerateJoysticks();

            [DllImport(LIBRARY)]
            public static extern int InitJoyStick(byte stickNumber);

            [DllImport(LIBRARY)]
            public static extern unsafe void* GetPollStick();

            [DllImport(LIBRARY)]
            public static extern unsafe HRESULT JoyStickPoll(void* js, byte stickNumber);

            [DllImport(LIBRARY)]
            public static extern unsafe ushort StickX(void* stick);

            [DllImport(LIBRARY)]
            public static extern unsafe ushort StickY(void* stick);

            [DllImport(LIBRARY)]
            public static extern unsafe byte Button(void* stick, int index);
        }
    }
}
