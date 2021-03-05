#pragma once

#include "EmuState.h"

//Menu "Actions"
#define MENU_FLUSH   0
#define MENU_DONE    1
#define MENU_REFRESH 2

//Type 0= HEAD TAG 1= Slave Tag 2=StandAlone
#define	MENU_PARENT     0
#define MENU_CHILD      1
#define MENU_STANDALONE 2

#define ID_DYNAMENU_START 5000	//Defines the start and end IDs for the dynamic menus
#define ID_DYNAMENU_END   5100

#define ID_MENU_LOAD_CART  5001
#define ID_MENU_EJECT_CART 5002

#define ID_MENU_CARTRIDGE  6000

void DynamicMenuCallback(char* menuName, int menuId, int type);

extern "C" __declspec(dllexport) void __cdecl DynamicMenuActivated(EmuState*, int);
extern "C" __declspec(dllexport) void __cdecl DynamicMenuCallback(EmuState*, char*, int, int);
extern "C" __declspec(dllexport) void __cdecl RefreshDynamicMenu(EmuState*);
