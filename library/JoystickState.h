#pragma once

#include "JoystickModel.h"

typedef struct
{
  unsigned short StickValue;

  unsigned char LeftStickNumber;
  unsigned char LeftButton1Status;
  unsigned char LeftButton2Status;
  unsigned short LeftStickX;
  unsigned short LeftStickY;

  unsigned char RightStickNumber;
  unsigned char RightButton1Status;
  unsigned char RightButton2Status;
  unsigned short RightStickX;
  unsigned short RightStickY;

  JoystickModel* Left;
  JoystickModel* Right;
} JoystickState;
