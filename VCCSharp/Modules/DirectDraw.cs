﻿using VCCSharp.Libraries;
using HINSTANCE = System.IntPtr;

namespace VCCSharp.Modules
{
    public interface IDirectDraw
    {
        bool InitDirectDraw(HINSTANCE hInstance, HINSTANCE resources);
        void ClearScreen();
    }

    public class DirectDraw : IDirectDraw
    {
        public bool InitDirectDraw(HINSTANCE hInstance, HINSTANCE resources)
        {
            return Library.DirectDraw.InitDirectDraw(hInstance, resources);
        }

        public void ClearScreen()
        {
            Library.DirectDraw.ClearScreen();
        }
    }
}