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
string GetClipboardText(HANDLE hClip)
{
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
