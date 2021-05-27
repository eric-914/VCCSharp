#pragma once

typedef struct
{
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
} JoystickState;
