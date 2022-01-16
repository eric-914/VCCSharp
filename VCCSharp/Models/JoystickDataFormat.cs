using VCCSharp.DX8.Models;

namespace VCCSharp.Models
{
    /// <summary>
    /// This is a dump of dinput.h :: c_dfDIJoystick2
    /// </summary>
    public class JoystickDataFormat
    {
        public unsafe DIDATAFORMAT GetDataFormat()
        {
            DIDATAFORMAT df;

            fixed (DIOBJECTDATAFORMAT* p = GetDataFormatArray())
            {
                df = new DIDATAFORMAT
                {
                    dwDataSize = 272,
                    dwFlags = 1,
                    dwNumObjs = 164,
                    dwObjSize = 24,
                    dwSize = 32,
                    rgodf = p
                };
            }

            return df;
        }

        public unsafe DIOBJECTDATAFORMAT[] GetDataFormatArray()
        {
            DIOBJECTDATAFORMAT[] a = new DIOBJECTDATAFORMAT[164];

            _GUID[] g = new _GUID[164];

            g[0] = Converter.ToGuid("A36D02E0-C9F3-11CF-BFC7-444553540000"); fixed (_GUID* p = &g[0]) { a[0] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260611, dwType = 256, unknown = 0, pguid = p }; }
            g[1] = Converter.ToGuid("A36D02E1-C9F3-11CF-BFC7-444553540000"); fixed (_GUID* p = &g[1]) { a[1] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260611, dwType = 256, unknown = 4, pguid = p }; }
            g[2] = Converter.ToGuid("A36D02E2-C9F3-11CF-BFC7-444553540000"); fixed (_GUID* p = &g[2]) { a[2] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260611, dwType = 256, unknown = 8, pguid = p }; }
            g[3] = Converter.ToGuid("A36D02F4-C9F3-11CF-BFC7-444553540000"); fixed (_GUID* p = &g[3]) { a[3] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260611, dwType = 256, unknown = 12, pguid = p }; }
            g[4] = Converter.ToGuid("A36D02F5-C9F3-11CF-BFC7-444553540000"); fixed (_GUID* p = &g[4]) { a[4] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260611, dwType = 256, unknown = 16, pguid = p }; }
            g[5] = Converter.ToGuid("A36D02E3-C9F3-11CF-BFC7-444553540000"); fixed (_GUID* p = &g[5]) { a[5] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260611, dwType = 256, unknown = 20, pguid = p }; }
            g[6] = Converter.ToGuid("A36D02E4-C9F3-11CF-BFC7-444553540000"); fixed (_GUID* p = &g[6]) { a[6] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260611, dwType = 256, unknown = 24, pguid = p }; }
            g[7] = Converter.ToGuid("A36D02E4-C9F3-11CF-BFC7-444553540000"); fixed (_GUID* p = &g[7]) { a[7] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260611, dwType = 256, unknown = 28, pguid = p }; }
            g[8] = Converter.ToGuid("A36D02F2-C9F3-11CF-BFC7-444553540000"); fixed (_GUID* p = &g[8]) { a[8] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260624, dwType = 0, unknown = 32, pguid = p }; }
            g[9] = Converter.ToGuid("A36D02F2-C9F3-11CF-BFC7-444553540000"); fixed (_GUID* p = &g[9]) { a[9] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260624, dwType = 0, unknown = 36, pguid = p }; }
            g[10] = Converter.ToGuid("A36D02F2-C9F3-11CF-BFC7-444553540000"); fixed (_GUID* p = &g[10]) { a[10] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260624, dwType = 0, unknown = 40, pguid = p }; }
            g[11] = Converter.ToGuid("A36D02F2-C9F3-11CF-BFC7-444553540000"); fixed (_GUID* p = &g[11]) { a[11] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260624, dwType = 0, unknown = 44, pguid = p }; }
            a[12] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 48, pguid = null };
            a[13] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 49, pguid = null };
            a[14] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 50, pguid = null };
            a[15] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 51, pguid = null };
            a[16] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 52, pguid = null };
            a[17] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 53, pguid = null };
            a[18] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 54, pguid = null };
            a[19] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 55, pguid = null };
            a[20] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 56, pguid = null };
            a[21] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 57, pguid = null };
            a[22] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 58, pguid = null };
            a[23] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 59, pguid = null };
            a[24] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 60, pguid = null };
            a[25] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 61, pguid = null };
            a[26] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 62, pguid = null };
            a[27] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 63, pguid = null };
            a[28] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 64, pguid = null };
            a[29] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 65, pguid = null };
            a[30] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 66, pguid = null };
            a[31] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 67, pguid = null };
            a[32] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 68, pguid = null };
            a[33] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 69, pguid = null };
            a[34] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 70, pguid = null };
            a[35] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 71, pguid = null };
            a[36] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 72, pguid = null };
            a[37] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 73, pguid = null };
            a[38] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 74, pguid = null };
            a[39] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 75, pguid = null };
            a[40] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 76, pguid = null };
            a[41] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 77, pguid = null };
            a[42] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 78, pguid = null };
            a[43] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 79, pguid = null };
            a[44] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 80, pguid = null };
            a[45] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 81, pguid = null };
            a[46] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 82, pguid = null };
            a[47] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 83, pguid = null };
            a[48] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 84, pguid = null };
            a[49] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 85, pguid = null };
            a[50] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 86, pguid = null };
            a[51] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 87, pguid = null };
            a[52] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 88, pguid = null };
            a[53] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 89, pguid = null };
            a[54] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 90, pguid = null };
            a[55] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 91, pguid = null };
            a[56] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 92, pguid = null };
            a[57] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 93, pguid = null };
            a[58] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 94, pguid = null };
            a[59] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 95, pguid = null };
            a[60] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 96, pguid = null };
            a[61] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 97, pguid = null };
            a[62] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 98, pguid = null };
            a[63] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 99, pguid = null };
            a[64] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 100, pguid = null };
            a[65] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 101, pguid = null };
            a[66] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 102, pguid = null };
            a[67] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 103, pguid = null };
            a[68] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 104, pguid = null };
            a[69] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 105, pguid = null };
            a[70] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 106, pguid = null };
            a[71] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 107, pguid = null };
            a[72] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 108, pguid = null };
            a[73] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 109, pguid = null };
            a[74] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 110, pguid = null };
            a[75] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 111, pguid = null };
            a[76] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 112, pguid = null };
            a[77] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 113, pguid = null };
            a[78] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 114, pguid = null };
            a[79] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 115, pguid = null };
            a[80] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 116, pguid = null };
            a[81] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 117, pguid = null };
            a[82] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 118, pguid = null };
            a[83] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 119, pguid = null };
            a[84] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 120, pguid = null };
            a[85] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 121, pguid = null };
            a[86] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 122, pguid = null };
            a[87] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 123, pguid = null };
            a[88] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 124, pguid = null };
            a[89] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 125, pguid = null };
            a[90] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 126, pguid = null };
            a[91] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 127, pguid = null };
            a[92] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 128, pguid = null };
            a[93] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 129, pguid = null };
            a[94] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 130, pguid = null };
            a[95] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 131, pguid = null };
            a[96] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 132, pguid = null };
            a[97] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 133, pguid = null };
            a[98] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 134, pguid = null };
            a[99] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 135, pguid = null };
            a[100] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 136, pguid = null };
            a[101] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 137, pguid = null };
            a[102] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 138, pguid = null };
            a[103] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 139, pguid = null };
            a[104] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 140, pguid = null };
            a[105] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 141, pguid = null };
            a[106] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 142, pguid = null };
            a[107] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 143, pguid = null };
            a[108] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 144, pguid = null };
            a[109] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 145, pguid = null };
            a[110] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 146, pguid = null };
            a[111] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 147, pguid = null };
            a[112] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 148, pguid = null };
            a[113] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 149, pguid = null };
            a[114] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 150, pguid = null };
            a[115] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 151, pguid = null };
            a[116] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 152, pguid = null };
            a[117] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 153, pguid = null };
            a[118] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 154, pguid = null };
            a[119] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 155, pguid = null };
            a[120] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 156, pguid = null };
            a[121] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 157, pguid = null };
            a[122] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 158, pguid = null };
            a[123] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 159, pguid = null };
            a[124] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 160, pguid = null };
            a[125] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 161, pguid = null };
            a[126] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 162, pguid = null };
            a[127] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 163, pguid = null };
            a[128] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 164, pguid = null };
            a[129] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 165, pguid = null };
            a[130] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 166, pguid = null };
            a[131] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 167, pguid = null };
            a[132] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 168, pguid = null };
            a[133] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 169, pguid = null };
            a[134] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 170, pguid = null };
            a[135] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 171, pguid = null };
            a[136] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 172, pguid = null };
            a[137] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 173, pguid = null };
            a[138] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 174, pguid = null };
            a[139] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260620, dwType = 0, unknown = 175, pguid = null };
            g[140] = Converter.ToGuid("A36D02E0-C9F3-11CF-BFC7-444553540000"); fixed (_GUID* p = &g[140]) { a[140] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260611, dwType = 512, unknown = 176, pguid = p }; }
            g[141] = Converter.ToGuid("A36D02E1-C9F3-11CF-BFC7-444553540000"); fixed (_GUID* p = &g[141]) { a[141] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260611, dwType = 512, unknown = 180, pguid = p }; }
            g[142] = Converter.ToGuid("A36D02E2-C9F3-11CF-BFC7-444553540000"); fixed (_GUID* p = &g[142]) { a[142] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260611, dwType = 512, unknown = 184, pguid = p }; }
            g[143] = Converter.ToGuid("A36D02F4-C9F3-11CF-BFC7-444553540000"); fixed (_GUID* p = &g[143]) { a[143] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260611, dwType = 512, unknown = 188, pguid = p }; }
            g[144] = Converter.ToGuid("A36D02F5-C9F3-11CF-BFC7-444553540000"); fixed (_GUID* p = &g[144]) { a[144] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260611, dwType = 512, unknown = 192, pguid = p }; }
            g[145] = Converter.ToGuid("A36D02E3-C9F3-11CF-BFC7-444553540000"); fixed (_GUID* p = &g[145]) { a[145] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260611, dwType = 512, unknown = 196, pguid = p }; }
            g[146] = Converter.ToGuid("A36D02E4-C9F3-11CF-BFC7-444553540000"); fixed (_GUID* p = &g[146]) { a[146] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260611, dwType = 512, unknown = 24, pguid = p }; }
            g[147] = Converter.ToGuid("A36D02E4-C9F3-11CF-BFC7-444553540000"); fixed (_GUID* p = &g[147]) { a[147] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260611, dwType = 512, unknown = 28, pguid = p }; }
            g[148] = Converter.ToGuid("A36D02E0-C9F3-11CF-BFC7-444553540000"); fixed (_GUID* p = &g[148]) { a[148] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260611, dwType = 768, unknown = 208, pguid = p }; }
            g[149] = Converter.ToGuid("A36D02E1-C9F3-11CF-BFC7-444553540000"); fixed (_GUID* p = &g[149]) { a[149] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260611, dwType = 768, unknown = 212, pguid = p }; }
            g[150] = Converter.ToGuid("A36D02E2-C9F3-11CF-BFC7-444553540000"); fixed (_GUID* p = &g[150]) { a[150] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260611, dwType = 768, unknown = 216, pguid = p }; }
            g[151] = Converter.ToGuid("A36D02F4-C9F3-11CF-BFC7-444553540000"); fixed (_GUID* p = &g[151]) { a[151] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260611, dwType = 768, unknown = 220, pguid = p }; }
            g[152] = Converter.ToGuid("A36D02F5-C9F3-11CF-BFC7-444553540000"); fixed (_GUID* p = &g[152]) { a[152] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260611, dwType = 768, unknown = 224, pguid = p }; }
            g[153] = Converter.ToGuid("A36D02E3-C9F3-11CF-BFC7-444553540000"); fixed (_GUID* p = &g[153]) { a[153] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260611, dwType = 768, unknown = 228, pguid = p }; }
            g[154] = Converter.ToGuid("A36D02E4-C9F3-11CF-BFC7-444553540000"); fixed (_GUID* p = &g[154]) { a[154] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260611, dwType = 768, unknown = 24, pguid = p }; }
            g[155] = Converter.ToGuid("A36D02E4-C9F3-11CF-BFC7-444553540000"); fixed (_GUID* p = &g[155]) { a[155] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260611, dwType = 768, unknown = 28, pguid = p }; }
            g[156] = Converter.ToGuid("A36D02E0-C9F3-11CF-BFC7-444553540000"); fixed (_GUID* p = &g[156]) { a[156] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260611, dwType = 1024, unknown = 240, pguid = p }; }
            g[157] = Converter.ToGuid("A36D02E1-C9F3-11CF-BFC7-444553540000"); fixed (_GUID* p = &g[157]) { a[157] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260611, dwType = 1024, unknown = 244, pguid = p }; }
            g[158] = Converter.ToGuid("A36D02E2-C9F3-11CF-BFC7-444553540000"); fixed (_GUID* p = &g[158]) { a[158] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260611, dwType = 1024, unknown = 248, pguid = p }; }
            g[159] = Converter.ToGuid("A36D02F4-C9F3-11CF-BFC7-444553540000"); fixed (_GUID* p = &g[159]) { a[159] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260611, dwType = 1024, unknown = 252, pguid = p }; }
            g[160] = Converter.ToGuid("A36D02F5-C9F3-11CF-BFC7-444553540000"); fixed (_GUID* p = &g[160]) { a[160] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260611, dwType = 1024, unknown = 256, pguid = p }; }
            g[161] = Converter.ToGuid("A36D02E3-C9F3-11CF-BFC7-444553540000"); fixed (_GUID* p = &g[161]) { a[161] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260611, dwType = 1024, unknown = 260, pguid = p }; }
            g[162] = Converter.ToGuid("A36D02E4-C9F3-11CF-BFC7-444553540000"); fixed (_GUID* p = &g[162]) { a[162] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260611, dwType = 1024, unknown = 24, pguid = p }; }
            g[163] = Converter.ToGuid("A36D02E4-C9F3-11CF-BFC7-444553540000"); fixed (_GUID* p = &g[163]) { a[163] = new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = 2164260611, dwType = 1024, unknown = 28, pguid = p }; }

            return a;
        }

        //public unsafe void Dump()
        //{
        //    DIOBJECTDATAFORMAT[] a = new DIOBJECTDATAFORMAT[164];

        //    for (int i = 0; i < 164; i++)
        //    {
        //        a[i] = Library.Joystick.GetDataObjectFormatByIndex(i);
        //    }

        //    for (int i = 0; i < 164; i++)
        //    {
        //        DIOBJECTDATAFORMAT t = a[i];

        //        if (t.pguid == null)
        //        {
        //            Debug.WriteLine($"/*{i:D3}*/ a[{i}]=new DIOBJECTDATAFORMAT {{ dwFlags={t.dwFlags},dwOfs={t.dwOfs},dwType={t.dwType},unknown={t.unknown},pguid=null}};");
        //        }
        //        else
        //        {
        //            Debug.WriteLine($"/*{i:D3}*/ g[{i}]=Converter.ToGuid(\"{Converter.ToString(*t.pguid)}\"); fixed (_GUID* p = &g[{i}]) {{ a[{i}]=new DIOBJECTDATAFORMAT {{ dwFlags={t.dwFlags},dwOfs={t.dwOfs},dwType={t.dwType},unknown={t.unknown},pguid=p}};}}");
        //        }
        //    }
        //}
    }
}
