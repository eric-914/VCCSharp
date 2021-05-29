#include <windows.h>

#include "PakInterface.h"
#include "PakInterfaceModule.h"

//Menu "Actions"
#define MENU_FLUSH   0
#define MENU_DONE    1
#define MENU_REFRESH 2

//Type 0= HEAD TAG 1= Slave Tag 2=StandAlone
#define	MENU_PARENT     0
#define MENU_CHILD      1
#define MENU_STANDALONE 2

#define ID_DYNAMENU_START 5000	//Defines the start and end IDs for the dynamic menus

#define ID_MENU_LOAD_CART  5001
#define ID_MENU_EJECT_CART 5002

#define ID_MENU_CARTRIDGE  6000

typedef struct {
  char MenuName[512];
  int MenuId;
  int Type;
} Dmenu;

static Dmenu MenuItem[100];

static HMENU hMenu = NULL;
static HMENU hSubMenu[64];

static unsigned char MenuIndex = 0;

static HWND WindowHandle = NULL;

extern "C" {
  __declspec(dllexport) void __cdecl SetMenuIndex(unsigned char value) {
    MenuIndex = value;
  }
}

extern "C" {
  __declspec(dllexport) unsigned char __cdecl GetMenuIndex() {
    return MenuIndex;
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl SetWindowHandle(HWND hWnd)
  {
    WindowHandle = hWnd;
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl RefreshDynamicMenu()
  {
    MENUITEMINFO mii;
    char menuTitle[32] = "Cartridge";
    unsigned char tempIndex = 0, index = 0;
    static HWND hOld = 0;
    int subMenuIndex = 0;

    if ((hMenu == NULL) || (WindowHandle != hOld)) {
      hMenu = GetMenu(WindowHandle);
    }
    else {
      DeleteMenu(hMenu, 3, MF_BYPOSITION);
    }

    hOld = WindowHandle;
    hSubMenu[subMenuIndex] = CreatePopupMenu();

    memset(&mii, 0, sizeof(MENUITEMINFO));

    mii.cbSize = sizeof(MENUITEMINFO);
    mii.fMask = MIIM_TYPE | MIIM_SUBMENU | MIIM_ID;
    mii.fType = MFT_STRING;
    mii.wID = 4999;
    mii.hSubMenu = hSubMenu[subMenuIndex];
    mii.dwTypeData = menuTitle;
    mii.cch = (UINT)strlen(menuTitle);

    InsertMenuItem(hMenu, 3, TRUE, &mii);

    subMenuIndex++;

    for (tempIndex = 0; tempIndex < MenuIndex; tempIndex++)
    {
      if (strlen(MenuItem[tempIndex].MenuName) == 0) {
        MenuItem[tempIndex].Type = MENU_STANDALONE;
      }

      //Create Menu item in title bar if no exist already
      switch (MenuItem[tempIndex].Type)
      {
      case MENU_PARENT:
        subMenuIndex++;

        hSubMenu[subMenuIndex] = CreatePopupMenu();

        memset(&mii, 0, sizeof(MENUITEMINFO));

        mii.cbSize = sizeof(MENUITEMINFO);
        mii.fMask = MIIM_TYPE | MIIM_SUBMENU | MIIM_ID;
        mii.fType = MFT_STRING;
        mii.wID = MenuItem[tempIndex].MenuId;
        mii.hSubMenu = hSubMenu[subMenuIndex];
        mii.dwTypeData = MenuItem[tempIndex].MenuName;
        mii.cch = (UINT)strlen(MenuItem[tempIndex].MenuName);

        InsertMenuItem(hSubMenu[0], 0, FALSE, &mii);

        break;

      case MENU_CHILD:
        memset(&mii, 0, sizeof(MENUITEMINFO));

        mii.cbSize = sizeof(MENUITEMINFO);
        mii.fMask = MIIM_TYPE | MIIM_ID;
        mii.fType = MFT_STRING;
        mii.wID = MenuItem[tempIndex].MenuId;
        mii.hSubMenu = hSubMenu[subMenuIndex];
        mii.dwTypeData = MenuItem[tempIndex].MenuName;
        mii.cch = (UINT)strlen(MenuItem[tempIndex].MenuName);

        InsertMenuItem(hSubMenu[subMenuIndex], 0, FALSE, &mii);

        break;

      case MENU_STANDALONE:
        memset(&mii, 0, sizeof(MENUITEMINFO));

        mii.cbSize = sizeof(MENUITEMINFO);
        mii.fMask = MIIM_TYPE | MIIM_ID;
        mii.fType = strlen(MenuItem[tempIndex].MenuName) == 0 ? MF_SEPARATOR : MFT_STRING;
        mii.wID = MenuItem[tempIndex].MenuId;
        mii.hSubMenu = hMenu;
        mii.dwTypeData = MenuItem[tempIndex].MenuName;
        mii.cch = (UINT)strlen(MenuItem[tempIndex].MenuName);

        InsertMenuItem(hSubMenu[0], 0, FALSE, &mii);

        break;
      }
    }

    DrawMenuBar(WindowHandle);
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl SetMenuItem(char* menuName, int menuId, int type) {
    strcpy(MenuItem[MenuIndex].MenuName, menuName);

    MenuItem[MenuIndex].MenuId = menuId;
    MenuItem[MenuIndex].Type = type;
  }
}
