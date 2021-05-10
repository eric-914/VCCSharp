#pragma once

//CC Flag masks
#define E 7
#define F 6
#define H 5
#define I 4
#define N 3
#define Z 2
#define V 1
#define C 0

// MC6809 Vector table
#define VRESET	0xFFFE

#define PC_REG (instance->pc.Reg)
#define DP_REG (instance->dp.Reg)

#define CC_E instance->cc[E]
#define CC_F instance->cc[F]
#define CC_H instance->cc[H]
#define CC_I instance->cc[I]
#define CC_N instance->cc[N]
#define CC_Z instance->cc[Z]
#define CC_V instance->cc[V]
#define CC_C instance->cc[C]

#define PUR(_I) (*(instance->ureg8[_I]))
#define PXF(_I) (*(instance->xfreg16[_I]))

typedef struct {
  unsigned char ccbits;
  unsigned int cc[8];
} MC6809State;
