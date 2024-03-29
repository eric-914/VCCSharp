﻿using DX8.Internal.Models;
using System.Globalization;
using System.Runtime.InteropServices;

namespace DX8.Internal.Converters
{
    internal static class GuidConverter
    {
        public static string ToString(_GUID guid)
        {
            string c1 = $"{guid.Data1:X8}";
            string c2 = $"{guid.Data2:X8}".Substring(4, 4);
            string c3 = $"{guid.Data3:X8}".Substring(4, 4);

            uint d0 = guid.Data4[0];
            uint d1 = guid.Data4[1];
            uint d2 = guid.Data4[2];
            uint d3 = guid.Data4[3];
            uint d4 = guid.Data4[4];
            uint d5 = guid.Data4[5];
            uint d6 = guid.Data4[6];
            uint d7 = guid.Data4[7];

            // ReSharper disable InconsistentNaming
            uint d4a = d0 << 8 | d1;
            uint d4b = d2 << 8 | d3;
            uint d4c = d4 << 24 | d5 << 16 | d6 << 8 | d7;

            string c4a = $"{d4a:X8}".Substring(4, 4);
            string c4b = $"{d4b:X8}".Substring(4, 4);
            string c4c = $"{d4c:X8}";
            // ReSharper restore InconsistentNaming

            return $"{c1}-{c2}-{c3}-{c4a}-{c4b}{c4c}";
        }

        public static _GUID ToGuid(string s)
        {
            var g = new _GUID
            {
                //--Range Operator:
                //The left-hand operand is an inclusive start of a range. The right-hand operand is an exclusive end of a range
                Data1 = uint.Parse(s[..8], NumberStyles.AllowHexSpecifier), //First 8
                Data2 = ushort.Parse(s[9..13], NumberStyles.AllowHexSpecifier), //9+4
                Data3 = ushort.Parse(s[14..18], NumberStyles.AllowHexSpecifier) //14+4
            };

            g.Data4[0] = byte.Parse(s[19..21], NumberStyles.AllowHexSpecifier); //19+2
            g.Data4[1] = byte.Parse(s[21..23], NumberStyles.AllowHexSpecifier); //21+2
            g.Data4[2] = byte.Parse(s[24..26], NumberStyles.AllowHexSpecifier); //24+2
            g.Data4[3] = byte.Parse(s[26..28], NumberStyles.AllowHexSpecifier); //26+2
            g.Data4[4] = byte.Parse(s[28..30], NumberStyles.AllowHexSpecifier); //28+2
            g.Data4[5] = byte.Parse(s[30..32], NumberStyles.AllowHexSpecifier); //30+2
            g.Data4[6] = byte.Parse(s[32..34], NumberStyles.AllowHexSpecifier); //32+2
            g.Data4[7] = byte.Parse(s[34..36], NumberStyles.AllowHexSpecifier); //34+2

            return g;
        }

        public static IntPtr ToIntPtr(_GUID guid) => IntPtrConverter.Convert(guid);

        public static _GUID ToGuid(IntPtr pGuid)
        {
            if (pGuid == IntPtr.Zero) return new _GUID();

            int len = Marshal.SizeOf(typeof(_GUID)); 
            byte[] destination = new byte[len];

            //--Copy pGuid contents into byte array
            Marshal.Copy(pGuid, destination, 0, len); 

            IntPtr ptr = Marshal.AllocHGlobal(len);
            
            //--Copy byte array into new _Guid
            Marshal.Copy(destination, 0, ptr, len);

            object? o = Marshal.PtrToStructure(ptr, typeof(_GUID));
            if (o == null)
            {
                throw new NullReferenceException("Failed to convert IntPtr to _GUID");
            }

            _GUID guid = (_GUID)o;

            Marshal.FreeHGlobal(ptr);

            return guid;
        }
    }
}
