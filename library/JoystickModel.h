#pragma once

typedef struct {
  // 0 -- Keyboard
  // 1 -- Mouse
  // 2 -- Audio 
  // 3 -- Joystick 
  unsigned char UseMouse;

  // Index of which Joystick is selected
  unsigned char DiDevice;

  // 0 -- Standard,
  // 1 -- TandyHiRes,
  // 2 -- CCMAX
  unsigned char HiRes;  //TODO: This doesn't seem bound to anything

  unsigned char Up;
  unsigned char Down;
  unsigned char Left;
  unsigned char Right;
  unsigned char Fire1;
  unsigned char Fire2;
} JoystickModel;
