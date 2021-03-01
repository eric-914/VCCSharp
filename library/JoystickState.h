#pragma once

#include "JoystickModel.h"

typedef struct
{
  unsigned short StickValue;
  unsigned short LeftStickX;
  unsigned short LeftStickY;
  unsigned short RightStickX;
  unsigned short RightStickY;

  unsigned char LeftButton1Status;
  unsigned char RightButton1Status;
  unsigned char LeftButton2Status;
  unsigned char RightButton2Status;
  unsigned char LeftStickNumber;
  unsigned char RightStickNumber;

  JoyStickModel Left;
  JoyStickModel Right;
} JoystickState;
