#include <string>

#include "Clipboard.h"
#include "Config.h"
#include "Keyboard.h"
#include "Graphics.h"
#include "TC1014MMU.h"

#include "ClipboardText.h"

ClipboardState* InitializeInstance(ClipboardState*);

static ClipboardState* instance = InitializeInstance(new ClipboardState());

extern "C" {
  __declspec(dllexport) ClipboardState* __cdecl GetClipboardState() {
    return instance;
  }
}

ClipboardState* InitializeInstance(ClipboardState* p) {
  p->PasteWithNew = false;

  return p;
}

extern "C" {
  __declspec(dllexport) int __cdecl GetCurrentKeyMap() {
    return instance->CurrentKeyMap;
  }
}

/*
* Internal, can't expose C++/string outside "C" .dll
*/
string GetClipboardText()
{
  if (!OpenClipboard(nullptr)) {
    MessageBox(0, "Unable to open clipboard.", "Clipboard", 0);

    return("");
  }

  HANDLE hClip = GetClipboardData(CF_TEXT);

  if (hClip == nullptr) {
    CloseClipboard();

    MessageBox(0, "No text found in clipboard.", "Clipboard", 0);

    return("");
  }

  char* tmp = static_cast<char*>(GlobalLock(hClip));

  if (tmp == nullptr) {
    CloseClipboard();

    MessageBox(0, "NULL Pointer", "Clipboard", 0);

    return("");
  }

  string out(tmp);

  GlobalUnlock(hClip);
  CloseClipboard();

  return out;
}

extern "C" {
  __declspec(dllexport) bool __cdecl SetClipboard(const char* clipout) {
    const size_t len = strlen(clipout) + 1;
    HGLOBAL hMem = GlobalAlloc(GMEM_MOVEABLE, len);

    if (hMem == 0) {
      MessageBox(0, "Failed to access clipboard.", "Clipboard", 0);
      return false;
    }

    memcpy(GlobalLock(hMem), clipout, len);

    GlobalUnlock(hMem);
    OpenClipboard(0);
    EmptyClipboard();
    SetClipboardData(CF_TEXT, hMem);
    CloseClipboard();

    return TRUE;
  }
}

unsigned char GetScanCode(char letter) {
  switch (letter)
  {
  case '@': return 0x03;
  case 'A': return 0x1E;
  case 'B': return 0x30;
  case 'C': return 0x2E;
  case 'D': return 0x20;
  case 'E': return 0x12;
  case 'F': return 0x21;
  case 'G': return 0x22;
  case 'H': return 0x23;
  case 'I': return 0x17;
  case 'J': return 0x24;
  case 'K': return 0x25;
  case 'L': return 0x26;
  case 'M': return 0x32;
  case 'N': return 0x31;
  case 'O': return 0x18;
  case 'P': return 0x19;
  case 'Q': return 0x10;
  case 'R': return 0x13;
  case 'S': return 0x1F;
  case 'T': return 0x14;
  case 'U': return 0x16;
  case 'V': return 0x2F;
  case 'W': return 0x11;
  case 'X': return 0x2D;
  case 'Y': return 0x15;
  case 'Z': return 0x2C;
  case ' ': return 0x39;
  case 'a': return 0x1E;
  case 'b': return 0x30;
  case 'c': return 0x2E;
  case 'd': return 0x20;
  case 'e': return 0x12;
  case 'f': return 0x21;
  case 'g': return 0x22;
  case 'h': return 0x23;
  case 'i': return 0x17;
  case 'j': return 0x24;
  case 'k': return 0x25;
  case 'l': return 0x26;
  case 'm': return 0x32;
  case 'n': return 0x31;
  case 'o': return 0x18;
  case 'p': return 0x19;
  case 'q': return 0x10;
  case 'r': return 0x13;
  case 's': return 0x1F;
  case 't': return 0x14;
  case 'u': return 0x16;
  case 'v': return 0x2F;
  case 'w': return 0x11;
  case 'x': return 0x2D;
  case 'y': return 0x15;
  case 'z': return 0x2C;
  case '0': return 0x0B;
  case '1': return 0x02;
  case '2': return 0x03;
  case '3': return 0x04;
  case '4': return 0x05;
  case '5': return 0x06;
  case '6': return 0x07;
  case '7': return 0x08;
  case '8': return 0x09;
  case '9': return 0x0A;
  case '!': return 0x02;
  case '#': return 0x04;
  case '$': return 0x05;
  case '%': return 0x06;
  case '^': return 0x07;
  case '&': return 0x08;
  case '*': return 0x09;
  case '(': return 0x0A;
  case ')': return 0x0B;
  case '-': return 0x0C;
  case '=': return 0x0D;
  case ';': return 0x27;
  case '\'': return 0x28;
  case '/': return 0x35;
  case '.': return 0x34;
  case ',': return 0x33;
  case '\n': return 0x1C;
  case '+': return 0x0D;
  case ':': return 0x27;
  case '\"': return 0x28;
  case '?': return 0x35;
  case '<': return 0x33;
  case '>': return 0x34;
  case '[': return 0x1A;
  case ']': return 0x1B;
  case '{': return 0x1A;
  case '}': return 0x1B;
  case '\\"': return 0x2B;
  case '|': return 0x2B;
  case '`': return 0x29;
  case '~': return 0x29;
  case '_': return 0x0C;
  case 0x09: return 0x39;  // TAB
  default: return 0xFF;
  }
}

BOOL GetCSHIFT(char letter) {
  switch (letter)
  {
  case '@': return TRUE;
  case 'A': return TRUE;
  case 'B': return TRUE;
  case 'C': return TRUE;
  case 'D': return TRUE;
  case 'E': return TRUE;
  case 'F': return TRUE;
  case 'G': return TRUE;
  case 'H': return TRUE;
  case 'I': return TRUE;
  case 'J': return TRUE;
  case 'K': return TRUE;
  case 'L': return TRUE;
  case 'M': return TRUE;
  case 'N': return TRUE;
  case 'O': return TRUE;
  case 'P': return TRUE;
  case 'Q': return TRUE;
  case 'R': return TRUE;
  case 'S': return TRUE;
  case 'T': return TRUE;
  case 'U': return TRUE;
  case 'V': return TRUE;
  case 'W': return TRUE;
  case 'X': return TRUE;
  case 'Y': return TRUE;
  case 'Z': return TRUE;
  case ' ': return FALSE;
  case 'a': return FALSE;
  case 'b': return FALSE;
  case 'c': return FALSE;
  case 'd': return FALSE;
  case 'e': return FALSE;
  case 'f': return FALSE;
  case 'g': return FALSE;
  case 'h': return FALSE;
  case 'i': return FALSE;
  case 'j': return FALSE;
  case 'k': return FALSE;
  case 'l': return FALSE;
  case 'm': return FALSE;
  case 'n': return FALSE;
  case 'o': return FALSE;
  case 'p': return FALSE;
  case 'q': return FALSE;
  case 'r': return FALSE;
  case 's': return FALSE;
  case 't': return FALSE;
  case 'u': return FALSE;
  case 'v': return FALSE;
  case 'w': return FALSE;
  case 'x': return FALSE;
  case 'y': return FALSE;
  case 'z': return FALSE;
  case '0': return FALSE;
  case '1': return FALSE;
  case '2': return FALSE;
  case '3': return FALSE;
  case '4': return FALSE;
  case '5': return FALSE;
  case '6': return FALSE;
  case '7': return FALSE;
  case '8': return FALSE;
  case '9': return FALSE;
  case '!': return TRUE;
  case '#': return TRUE;
  case '$': return TRUE;
  case '%': return TRUE;
  case '^': return TRUE;
  case '&': return TRUE;
  case '*': return TRUE;
  case '(': return TRUE;
  case ')': return TRUE;
  case '-': return FALSE;
  case '=': return FALSE;
  case ';': return FALSE;
  case '\'': return FALSE;
  case '/': return FALSE;
  case '.': return FALSE;
  case ',': return FALSE;
  case '\n': return FALSE;
  case '+': return TRUE;
  case ':': return TRUE;
  case '\"': return TRUE;
  case '?': return TRUE;
  case '<': return TRUE;
  case '>': return TRUE;
  case '[': return FALSE;
  case ']': return FALSE;
  case '{': return TRUE;
  case '}': return TRUE;
  case '\\"': return FALSE;
  case '|': return TRUE;
  case '`': return FALSE;
  case '~': return TRUE;
  case '_': return TRUE;
  case 0x09: return FALSE; // TAB
  default: return FALSE;
  }
}

BOOL GetLCNTRL(char letter) {
  switch (letter)
  {
  case '@': return FALSE;
  case 'A': return FALSE;
  case 'B': return FALSE;
  case 'C': return FALSE;
  case 'D': return FALSE;
  case 'E': return FALSE;
  case 'F': return FALSE;
  case 'G': return FALSE;
  case 'H': return FALSE;
  case 'I': return FALSE;
  case 'J': return FALSE;
  case 'K': return FALSE;
  case 'L': return FALSE;
  case 'M': return FALSE;
  case 'N': return FALSE;
  case 'O': return FALSE;
  case 'P': return FALSE;
  case 'Q': return FALSE;
  case 'R': return FALSE;
  case 'S': return FALSE;
  case 'T': return FALSE;
  case 'U': return FALSE;
  case 'V': return FALSE;
  case 'W': return FALSE;
  case 'X': return FALSE;
  case 'Y': return FALSE;
  case 'Z': return FALSE;
  case ' ': return FALSE;
  case 'a': return FALSE;
  case 'b': return FALSE;
  case 'c': return FALSE;
  case 'd': return FALSE;
  case 'e': return FALSE;
  case 'f': return FALSE;
  case 'g': return FALSE;
  case 'h': return FALSE;
  case 'i': return FALSE;
  case 'j': return FALSE;
  case 'k': return FALSE;
  case 'l': return FALSE;
  case 'm': return FALSE;
  case 'n': return FALSE;
  case 'o': return FALSE;
  case 'p': return FALSE;
  case 'q': return FALSE;
  case 'r': return FALSE;
  case 's': return FALSE;
  case 't': return FALSE;
  case 'u': return FALSE;
  case 'v': return FALSE;
  case 'w': return FALSE;
  case 'x': return FALSE;
  case 'y': return FALSE;
  case 'z': return FALSE;
  case '0': return FALSE;
  case '1': return FALSE;
  case '2': return FALSE;
  case '3': return FALSE;
  case '4': return FALSE;
  case '5': return FALSE;
  case '6': return FALSE;
  case '7': return FALSE;
  case '8': return FALSE;
  case '9': return FALSE;
  case '!': return FALSE;
  case '#': return FALSE;
  case '$': return FALSE;
  case '%': return FALSE;
  case '^': return FALSE;
  case '&': return FALSE;
  case '*': return FALSE;
  case '(': return FALSE;
  case ')': return FALSE;
  case '-': return FALSE;
  case '=': return FALSE;
  case ';': return FALSE;
  case '\'': return FALSE;
  case '/': return FALSE;
  case '.': return FALSE;
  case ',': return FALSE;
  case '\n': return FALSE;
  case '+': return FALSE;
  case ':': return FALSE;
  case '\"': return FALSE;
  case '?': return FALSE;
  case '<': return FALSE;
  case '>': return FALSE;
  case '[': return TRUE;
  case ']': return TRUE;
  case '{': return FALSE;
  case '}': return FALSE;
  case '\\"': return TRUE;
  case '|': return FALSE;
  case '`': return FALSE;
  case '~': return FALSE;
  case '_': return FALSE;
  case 0x09: return FALSE; // TAB
  default: return FALSE;
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl PasteText() {
    string clipparse, lines, debugout, tmp;
    unsigned char sc;
    char letter;
    BOOL CSHIFT;
    BOOL LCNTRL;

    int GraphicsMode = GetGraphicsState()->GraphicsMode;

    if (GraphicsMode != 0) {
      int tmp = MessageBox(0, "Warning: You are not in text mode. Continue Pasting?", "Clipboard", MB_YESNO);

      if (tmp != 6) {
        return;
      }
    }

    SetPaste(true);

    //This sets the keyboard to Natural,
    //but we need to read it first so we can set it back
    instance->CurrentKeyMap = GetCurrentKeyboardLayout();

    vccKeyboardBuildRuntimeTable(1);

    string cliptxt = GetClipboardText().c_str();

    if (instance->PasteWithNew) {
      cliptxt = "NEW\n" + cliptxt;
    }

    for (int t = 0; t < (int)cliptxt.length(); t++) {
      char tmp = cliptxt[t];

      if (tmp != (char)'\n') {
        lines += tmp;
      }
      else { //...the character is a <CR>
        if (lines.length() > 249 && lines.length() < 257 && instance->CodePaste == TRUE) {
          size_t b = lines.find(" ");
          string main = lines.substr(0, 249);
          string extra = lines.substr(249, lines.length() - 249);
          string spaces;

          for (int p = 1; p < 249; p++) {
            spaces.append(" ");
          }

          string linestr = lines.substr(0, b);

          lines = main + "\n\nEDIT " + linestr + "\n" + spaces + "I" + extra + "\n";
          clipparse += lines;
          lines.clear();
        }

        if (lines.length() >= 257 && instance->CodePaste == TRUE) {
          // Line is too long to handle. Truncate.
          size_t b = lines.find(" ");
          string linestr = "Warning! Line " + lines.substr(0, b) + " is too long for BASIC and will be truncated.";

          MessageBox(0, linestr.c_str(), "Clipboard", 0);

          lines = (lines.substr(0, 249));
        }

        if (lines.length() <= 249 || instance->CodePaste == false) {
          // Just a regular line.
          clipparse += lines + "\n";
          lines.clear();
        }
      }

      if (t == cliptxt.length() - 1) {
        clipparse += lines;
      }
    }

    cliptxt = clipparse;

    string out;

    for (int pp = 0; pp <= (int)cliptxt.size(); pp++) {
      letter = cliptxt[pp];

      sc = GetScanCode(letter);
      CSHIFT = GetCSHIFT(letter);
      LCNTRL = GetLCNTRL(letter);

      if (CSHIFT) { out += 0x36; CSHIFT = FALSE; }
      if (LCNTRL) { out += 0x1D; LCNTRL = FALSE; }

      out += sc;
    }

    SetClipboardText(out.c_str());
  }
}

extern "C" {
  __declspec(dllexport) void CopyText() {
    int idx;
    int tmp;
    int lines;
    int offset;
    int lastchar;
    string out;
    string tmpline;

    GraphicsState* graphicsState = GetGraphicsState();

    int bytesPerRow = graphicsState->BytesperRow;
    int graphicsMode = graphicsState->GraphicsMode;
    unsigned int screenstart = graphicsState->StartofVidram;

    if (graphicsMode != 0) {
      MessageBox(0, "ERROR: Graphics screen can not be copied.\nCopy can ONLY use a hardware text screen.", "Clipboard", 0);

      return;
    }

    lines = bytesPerRow == 32 ? 15 : 23;

    string dbug = "StartofVidram is: " + to_string(screenstart) + "\nGraphicsMode is: " + to_string(graphicsMode) + "\n";

    OutputDebugString(dbug.c_str());

    // Read the lo-res text screen...
    if (bytesPerRow == 32) {
      offset = 0;

      char pcchars[] =
      {
        '@','a','b','c','d','e','f','g',
        'h','i','j','k','l','m','n','o',
        'p','q','r','s','t','u','v','w',
        'x','y','z','[','\\',']',' ',' ',
        ' ','!','\"','#','$','%','&','\'',
        '(',')','*','+',',','-','.','/',
        '0','1','2','3','4','5','6','7',
        '8','9',':',';','<','=','>','?',
        '@','A','B','C','D','E','F','G',
        'H','I','J','K','L','M','N','O',
        'P','Q','R','S','T','U','V','W',
        'X','Y','Z','[','\\',']',' ',' ',
        ' ','!','\"','#','$','%','&','\'',
        '(',')','*','+',',','-','.','/',
        '0','1','2','3','4','5','6','7',
        '8','9',':',';','<','=','>','?',
        '@','a','b','c','d','e','f','g',
        'h','i','j','k','l','m','n','o',
        'p','q','r','s','t','u','v','w',
        'x','y','z','[','\\',']',' ',' ',
        ' ','!','\"','#','$','%','&','\'',
        '(',')','*','+',',','-','.','/',
        '0','1','2','3','4','5','6','7',
        '8','9',':',';','<','=','>','?'
      };

      for (int y = 0; y <= lines; y++) {
        lastchar = 0;
        tmpline.clear();
        tmp = 0;

        for (idx = 0; idx < bytesPerRow; idx++) {
          tmp = MemRead8(0x0400 + y * bytesPerRow + idx);

          if (tmp == 32 || tmp == 64 || tmp == 96) {
            tmp = 30 + offset;
          }
          else {
            lastchar = idx + 1;
          }

          tmpline += pcchars[tmp - offset];
        }

        tmpline = tmpline.substr(0, lastchar);

        if (lastchar != 0) {
          out += tmpline; out += "\n";
        }
      }

      if (out == "") {
        MessageBox(0, "No text found on screen.", "Clipboard", 0);
      }
    }
    else if (bytesPerRow == 40 || bytesPerRow == 80) {
      offset = 32;
      char pcchars[] =
      {
        ' ','!','\"','#','$','%','&','\'',
        '(',')','*','+',',','-','.','/',
        '0','1','2','3','4','5','6','7',
        '8','9',':',';','<','=','>','?',
        '@','A','B','C','D','E','F','G',
        'H','I','J','K','L','M','N','O',
        'P','Q','R','S','T','U','V','W',
        'X','Y','Z','[','\\',']',' ',' ',
        '^','a','b','c','d','e','f','g',
        'h','i','j','k','l','m','n','o',
        'p','q','r','s','t','u','v','w',
        'x','y','z','{','|','}','~','_',
        (char)'Ç',(char)'ü',(char)'é',(char)'â',(char)'ä',(char)'à',(char)'å',(char)'ç',
        (char)'ê',(char)'ë',(char)'è',(char)'ï',(char)'î',(char)'ß',(char)'Ä',(char)'Â',
        (char)'Ó',(char)'æ',(char)'Æ',(char)'ô',(char)'ö',(char)'ø',(char)'û',(char)'ù',
        (char)'Ø',(char)'Ö',(char)'Ü',(char)'§',(char)'£',(char)'±',(char)'º',(char)'¥',
        ' ',' ','!','\"','#','$','%','&',
        '\'','(',')','*','+',',','-','.',
        '/','0','1','2','3','4','5','6',
        '7','8','9',':',';','<','=','>',
        '?','@','A','B','C','D','E','F',
        'G','H','I','J','K','L','M','N',
        'O','P','Q','R','S','T','U','V',
        'W','X','Y','Z','[','\\',']',' ',
        ' ','^','a','b','c','d','e','f',
        'g','h','i','j','k','l','m','n',
        'o','p','q','r','s','t','u','v',
        'w','x','y','z','{','|','}','~','_'
      };

      for (int y = 0; y <= lines; y++) {
        lastchar = 0;
        tmpline.clear();
        tmp = 0;

        for (idx = 0; idx < bytesPerRow * 2; idx += 2) {
          tmp = GetMem(screenstart + y * (bytesPerRow * 2) + idx);

          if (tmp == 32 || tmp == 64 || tmp == 96) {
            tmp = offset;
          }
          else {
            lastchar = idx / 2 + 1;
          }

          tmpline += pcchars[tmp - offset];
        }

        tmpline = tmpline.substr(0, lastchar);

        if (lastchar != 0) {
          out += tmpline; out += "\n";
        }
      }
    }

    bool succ = SetClipboard(out.c_str());
  }
}
