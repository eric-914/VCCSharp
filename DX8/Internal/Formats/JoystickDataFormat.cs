// ReSharper disable CommentTypo

using DX8.Internal.Converters;
using DX8.Internal.Models;
using static System.IntPtr;

namespace DX8.Internal.Formats
{
    /// <summary>
    /// This is a dump of dinput.h :: c_dfDIJoystick2
    /// </summary>
    internal class JoystickDataFormat
    {
        public const int Count = 164;

        public static DIDATAFORMAT GetDataFormat()
        {
            return new DIDATAFORMAT
            {
                dwDataSize = 272,
                dwFlags = 1,
                dwNumObjs = Count,
                dwObjSize = 24,
                dwSize = 32,
                rgodf = IntPtrConverter.Convert(GetDataFormatArray())
            };
        }

        private static DIOBJECTDATAFORMAT[] GetDataFormatArray()
        {
            DIOBJECTDATAFORMAT X(uint dwOfs, uint unknown)
                => new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = dwOfs, dwType = 0, unknown = unknown, pguid = Zero };

            DIOBJECTDATAFORMAT Y(uint dwOfs, uint dwType, uint unknown, _GUID guid)
                => new DIOBJECTDATAFORMAT { dwFlags = 0, dwOfs = dwOfs, dwType = dwType, unknown = unknown, pguid = GuidConverter.ToIntPtr(guid) };

            DIOBJECTDATAFORMAT Z(uint dwOfs, uint dwType, uint unknown, string guid)
                => Y(dwOfs, dwType, unknown, GuidConverter.ToGuid(guid));

            //--Data has been reordered to better organize
            // ReSharper disable once RedundantExplicitArraySize
            return new DIOBJECTDATAFORMAT[Count]
            {
                X(0x80FFFF0C, 48),
                X(0x80FFFF0C, 49),
                X(0x80FFFF0C, 50),
                X(0x80FFFF0C, 51),
                X(0x80FFFF0C, 52),
                X(0x80FFFF0C, 53),
                X(0x80FFFF0C, 54),
                X(0x80FFFF0C, 55),
                X(0x80FFFF0C, 56),
                X(0x80FFFF0C, 57),
                X(0x80FFFF0C, 58),
                X(0x80FFFF0C, 59),
                X(0x80FFFF0C, 60),
                X(0x80FFFF0C, 61),
                X(0x80FFFF0C, 62),
                X(0x80FFFF0C, 63),
                X(0x80FFFF0C, 64),
                X(0x80FFFF0C, 65),
                X(0x80FFFF0C, 66),
                X(0x80FFFF0C, 67),
                X(0x80FFFF0C, 68),
                X(0x80FFFF0C, 69),
                X(0x80FFFF0C, 70),
                X(0x80FFFF0C, 71),
                X(0x80FFFF0C, 72),
                X(0x80FFFF0C, 73),
                X(0x80FFFF0C, 74),
                X(0x80FFFF0C, 75),
                X(0x80FFFF0C, 76),
                X(0x80FFFF0C, 77),
                X(0x80FFFF0C, 78),
                X(0x80FFFF0C, 79),
                X(0x80FFFF0C, 80),
                X(0x80FFFF0C, 81),
                X(0x80FFFF0C, 82),
                X(0x80FFFF0C, 83),
                X(0x80FFFF0C, 84),
                X(0x80FFFF0C, 85),
                X(0x80FFFF0C, 86),
                X(0x80FFFF0C, 87),
                X(0x80FFFF0C, 88),
                X(0x80FFFF0C, 89),
                X(0x80FFFF0C, 90),
                X(0x80FFFF0C, 91),
                X(0x80FFFF0C, 92),
                X(0x80FFFF0C, 93),
                X(0x80FFFF0C, 94),
                X(0x80FFFF0C, 95),
                X(0x80FFFF0C, 96),
                X(0x80FFFF0C, 97),
                X(0x80FFFF0C, 98),
                X(0x80FFFF0C, 99),
                X(0x80FFFF0C, 100),
                X(0x80FFFF0C, 101),
                X(0x80FFFF0C, 102),
                X(0x80FFFF0C, 103),
                X(0x80FFFF0C, 104),
                X(0x80FFFF0C, 105),
                X(0x80FFFF0C, 106),
                X(0x80FFFF0C, 107),
                X(0x80FFFF0C, 108),
                X(0x80FFFF0C, 109),
                X(0x80FFFF0C, 110),
                X(0x80FFFF0C, 111),
                X(0x80FFFF0C, 112),
                X(0x80FFFF0C, 113),
                X(0x80FFFF0C, 114),
                X(0x80FFFF0C, 115),
                X(0x80FFFF0C, 116),
                X(0x80FFFF0C, 117),
                X(0x80FFFF0C, 118),
                X(0x80FFFF0C, 119),
                X(0x80FFFF0C, 120),
                X(0x80FFFF0C, 121),
                X(0x80FFFF0C, 122),
                X(0x80FFFF0C, 123),
                X(0x80FFFF0C, 124),
                X(0x80FFFF0C, 125),
                X(0x80FFFF0C, 126),
                X(0x80FFFF0C, 127),
                X(0x80FFFF0C, 128),
                X(0x80FFFF0C, 129),
                X(0x80FFFF0C, 130),
                X(0x80FFFF0C, 131),
                X(0x80FFFF0C, 132),
                X(0x80FFFF0C, 133),
                X(0x80FFFF0C, 134),
                X(0x80FFFF0C, 135),
                X(0x80FFFF0C, 136),
                X(0x80FFFF0C, 137),
                X(0x80FFFF0C, 138),
                X(0x80FFFF0C, 139),
                X(0x80FFFF0C, 140),
                X(0x80FFFF0C, 141),
                X(0x80FFFF0C, 142),
                X(0x80FFFF0C, 143),
                X(0x80FFFF0C, 144),
                X(0x80FFFF0C, 145),
                X(0x80FFFF0C, 146),
                X(0x80FFFF0C, 147),
                X(0x80FFFF0C, 148),
                X(0x80FFFF0C, 149),
                X(0x80FFFF0C, 150),
                X(0x80FFFF0C, 151),
                X(0x80FFFF0C, 152),
                X(0x80FFFF0C, 153),
                X(0x80FFFF0C, 154),
                X(0x80FFFF0C, 155),
                X(0x80FFFF0C, 156),
                X(0x80FFFF0C, 157),
                X(0x80FFFF0C, 158),
                X(0x80FFFF0C, 159),
                X(0x80FFFF0C, 160),
                X(0x80FFFF0C, 161),
                X(0x80FFFF0C, 162),
                X(0x80FFFF0C, 163),
                X(0x80FFFF0C, 164),
                X(0x80FFFF0C, 165),
                X(0x80FFFF0C, 166),
                X(0x80FFFF0C, 167),
                X(0x80FFFF0C, 168),
                X(0x80FFFF0C, 169),
                X(0x80FFFF0C, 170),
                X(0x80FFFF0C, 171),
                X(0x80FFFF0C, 172),
                X(0x80FFFF0C, 173),
                X(0x80FFFF0C, 174),
                X(0x80FFFF0C, 175),

                Z(0x80FFFF03, 256, /* */ 0, /*  */ "A36D02E0-C9F3-11CF-BFC7-444553540000"),
                Z(0x80FFFF03, 512, /* */ 176, /**/ "A36D02E0-C9F3-11CF-BFC7-444553540000"),
                Z(0x80FFFF03, 768, /* */ 208, /**/ "A36D02E0-C9F3-11CF-BFC7-444553540000"),
                Z(0x80FFFF03, 1024, /**/ 240, /**/ "A36D02E0-C9F3-11CF-BFC7-444553540000"),

                Z(0x80FFFF03, 256, /* */ 4, /*  */ "A36D02E1-C9F3-11CF-BFC7-444553540000"),
                Z(0x80FFFF03, 512, /* */ 180, /**/ "A36D02E1-C9F3-11CF-BFC7-444553540000"),
                Z(0x80FFFF03, 768, /* */ 212, /**/ "A36D02E1-C9F3-11CF-BFC7-444553540000"),
                Z(0x80FFFF03, 1024, /**/ 244, /**/ "A36D02E1-C9F3-11CF-BFC7-444553540000"),

                Z(0x80FFFF03, 256, /* */ 8, /*  */ "A36D02E2-C9F3-11CF-BFC7-444553540000"),
                Z(0x80FFFF03, 512, /* */ 184, /**/ "A36D02E2-C9F3-11CF-BFC7-444553540000"),
                Z(0x80FFFF03, 768, /* */ 216, /**/ "A36D02E2-C9F3-11CF-BFC7-444553540000"),
                Z(0x80FFFF03, 1024, /**/ 248, /**/ "A36D02E2-C9F3-11CF-BFC7-444553540000"),

                Z(0x80FFFF03, 256, /* */ 20, /* */ "A36D02E3-C9F3-11CF-BFC7-444553540000"),
                Z(0x80FFFF03, 512, /* */ 196, /**/ "A36D02E3-C9F3-11CF-BFC7-444553540000"),
                Z(0x80FFFF03, 768, /* */ 228, /**/ "A36D02E3-C9F3-11CF-BFC7-444553540000"),
                Z(0x80FFFF03, 1024, /**/ 260, /**/ "A36D02E3-C9F3-11CF-BFC7-444553540000"),

                Z(0x80FFFF03, 256, /* */ 24, /* */ "A36D02E4-C9F3-11CF-BFC7-444553540000"),
                Z(0x80FFFF03, 512, /* */ 24, /* */ "A36D02E4-C9F3-11CF-BFC7-444553540000"),
                Z(0x80FFFF03, 768, /* */ 24, /* */ "A36D02E4-C9F3-11CF-BFC7-444553540000"),
                Z(0x80FFFF03, 1024, /**/ 24, /* */ "A36D02E4-C9F3-11CF-BFC7-444553540000"),
                //--Duplicate?
                Z(0x80FFFF03, 256, /* */ 28, /* */ "A36D02E4-C9F3-11CF-BFC7-444553540000"),
                Z(0x80FFFF03, 512, /* */ 28, /* */ "A36D02E4-C9F3-11CF-BFC7-444553540000"),
                Z(0x80FFFF03, 768, /* */ 28, /* */ "A36D02E4-C9F3-11CF-BFC7-444553540000"),
                Z(0x80FFFF03, 1024, /**/ 28, /* */ "A36D02E4-C9F3-11CF-BFC7-444553540000"),

                Z(0x80FFFF03, 256, /* */ 12, /* */ "A36D02F4-C9F3-11CF-BFC7-444553540000"),
                Z(0x80FFFF03, 512, /* */ 188, /**/ "A36D02F4-C9F3-11CF-BFC7-444553540000"),
                Z(0x80FFFF03, 768, /* */ 220, /**/ "A36D02F4-C9F3-11CF-BFC7-444553540000"),
                Z(0x80FFFF03, 1024, /**/ 252, /**/ "A36D02F4-C9F3-11CF-BFC7-444553540000"),

                Z(0x80FFFF03, 256, /* */ 16, /* */ "A36D02F5-C9F3-11CF-BFC7-444553540000"),
                Z(0x80FFFF03, 512, /* */ 192, /**/ "A36D02F5-C9F3-11CF-BFC7-444553540000"),
                Z(0x80FFFF03, 768, /* */ 224, /**/ "A36D02F5-C9F3-11CF-BFC7-444553540000"),
                Z(0x80FFFF03, 1024, /**/ 256, /**/ "A36D02F5-C9F3-11CF-BFC7-444553540000"),

                Z(0x80FFFF10, 0, /*   */ 32, /* */ "A36D02F2-C9F3-11CF-BFC7-444553540000"),
                Z(0x80FFFF10, 0, /*   */ 36, /* */ "A36D02F2-C9F3-11CF-BFC7-444553540000"),
                Z(0x80FFFF10, 0, /*   */ 40, /* */ "A36D02F2-C9F3-11CF-BFC7-444553540000"),
                Z(0x80FFFF10, 0, /*   */ 44, /* */ "A36D02F2-C9F3-11CF-BFC7-444553540000")
            };
        }
    }
}
