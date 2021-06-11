using System;

namespace VCCSharp.Models
{
    // ReSharper disable InconsistentNaming
    // ReSharper disable IdentifierTypo
    public static class Define
    {
        public static readonly IntPtr INVALID_HANDLE_VALUE = (IntPtr.Add(IntPtr.Zero, -1));

        public const byte TRUE = 1;     //--Need to be careful, as TRUE doesn't necessarily mean NOT FALSE
        public const byte FALSE = 0;

        public const int IGNORE = 0;

        public const int MAX_LOADSTRING = 100;

        public const int MAX_PATH = 260;

        public const long S_OK = 0;
        public const long DS_OK = S_OK;

        public const int MAXCARDS = 12;

        public const ushort FRAMEINTERVAL = 120;
        public const int TARGETFRAMERATE = 60;
        public const int LINESPERFIELD = 262;

        public const double FRAMESPERSECORD = 59.923;   //The coco really runs at about 59.923 Frames per second
        public const double PICOSECOND = 1000000000;
        public const double COLORBURST = 3579545;

        public const int AUDIOBUFFERS = 12;

        public const int TAPEAUDIORATE = 44100;

        public const uint DDSCL_NORMAL = 0x00000008;

        public const uint DDSCAPS_PRIMARYSURFACE = 0x00000200;
        public const uint DDSCAPS_VIDEOMEMORY = 0x00004000;
        public const uint DDSCAPS_SYSTEMMEMORY = 0x00000800;

        public const uint DDSD_CAPS = 0x00000001;
        public const uint DDSD_HEIGHT = 0x00000002;
        public const uint DDSD_WIDTH = 0x00000004;

        public const int SW_SHOWDEFAULT = 10;

        // MC6809 Vector table
        public const ushort VTRAP = 0xFFF0;
        public const ushort VSWI3 = 0xFFF2;
        public const ushort VSWI2 = 0xFFF4;
        public const ushort VFIRQ = 0xFFF6;
        public const ushort VIRQ = 0xFFF8;
        public const ushort VSWI = 0xFFFA;
        public const ushort VNMI = 0xFFFC;
        public const ushort VRESET = 0xFFFE;

        public const byte STOP = 0;
        public const byte PLAY = 1;
        public const byte REC = 2;
        public const byte EJECT = 3;

        #region ScanCodes (1)

        /****************************************************************************
         *
         *      DirectInput keyboard scan codes
         *
         ****************************************************************************/
        //
        //    Copyright (C) Microsoft.  All rights reserved.
        //
        public const byte DIK_ESCAPE = 0x01;
        public const byte DIK_1 = 0x02;
        public const byte DIK_2 = 0x03;
        public const byte DIK_3 = 0x04;
        public const byte DIK_4 = 0x05;
        public const byte DIK_5 = 0x06;
        public const byte DIK_6 = 0x07;
        public const byte DIK_7 = 0x08;
        public const byte DIK_8 = 0x09;
        public const byte DIK_9 = 0x0A;
        public const byte DIK_0 = 0x0B;
        public const byte DIK_MINUS = 0x0C;    /* - on main keyboard */
        public const byte DIK_EQUALS = 0x0D;
        public const byte DIK_BACK = 0x0E;    /* backspace */
        public const byte DIK_TAB = 0x0F;
        public const byte DIK_Q = 0x10;
        public const byte DIK_W = 0x11;
        public const byte DIK_E = 0x12;
        public const byte DIK_R = 0x13;
        public const byte DIK_T = 0x14;
        public const byte DIK_Y = 0x15;
        public const byte DIK_U = 0x16;
        public const byte DIK_I = 0x17;
        public const byte DIK_O = 0x18;
        public const byte DIK_P = 0x19;
        public const byte DIK_LBRACKET = 0x1A;
        public const byte DIK_RBRACKET = 0x1B;
        public const byte DIK_RETURN = 0x1C;    /* Enter on main keyboard */
        public const byte DIK_LCONTROL = 0x1D;
        public const byte DIK_A = 0x1E;
        public const byte DIK_S = 0x1F;
        public const byte DIK_D = 0x20;
        public const byte DIK_F = 0x21;
        public const byte DIK_G = 0x22;
        public const byte DIK_H = 0x23;
        public const byte DIK_J = 0x24;
        public const byte DIK_K = 0x25;
        public const byte DIK_L = 0x26;
        public const byte DIK_SEMICOLON = 0x27;
        public const byte DIK_APOSTROPHE = 0x28;
        public const byte DIK_GRAVE = 0x29;    /* accent grave */
        public const byte DIK_LSHIFT = 0x2A;
        public const byte DIK_BACKSLASH = 0x2B;
        public const byte DIK_Z = 0x2C;
        public const byte DIK_X = 0x2D;
        public const byte DIK_C = 0x2E;
        public const byte DIK_V = 0x2F;
        public const byte DIK_B = 0x30;
        public const byte DIK_N = 0x31;
        public const byte DIK_M = 0x32;
        public const byte DIK_COMMA = 0x33;
        public const byte DIK_PERIOD = 0x34;    /* . on main keyboard */
        public const byte DIK_SLASH = 0x35;    /* / on main keyboard */
        public const byte DIK_RSHIFT = 0x36;
        public const byte DIK_MULTIPLY = 0x37;    /* * on numeric keypad */
        public const byte DIK_LMENU = 0x38;    /* left Alt */
        public const byte DIK_SPACE = 0x39;
        public const byte DIK_CAPITAL = 0x3A;
        public const byte DIK_F1 = 0x3B;
        public const byte DIK_F2 = 0x3C;
        public const byte DIK_F3 = 0x3D;
        public const byte DIK_F4 = 0x3E;
        public const byte DIK_F5 = 0x3F;
        public const byte DIK_F6 = 0x40;
        public const byte DIK_F7 = 0x41;
        public const byte DIK_F8 = 0x42;
        public const byte DIK_F9 = 0x43;
        public const byte DIK_F10 = 0x44;
        public const byte DIK_NUMLOCK = 0x45;
        public const byte DIK_SCROLL = 0x46;    /* Scroll Lock */
        public const byte DIK_NUMPAD7 = 0x47;
        public const byte DIK_NUMPAD8 = 0x48;
        public const byte DIK_NUMPAD9 = 0x49;
        public const byte DIK_SUBTRACT = 0x4A;    /* - on numeric keypad */
        public const byte DIK_NUMPAD4 = 0x4B;
        public const byte DIK_NUMPAD5 = 0x4C;
        public const byte DIK_NUMPAD6 = 0x4D;
        public const byte DIK_ADD = 0x4E;    /* + on numeric keypad */
        public const byte DIK_NUMPAD1 = 0x4F;
        public const byte DIK_NUMPAD2 = 0x50;
        public const byte DIK_NUMPAD3 = 0x51;
        public const byte DIK_NUMPAD0 = 0x52;
        public const byte DIK_DECIMAL = 0x53;    /* . on numeric keypad */
        public const byte DIK_OEM_102 = 0x56;    /* <> or \| on RT 102-key keyboard (Non-U.S.) */
        public const byte DIK_F11 = 0x57;
        public const byte DIK_F12 = 0x58;
        public const byte DIK_F13 = 0x64;    /*                     (NEC PC98) */
        public const byte DIK_F14 = 0x65;    /*                     (NEC PC98) */
        public const byte DIK_F15 = 0x66;    /*                     (NEC PC98) */
        public const byte DIK_KANA = 0x70;    /* (Japanese keyboard)            */
        public const byte DIK_ABNT_C1 = 0x73;    /* /? on Brazilian keyboard */
        public const byte DIK_CONVERT = 0x79;    /* (Japanese keyboard)            */
        public const byte DIK_NOCONVERT = 0x7B;    /* (Japanese keyboard)            */
        public const byte DIK_YEN = 0x7D;    /* (Japanese keyboard)            */
        public const byte DIK_ABNT_C2 = 0x7E;    /* Numpad . on Brazilian keyboard */
        public const byte DIK_NUMPADEQUALS = 0x8D;    /* = on numeric keypad (NEC PC98) */
        public const byte DIK_PREVTRACK = 0x90;    /* Previous Track (DIK_CIRCUMFLEX on Japanese keyboard) */
        public const byte DIK_AT = 0x91;    /*                     (NEC PC98) */
        public const byte DIK_COLON = 0x92;    /*                     (NEC PC98) */
        public const byte DIK_UNDERLINE = 0x93;    /*                     (NEC PC98) */
        public const byte DIK_KANJI = 0x94;    /* (Japanese keyboard)            */
        public const byte DIK_STOP = 0x95;    /*                     (NEC PC98) */
        public const byte DIK_AX = 0x96;    /*                     (Japan AX) */
        public const byte DIK_UNLABELED = 0x97;    /*                        (J3100) */
        public const byte DIK_NEXTTRACK = 0x99;    /* Next Track */
        public const byte DIK_NUMPADENTER = 0x9C;    /* Enter on numeric keypad */
        public const byte DIK_RCONTROL = 0x9D;
        public const byte DIK_MUTE = 0xA0;    /* Mute */
        public const byte DIK_CALCULATOR = 0xA1;    /* Calculator */
        public const byte DIK_PLAYPAUSE = 0xA2;    /* Play / Pause */
        public const byte DIK_MEDIASTOP = 0xA4;    /* Media Stop */
        public const byte DIK_VOLUMEDOWN = 0xAE;    /* Volume - */
        public const byte DIK_VOLUMEUP = 0xB0;    /* Volume + */
        public const byte DIK_WEBHOME = 0xB2;    /* Web home */
        public const byte DIK_NUMPADCOMMA = 0xB3;    /* , on numeric keypad (NEC PC98) */
        public const byte DIK_DIVIDE = 0xB5;    /* / on numeric keypad */
        public const byte DIK_SYSRQ = 0xB7;
        public const byte DIK_RMENU = 0xB8;    /* right Alt */
        public const byte DIK_PAUSE = 0xC5;    /* Pause */
        public const byte DIK_HOME = 0xC7;    /* Home on arrow keypad */
        public const byte DIK_UP = 0xC8;    /* UpArrow on arrow keypad */
        public const byte DIK_PRIOR = 0xC9;    /* PgUp on arrow keypad */
        public const byte DIK_LEFT = 0xCB;    /* LeftArrow on arrow keypad */
        public const byte DIK_RIGHT = 0xCD;    /* RightArrow on arrow keypad */
        public const byte DIK_END = 0xCF;    /* End on arrow keypad */
        public const byte DIK_DOWN = 0xD0;    /* DownArrow on arrow keypad */
        public const byte DIK_NEXT = 0xD1;    /* PgDn on arrow keypad */
        public const byte DIK_INSERT = 0xD2;    /* Insert on arrow keypad */
        public const byte DIK_DELETE = 0xD3;    /* Delete on arrow keypad */
        public const byte DIK_LWIN = 0xDB;    /* Left Windows key */
        public const byte DIK_RWIN = 0xDC;    /* Right Windows key */
        public const byte DIK_APPS = 0xDD;    /* AppMenu key */
        public const byte DIK_POWER = 0xDE;    /* System Power */
        public const byte DIK_SLEEP = 0xDF;    /* System Sleep */
        public const byte DIK_WAKE = 0xE3;    /* System Wake */
        public const byte DIK_WEBSEARCH = 0xE5;    /* Web Search */
        public const byte DIK_WEBFAVORITES = 0xE6;    /* Web Favorites */
        public const byte DIK_WEBREFRESH = 0xE7;    /* Web Refresh */
        public const byte DIK_WEBSTOP = 0xE8;    /* Web Stop */
        public const byte DIK_WEBFORWARD = 0xE9;    /* Web Forward */
        public const byte DIK_WEBBACK = 0xEA;    /* Web Back */
        public const byte DIK_MYCOMPUTER = 0xEB;    /* My Computer */
        public const byte DIK_MAIL = 0xEC;    /* Mail */
        public const byte DIK_MEDIASELECT = 0xED;    /* Media Select */

        /*
         *  Alternate names for keys, to facilitate transition from DOS.
         */
        public const byte DIK_BACKSPACE = DIK_BACK;   /* backspace */
        public const byte DIK_NUMPADSTAR = DIK_MULTIPLY;   /* * on numeric keypad */
        public const byte DIK_LALT = DIK_LMENU;   /* left Alt */
        public const byte DIK_CAPSLOCK = DIK_CAPITAL;   /* CapsLock */
        public const byte DIK_NUMPADMINUS = DIK_SUBTRACT;   /* - on numeric keypad */
        public const byte DIK_NUMPADPLUS = DIK_ADD;   /* + on numeric keypad */
        public const byte DIK_NUMPADPERIOD = DIK_DECIMAL;   /* . on numeric keypad */
        public const byte DIK_NUMPADSLASH = DIK_DIVIDE;   /* / on numeric keypad */
        public const byte DIK_RALT = DIK_RMENU;   /* right Alt */
        public const byte DIK_UPARROW = DIK_UP;   /* UpArrow on arrow keypad */
        public const byte DIK_PGUP = DIK_PRIOR;   /* PgUp on arrow keypad */
        public const byte DIK_LEFTARROW = DIK_LEFT;   /* LeftArrow on arrow keypad */
        public const byte DIK_RIGHTARROW = DIK_RIGHT;   /* RightArrow on arrow keypad */
        public const byte DIK_DOWNARROW = DIK_DOWN;   /* DownArrow on arrow keypad */
        public const byte DIK_PGDN = DIK_NEXT;   /* PgDn on arrow keypad */

        /*
         *  Alternate names for keys originally not used on US keyboards.
         */
        public const byte DIK_CIRCUMFLEX = DIK_PREVTRACK; /* Japanese keyboard */


        #endregion

        public const int KBTABLE_ENTRY_COUNT = 100; //< key translation table maximum size, (arbitrary) most of the layouts are < 80 entries

        public const int WM_CREATE = 0x0001;
        public const int WM_KILLFOCUS = 0x0008;
        public const int WM_CLOSE = 0x0010;
        public const int WM_KEYDOWN = 0x0100;
        public const int WM_KEYUP = 0x0101;
        public const int WM_SYSKEYDOWN = 0x0104;
        public const int WM_SYSKEYUP = 0x0105;
        public const int WM_COMMAND = 0x0111;
        public const int WM_SYSCOMMAND = 0x0112;
        public const int WM_MOUSEMOVE = 0x0200;
        public const int WM_LBUTTONDOWN = 0x0201;
        public const int WM_LBUTTONUP = 0x0202;
        public const int WM_RBUTTONDOWN = 0x0204;
        public const int WM_RBUTTONUP = 0x0205;

        public const uint SC_KEYMENU = 0xF100;
        public const uint VK_LMENU = 0xA4;

        //Defines the start and end IDs for the dynamic menus
        public const int ID_DYNAMENU_START = 5000;
        public const int ID_DYNAMENU_END = 5100;

        public const int KEY_DOWN = 1;
        public const int KEY_UP = 0;

        public const byte EMU_RUNSTATE_RUNNING = 0;

        public const byte CAS = 1;
        public const byte WAV = 0;

        public const uint WRITEBUFFERSIZE = 0x1FFFF;

        public const uint GENERIC_READ = 0x80000000;
        public const uint GENERIC_WRITE = 0x40000000;

        public const uint FILE_BEGIN = 0;
        public const uint FILE_END = 2;

        public const byte RESET_NONE = 0;
        public const byte RESET_HARD = 2;

        public const int NOMODULE = 1;
        public const int NOTVCC = 2;

        public const int MENU_PARENT = 0;
        public const int MENU_CHILD = 1;
        public const int MENU_STANDALONE = 2;

        public const int PAK_MAX_MEM = 0x40000;

        public const int DSBCAPS_STATIC = 0x00000002;
        public const int DSBCAPS_LOCSOFTWARE = 0x00000008;
        public const int DSBCAPS_GLOBALFOCUS = 0x00008000;
        public const int DSBCAPS_GETCURRENTPOSITION2 = 0x00010000;

        public const int DSSCL_NORMAL = 0x00000001;

        public const long DDLOCK_SURFACEMEMORYPTR = 0x00000000L;     // default
        public const long DDLOCK_WAIT = 0x00000001L;

        public const int DEFAULT_WIDTH = 640;
        public const int DEFAULT_HEIGHT = 480;

        public const byte _192Lines = 0; //--Indexer
        public const byte _225Lines = 1;

        public const uint CREATE_ALWAYS = 2;
        public const uint OPEN_ALWAYS = 4;

        public const uint FILE_ATTRIBUTE_NORMAL = 0x00000080;
    }
}
