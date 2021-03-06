﻿using System;

namespace VCCSharp.Models.CPU.HD6309
{
    // ReSharper disable once InconsistentNaming
    public class HD6309NatEmuCycles
    {
        public byte _65 { get; set; } = 6;
        public byte _64 { get; set; } = 6;
        public byte _32 { get; set; } = 3;
        public byte _21 { get; set; } = 2;
        public byte _54 { get; set; } = 5;
        public byte _97 { get; set; } = 9;
        public byte _85 { get; set; } = 8;
        public byte _51 { get; set; } = 5;
        public byte _31 { get; set; } = 3;
        public byte _1110 { get; set; } = 11;
        public byte _76 { get; set; } = 7;
        public byte _75 { get; set; } = 7;
        public byte _43 { get; set; } = 4;
        public byte _87 { get; set; } = 8;
        public byte _86 { get; set; } = 8;
        public byte _98 { get; set; } = 9;
        public byte _2726 { get; set; } = 27;
        public byte _3635 { get; set; } = 36;
        public byte _3029 { get; set; } = 30;
        public byte _2827 { get; set; } = 28;
        public byte _3726 { get; set; } = 37;
        public byte _3130 { get; set; } = 31;
        public byte _42 { get; set; } = 4;
        public byte _53 { get; set; } = 5;

        //public unsafe fixed byte InsCycles[2 * 25]; //[2][25];

        public byte[,] InsCycles =
        {
            {6, 5},    /* M65 */
            {6, 4},    /* M64 */
            {3, 2},    /* M32 */
            {2, 1},    /* M21 */
            {5, 4},    /* M54 */
            {9, 7},    /* M97 */
            {8, 5},    /* M85 */
            {5, 1},    /* M51 */
            {3, 1},    /* M31 */
            {11, 10},  /* M1110 */
            {7, 6},    /* M76 */
            {7, 5},    /* M75 */
            {4, 3},    /* M43 */
            {8, 7},    /* M87 */
            {8, 6},    /* M86 */
            {9, 8},    /* M98 */
            {27, 26},  /* M2726 */
            {36, 35},  /* M3635 */
            {30, 29},  /* M3029 */
            {28, 27},  /* M2827 */
            {37, 26},  /* M3726 */
            {31, 30},  /* M3130 */
            {4, 2},    /* M42 */
            {5, 3}     /* M53 */
        };

        //public unsafe fixed long /* byte* */ NatEmuCycles[24];

        public byte this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return _65;
                    case 1: return _64;
                    case 2: return _32;
                    case 3: return _21;
                    case 4: return _54;
                    case 5: return _97;
                    case 6: return _85;
                    case 7: return _51;
                    case 8: return _31;
                    case 9: return _1110;
                    case 10: return _76;
                    case 11: return _75;
                    case 12: return _43;
                    case 13: return _87;
                    case 14: return _86;
                    case 15: return _98;
                    case 16: return _2726;
                    case 17: return _3635;
                    case 18: return _3029;
                    case 19: return _2827;
                    case 20: return _3726;
                    case 21: return _3130;
                    case 22: return _42;
                    case 23: return _53;
                }

                throw new NotImplementedException();
            }

            set
            {
                switch (index)
                {
                    case 0: _65 = value; break;
                    case 1: _64 = value; break;
                    case 2: _32 = value; break;
                    case 3: _21 = value; break;
                    case 4: _54 = value; break;
                    case 5: _97 = value; break;
                    case 6: _85 = value; break;
                    case 7: _51 = value; break;
                    case 8: _31 = value; break;
                    case 9: _1110 = value; break;
                    case 10: _76 = value; break;
                    case 11: _75 = value; break;
                    case 12: _43 = value; break;
                    case 13: _87 = value; break;
                    case 14: _86 = value; break;
                    case 15: _98 = value; break;
                    case 16: _2726 = value; break;
                    case 17: _3635 = value; break;
                    case 18: _3029 = value; break;
                    case 19: _2827 = value; break;
                    case 20: _3726 = value; break;
                    case 21: _3130 = value; break;
                    case 22: _42 = value; break;
                    case 23: _53 = value; break;
                }
            }
        }
    }
}
