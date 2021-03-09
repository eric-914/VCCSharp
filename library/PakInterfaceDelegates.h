#pragma once

typedef void (*DYNAMICMENUCALLBACK)(char*, int, int);
typedef void (*GETNAME)(char*, char*, DYNAMICMENUCALLBACK);
typedef void (*CONFIGIT)(unsigned char);
typedef void (*HEARTBEAT) (void);
typedef unsigned char (*PACKPORTREAD)(unsigned char);
typedef void (*PACKPORTWRITE)(unsigned char, unsigned char);
typedef void (*ASSERTINTERRUPT) (unsigned char, unsigned char);
typedef unsigned char (*MEMREAD8)(unsigned short);
typedef void (*SETCART)(unsigned char);
typedef void (*MEMWRITE8)(unsigned char, unsigned short);
typedef void (*MODULESTATUS)(char*);
typedef void (*DMAMEMPOINTERS) (MEMREAD8, MEMWRITE8);
typedef void (*SETCARTPOINTER)(SETCART);
typedef void (*SETINTERRUPTCALLPOINTER) (ASSERTINTERRUPT);
typedef unsigned short (*MODULEAUDIOSAMPLE)(void);
typedef void (*MODULERESET)(void);
typedef void (*SETINIPATH)(char*);

typedef struct {
  void (*GetModuleName)(char*, char*, DYNAMICMENUCALLBACK);
  void (*ConfigModule)(unsigned char);
  void (*SetInterruptCallPointer) (ASSERTINTERRUPT);
  void (*DmaMemPointer) (MEMREAD8, MEMWRITE8);
  void (*HeartBeat)(void);
  void (*PakPortWrite)(unsigned char, unsigned char);
  unsigned char (*PakPortRead)(unsigned char);
  void (*PakMemWrite8)(unsigned char, unsigned short);
  unsigned char (*PakMemRead8)(unsigned short);
  void (*ModuleStatus)(char*);
  unsigned short (*ModuleAudioSample)(void);
  void (*ModuleReset) (void);
  void (*SetIniPath) (char*);
  void (*PakSetCart)(SETCART);
} PakInterfaceDelegates;
