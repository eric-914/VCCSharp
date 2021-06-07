using System;
using System.Runtime.InteropServices;
using HBITMAP = System.IntPtr;
using HMENU = System.IntPtr;

namespace VCCSharp.Models
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct MENUITEMINFO
    {
        public uint cbSize;
        public uint fMask;
        public uint fType;
        public uint fState;
        public uint wID;
        public HMENU hSubMenu;
        public HBITMAP hbmpChecked;
        public HBITMAP hbmpUnchecked;
        public unsafe long* dwItemData;
        public unsafe byte* dwTypeData; //LPSTR
        public uint cch;
        public HBITMAP hbmpItem;
    }
}
