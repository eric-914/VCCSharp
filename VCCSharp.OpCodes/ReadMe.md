# OpCodes Library
The processing of the opcodes for the MC6809/HD6309 CPU is rather independent of the CPU state machine: {Registers, Memory, Interrupt Status}

As long as the CPU implements the required registers, memory access, and interrupt states, the opcodes will process just fine.

The HD6309 is an extension of the MC6809.  It includes all registers plus more and all memory access plus more.

## UTF-16/Documentation
UTF-16 characters are used in the opcode documentation.  This was built using font "Cascadia Mono".  Other fonts may not offer support for these characters.