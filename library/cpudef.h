#pragma once

/*
Copyright 2015 by Joseph Forgione
This file is part of VCC (Virtual Color Computer).

    VCC (Virtual Color Computer) is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    VCC (Virtual Color Computer) is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with VCC (Virtual Color Computer).  If not, see <http://www.gnu.org/licenses/>.
*/

typedef struct
{
  void (*CPUInit)(void);
  int  (*CPUExec)(int);
  void (*CPUReset)(void);
  void (*CPUAssertInterrupt)(unsigned char, unsigned char);
  void (*CPUDeAssertInterrupt)(unsigned char);
  void (*CPUForcePC)(unsigned short);
} CPU;

extern "C" __declspec(dllexport) CPU* __cdecl GetCPU();

extern "C" __declspec(dllexport) void __cdecl SetCPUToHD6309();
extern "C" __declspec(dllexport) void __cdecl SetCPUToMC6809();

extern "C" __declspec(dllexport) void __cdecl CPUAssertInterrupt(unsigned char irq, unsigned char flag);
extern "C" __declspec(dllexport) void __cdecl CPUDeAssertInterrupt(unsigned char irq);
extern "C" __declspec(dllexport) int __cdecl CPUExec(int cycle);
extern "C" __declspec(dllexport) void __cdecl CPUForcePC(unsigned short xferAddress);
extern "C" __declspec(dllexport) void __cdecl CPUInit();
extern "C" __declspec(dllexport) void __cdecl CPUReset();
