// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo
namespace VCCSharp.Models.Keyboard.Definitions;

/// <summary>
/// DirectInput keyboard scan codes
/// Copyright (C) Microsoft.  All rights reserved.
/// </summary>
public static class DIK
{
    public const byte DIK_NONE = 0x00;
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
    public const byte DIK_MINUS = 0x0C;         /* - on main keyboard */
    public const byte DIK_EQUALS = 0x0D;
    public const byte DIK_BACK = 0x0E;          /* backspace */
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
    public const byte DIK_RETURN = 0x1C;        /* Enter on main keyboard */
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
    public const byte DIK_GRAVE = 0x29;         /* accent grave */
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
    public const byte DIK_PERIOD = 0x34;        /* . on main keyboard */
    public const byte DIK_SLASH = 0x35;         /* / on main keyboard */
    public const byte DIK_RSHIFT = 0x36;
    public const byte DIK_MULTIPLY = 0x37;      /* * on numeric keypad */
    public const byte DIK_LMENU = 0x38;         /* left Alt */
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
    public const byte DIK_SCROLL = 0x46;        /* Scroll Lock */
    public const byte DIK_NUMPAD7 = 0x47;
    public const byte DIK_NUMPAD8 = 0x48;
    public const byte DIK_NUMPAD9 = 0x49;
    public const byte DIK_SUBTRACT = 0x4A;      /* - on numeric keypad */
    public const byte DIK_NUMPAD4 = 0x4B;
    public const byte DIK_NUMPAD5 = 0x4C;
    public const byte DIK_NUMPAD6 = 0x4D;
    public const byte DIK_ADD = 0x4E;           /* + on numeric keypad */
    public const byte DIK_NUMPAD1 = 0x4F;
    public const byte DIK_NUMPAD2 = 0x50;
    public const byte DIK_NUMPAD3 = 0x51;
    public const byte DIK_NUMPAD0 = 0x52;
    public const byte DIK_DECIMAL = 0x53;       /* . on numeric keypad */
    public const byte DIK_OEM_102 = 0x56;       /* <> or \| on RT 102-key keyboard (Non-U.S.) */
    public const byte DIK_F11 = 0x57;
    public const byte DIK_F12 = 0x58;
    public const byte DIK_NUMPADEQUALS = 0x8D;  /* = on numeric keypad (NEC PC98) */
    public const byte DIK_AT = 0x91;            /*        (NEC PC98) */
    public const byte DIK_COLON = 0x92;         /*        (NEC PC98) */
    public const byte DIK_UNDERLINE = 0x93;     /*        (NEC PC98) */
    public const byte DIK_NUMPADENTER = 0x9C;   /* Enter on numeric keypad */
    public const byte DIK_RCONTROL = 0x9D;
    public const byte DIK_MUTE = 0xA0;          /* Mute */
    public const byte DIK_CALCULATOR = 0xA1;    /* Calculator */
    public const byte DIK_PLAYPAUSE = 0xA2;     /* Play / Pause */
    public const byte DIK_MEDIASTOP = 0xA4;     /* Media Stop */
    public const byte DIK_VOLUMEDOWN = 0xAE;    /* Volume - */
    public const byte DIK_VOLUMEUP = 0xB0;      /* Volume + */
    public const byte DIK_WEBHOME = 0xB2;       /* Web home */
    public const byte DIK_NUMPADCOMMA = 0xB3;   /* , on numeric keypad (NEC PC98) */
    public const byte DIK_DIVIDE = 0xB5;        /* / on numeric keypad */
    public const byte DIK_SYSRQ = 0xB7;
    public const byte DIK_RMENU = 0xB8;         /* right Alt */
    public const byte DIK_PAUSE = 0xC5;         /* Pause */
    public const byte DIK_HOME = 0xC7;          /* Home on arrow keypad */
    public const byte DIK_UP = 0xC8;            /* UpArrow on arrow keypad */
    public const byte DIK_PRIOR = 0xC9;         /* PgUp on arrow keypad */
    public const byte DIK_LEFT = 0xCB;          /* LeftArrow on arrow keypad */
    public const byte DIK_RIGHT = 0xCD;         /* RightArrow on arrow keypad */
    public const byte DIK_END = 0xCF;           /* End on arrow keypad */
    public const byte DIK_DOWN = 0xD0;          /* DownArrow on arrow keypad */
    public const byte DIK_NEXT = 0xD1;          /* PgDn on arrow keypad */
    public const byte DIK_INSERT = 0xD2;        /* Insert on arrow keypad */
    public const byte DIK_DELETE = 0xD3;        /* Delete on arrow keypad */
    public const byte DIK_LWIN = 0xDB;          /* Left Windows key */
    public const byte DIK_RWIN = 0xDC;          /* Right Windows key */
    public const byte DIK_APPS = 0xDD;          /* AppMenu key */

    /*
     *  Alternate names for keys, to facilitate transition from DOS.
     */
    public const byte DIK_BACKSPACE = DIK_BACK;         /* backspace */
    public const byte DIK_NUMPADSTAR = DIK_MULTIPLY;    /* * on numeric keypad */
    public const byte DIK_LALT = DIK_LMENU;             /* left Alt */
    public const byte DIK_CAPSLOCK = DIK_CAPITAL;       /* CapsLock */
    public const byte DIK_NUMPADMINUS = DIK_SUBTRACT;   /* - on numeric keypad */
    public const byte DIK_NUMPADPLUS = DIK_ADD;         /* + on numeric keypad */
    public const byte DIK_NUMPADPERIOD = DIK_DECIMAL;   /* . on numeric keypad */
    public const byte DIK_NUMPADSLASH = DIK_DIVIDE;     /* / on numeric keypad */
    public const byte DIK_RALT = DIK_RMENU;             /* right Alt */
    public const byte DIK_UPARROW = DIK_UP;             /* UpArrow on arrow keypad */
    public const byte DIK_PGUP = DIK_PRIOR;             /* PgUp on arrow keypad */
    public const byte DIK_LEFTARROW = DIK_LEFT;         /* LeftArrow on arrow keypad */
    public const byte DIK_RIGHTARROW = DIK_RIGHT;       /* RightArrow on arrow keypad */
    public const byte DIK_DOWNARROW = DIK_DOWN;         /* DownArrow on arrow keypad */
    public const byte DIK_PGDN = DIK_NEXT;              /* PgDn on arrow keypad */
}
