#include <string>

#include "MenuCallbacks.h"

#include "PakInterfaceState.h"
#include "PakInterface.h"
#include "PakInterfaceDelegates.h"
#include "PakInterfaceModule.h"
#include "Emu.h"
#include "Cartridge.h"

typedef struct {
  char MenuName[512];
  int MenuId;
  int Type;
} Dmenu;

static Dmenu MenuItem[100];

static HMENU hMenu = NULL;
static HMENU hSubMenu[64];

static unsigned char MenuIndex = 0;

extern "C" {
  __declspec(dllexport) void __cdecl RefreshDynamicMenu(EmuState* emuState)
  {
    MENUITEMINFO mii;
    char menuTitle[32] = "Cartridge";
    unsigned char tempIndex = 0, index = 0;
    static HWND hOld = 0;
    int subMenuIndex = 0;

    if ((hMenu == NULL) || (emuState->WindowHandle != hOld)) {
      hMenu = GetMenu(emuState->WindowHandle);
    }
    else {
      DeleteMenu(hMenu, 3, MF_BYPOSITION);
    }

    hOld = emuState->WindowHandle;
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

    DrawMenuBar(emuState->WindowHandle);
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl DynamicMenuCallback(EmuState* emuState, char* menuName, int menuId, int type)
  {
    char temp[256] = "";

    //MenuId=0 Flush Buffer MenuId=1 Done 
    switch (menuId)
    {
    case MENU_FLUSH:
      MenuIndex = 0;

      DynamicMenuCallback(emuState, "Cartridge", ID_MENU_CARTRIDGE, MENU_PARENT);	//Recursion is fun
      DynamicMenuCallback(emuState, "Load Cart", ID_MENU_LOAD_CART, MENU_CHILD);

      sprintf(temp, "Eject Cart: ");
      strcat(temp, GetPakInterfaceState()->Modname);

      DynamicMenuCallback(emuState, temp, ID_MENU_EJECT_CART, MENU_CHILD);

      break;

    case MENU_DONE:
      RefreshDynamicMenu(emuState);
      break;

    case MENU_REFRESH:
      DynamicMenuCallback(emuState, NULL, MENU_FLUSH, IGNORE);
      DynamicMenuCallback(emuState, NULL, MENU_DONE, IGNORE);
      break;

    default:
      strcpy(MenuItem[MenuIndex].MenuName, menuName);

      MenuItem[MenuIndex].MenuId = menuId;
      MenuItem[MenuIndex].Type = type;

      MenuIndex++;

      break;
    }
  }
}

/*
* TODO: This exists because this is what the different plugins expect, but it requires the EmuState
*/
void DynamicMenuCallback(char* menuName, int menuId, int type)
{
  DynamicMenuCallback(GetEmuState(), menuName, menuId, type);
}

extern "C" {
  __declspec(dllexport) void __cdecl DynamicMenuActivated(EmuState* emuState, int menuItem)
  {
    switch (menuItem)
    {
    case ID_MENU_LOAD_CART:
      LoadPack();
      break;

    case ID_MENU_EJECT_CART:
      UnloadPack(emuState);
      break;

    default:
      if (HasConfigModule()) {
        //--Original code was passing an unsigned char, though the menu ids are integers
        InvokeConfigModule((unsigned char)(menuItem - ID_DYNAMENU_START));
      }

      break;
    }
  }
}
