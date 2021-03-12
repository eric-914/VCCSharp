#include <string>

using namespace std;

static string ClipboardText;

extern "C" {
  __declspec(dllexport) unsigned char __cdecl ClipboardEmpty() {
    return ClipboardText.empty();
  }
}

extern "C" {
  __declspec(dllexport) char __cdecl PeekClipboard() {
    return ClipboardText[0]; // get the next key in the string
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl PopClipboard() {
    //TODO: Will get some <string> assertion failures if these 3 lines are combined into 1.
    size_t length = ClipboardText.length() - 1;
    string remaining = ClipboardText.substr(1, length);

    ClipboardText = remaining; //move to next key in string
  }
}

void SetClipboardText(string text) {
  ClipboardText = text;
}
