#pragma once

typedef unsigned char (*MEMREAD8)(unsigned short);
typedef void (*MEMWRITE8)(unsigned char, unsigned short);

typedef unsigned char (*PACKPORTREAD)(unsigned char);
typedef unsigned short (*MODULEAUDIOSAMPLE)(void);

typedef void (*ASSERTINTERRUPT) (unsigned char, unsigned char);
typedef void (*CONFIGIT)(unsigned char);
typedef void (*DMAMEMPOINTERS) (MEMREAD8, MEMWRITE8);
typedef void (*DYNAMICMENUCALLBACK)(char*, int, int);
typedef void (*GETNAME)(char*, char*, DYNAMICMENUCALLBACK);
typedef void (*HEARTBEAT) (void);
typedef void (*MODULERESET)(void);
typedef void (*MODULESTATUS)(char*);
typedef void (*PACKPORTWRITE)(unsigned char, unsigned char);
typedef void (*SETCART)(unsigned char);
typedef void (*SETCARTPOINTER)(SETCART);
typedef void (*SETINIPATH)(char*);
typedef void (*SETINTERRUPTCALLPOINTER) (ASSERTINTERRUPT);

typedef struct {
  unsigned char (*PakMemRead8)(unsigned short);
  unsigned char (*PakPortRead)(unsigned char);
  unsigned short (*ModuleAudioSample)(void);

  void (*ConfigModule)(unsigned char);
  void (*DmaMemPointer) (MEMREAD8, MEMWRITE8);
  void (*GetModuleName)(char*, char*, DYNAMICMENUCALLBACK);
  void (*HeartBeat)(void);
  void (*ModuleReset) (void);
  void (*ModuleStatus)(char*);
  void (*PakMemWrite8)(unsigned char, unsigned short);
  void (*PakPortWrite)(unsigned char, unsigned char);
  void (*PakSetCart)(SETCART);
  void (*SetIniPath) (char*);
  void (*SetInterruptCallPointer) (ASSERTINTERRUPT);
} PakInterfaceDelegates;
