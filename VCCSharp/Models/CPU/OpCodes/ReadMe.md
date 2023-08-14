# MC6809E/HT6309 OpCodes

<span style="font-family:Consolas">

## TABLE 4 - 8-BIT ACCUMULATOR AND MEMORY INSTRUCTIONS
| Mnemonic(s)     | Operation                                                                                
|:----------------|:-----------------------------------------------------------------------------------------
| ADCA, ADCB      | Add memory to accumulator with carry
| ADDA, ADDB      | Add memory to accumulator
| ANDA, ANDB      | And memory with accumulator
| ASL ASLA, ASLB  | Arithmetic shift of accumulator or memory left
| ASR, ASRA, ASRB | Arithmetic shift of accumulator or memory right
| BITA, BITB      | Bit test memory with accumulator
| CLR, CLRA, CLRB | Clear accumulator or memory location
| CMPA, CMPB      | Compare memory from accumulator
| COM, COMA, COMB | Complement accumulator or memory location DAA Decimal adjust A accumulator
| DEC, DECA, DECB | Decrement accumulator or memory location EORA, EORB Exclusive or memory with accumulator
| EXG R1, R2      | Exchange R1 with R2 (R1, R2 = A, B, CC, DP)
| INC, INCA, INCB | Increment accumulator or memory location
| LDA, LDB        | Load accumulator from memory
| LSL, LSLA, LSLB | Logical shift left accumulator or memory location
| LSR, LSRA, LSRB | Logical shift right accumulator or memory location
| MUL             | Unsigned multiply (Ax B — D)
| NEG, NEGA, NEGB | Negate accumulator or memory
| ORA, ORB        | Or memory with accumulator
| ROL, ROLA, ROLB | Rotate accumulator or memory left
| ROR, RORA, RORB | Rotate accumulator or memory right
| SBCA, SBCB      | Subtract memory from accumulator with borrow
| STA, STB        | Store accumulator to memory
| SUBA, SUBB      | Subtract memory from accumulator
| TST, TSTA, TSTB | Test accumulator or memory location
| TFR R1, R2      | Transfer R1 to R2 (R1, R2 = A, B, CC, DP)

NOTE: A, B, CC or DP may be pushed to (pulled from) either stack with PSHS, PSHU (PULS, PULU) instructions. 

## TABLE 5 - 16-BIT ACCUMULATOR AND MEMORY INSTRUCTIONS
| Mnemonic(s)     | Operation                                                                                
|:----------------|:-----------------------------------------------------------------------------------------
| ADDD            | Add memory to D accumulator 
| CMPD            | Compare memory from D accumulator 
| EXG D, R        | Exchange D with X, Y, S, U or PC
| LDD             | Load D accumulator from memory
| SEX             | Sign Extend B accumulator into A accumulator 
| STD             | Store D accumulator to memory
| SUBD            | Subtract memory from D accumulator 
| TFR D, R        | Transfer D to X, Y, S, U or PC
| TFR R, D        | Transfer X, Y, S, U or PC to D

NOTE: D may be pushed (pulled) to either stack with PSHS, PSHU (PULS, PULU) instructions. 

## TABLE 6 - INDEX REGISTER/ STACK POINTER INSTRUCTIONS
| Instruction     | Description                                                                                
|:----------------|:-----------------------------------------------------------------------------------------
| CMPS, CMPU      | Compare memory from stack pointer
| CMPX, CMPY      | Compare memory from index register 
| EXG Rl, R2      | Exchange D. X, Y, S. U or PC with D, X, Y, S, U or PC
| LEAS, LEAU      | Load effective address into stack pointer
| LEAX, LEAY      | Load effective address into index register 
| LDS, LDU        | Load stack pointer from memory
| LDX, LDY        | Load index register from memory
| PSHS            | Push A, B, CC, DP, D, X, Y, U, or PC onto hardware stack 
| PSHU            | Push A, B, CC, DP, D, X, Y, S, or PC onto user stack 
| PULS            | Pull A, B, CC, DP, D, X, Y, U or PC from hardware stack 
| PULU            | Pull A, B, CC, DP, D, X, Y, S or PC from hardware stack 
| STS,            | STU Store stack pointer to memory
| STX,            | STY Store index register to memory
| TFR R1, R2      | Transfer D, X, Y, S, U or PC to D, X, Y, S, U or PC
| ABX             | Add B accumulator to X (unsigned)

## TABLE 7 - BRANCH INSTRUCTIONS
|                     | Instruction     | Description                                                                                
|:----------------    |:----------------|:-----------------------------------------------------------------------------------------
| *SIMPLE BRANCHES*   |                 | 
|                     | BEQ, LBEQ       | Branch if equal
|                     | 8NE, LBNE       | Branch if not equal
|                     | BMI, LBMI       | Branch if minus 
|                     | BPL, LBPL       | Branch if plus 
|                     | BCS, LBCS       | Branch if carry set BCC. 
|                     | LBCC            | Branch if carry clear BVS. 
|                     | LBVS            | Branch if overflow set BVC, 
|                     | LBVC            | Branch if overflow clear 
| *SIGNED BRANCHES*   | 
|                     | BGT, LBGT       | Branch if greater (signed) BVS. 
|                     | LBVS            | Branch if invalid 2's complement result 
|                     | BGE, LBGE       | Branch if greater than or equal (signed)
|                     | BEQ, LBEQ       | Branch if equal
|                     | BNE, LBNE       | Branch if not equal
|                     | BLE, LBLE       | Branch if less than or equal (signed)
|                     | BVC, LBVC       | Branch if valid 2's complement result 
|                     | BLT, LBLT       | Branch if less than (signed)
| *UNSIGNED BRANCHES* | 
|                     | BHI, LBHI       | Branch if higher (unsigned)
|                     | BCC, LBCC       | Branch if higher or same (unsigned)
|                     | BHS, LBHS       | Branch if higher or same (unsigned)
|                     | BEQ, LBEQ       | Branch if equal
|                     | BNE, LBNE       | Branch if not equal
|                     | BLS, LBLS       | Branch if lower or same (unsigned)
|                     | BCS, LBCS       | Branch if lower (unsigned)
|                     | BLO, LBLO       | Branch if lower (unsigned)
| *OTHER BRANCHES*    |                 |
|                     | BSR, LBSR       | Branch to subroutine BRA, LBRA Branch always
|                     | BRN, LBRN       | Branch never

## TABLE 8 - MISCELLANEOUS INSTRUCTIONS
| Instruction     | Description                                                                                
|:----------------|:-----------------------------------------------------------------------------------------
| ANDCC           | AND condition code register 
| CWAI            | AND condition code register, then wait for interrupt
| NOP             | No operation
| ORCC            | OR condition code register 
| JMP             | Jump
| JSR             | Jump to subroutine
| RTI             | Return from interrupt 
| RTS             | Return from subroutine 
| SWI, SWI2, SWI3 | Software interrupt (absolute indirect) 
| SYNC            | Synchronize with interrupt line


## Machine Code to Instruction Cross Reference

<span style="font-size:13px;font-family:Courier">

|   |  0   |  1   |  2   |  3   |    4    |    5    |  6   |    7    |     8     |  9   |  A   |  B   |   C   |  D   |  E  |  F
|---|------|------|------|------|---------|---------|------|---------|-----------|------|------|------|-------|------|-----|------
| 0 |NEG  D|------|------|COM  D|LSR     D|---------|ROR  D|ASR     D|ASL,LSL   D|ROL  D|DEC  D|------|INC   D|TST  D|JMP D|CLR  D
| 1 |Page 2|Page3 |NOP  I|SYNC I|---------|---------|LBRA R|LBSR    R|-----------|DAA  I|ORCC M|------|ANDCC M|SEX  I|EXG M|TFR  M
| 2 |BRA  R|BRN  R|BHI  R|BLS  R|BHS,BCC R|BLO,BCS R|BNE  R|BEQ     R|BVC       R|BVS  R|BPL  R|BMI  R|BGE   R|BLT  R|BGT R|BLE  R
| 3 |LEAX X|LEAY X|LEAS X|LEAU X|PSHS    M|PULS    M|PSHU M|PULU    M|-----------|RTS  I|ABX  I|RTI  I|CWAI  I|MUL  I|-----|SWI  I
| 4 |NEGA I|------|------|COMA I|LSRA    I|---------|RORA I|ASRA    I|ASLA,LSLA I|ROLA I|DECA I|------|INCA  I|TSTA I|-----|CLRA I
| 5 |NEGB I|------|------|COMB I|LSRB    I|---------|RORB I|ASRB    I|ASLB,LSLB I|ROLB I|DECB I|------|INCB  I|TSTB I|-----|CLRB I
| 6 |NEG  X|------|------|COM  X|LSR     X|---------|ROR  X|ASR     X|ASL,LSL   X|ROL  X|DEC  X|------|INC   X|TST  X|JMP X|CLR  X
| 7 |NEG  E|------|------|COM  E|LSR     E|---------|ROR  E|ASR     E|ASL,LSL   E|ROL  E|DEC  E|------|INC   E|TST  E|JMP E|CLR  E
| 8 |SUBA M|CMPA M|SBCA M|SUBD M|ANDA    M|BITA    M|LDA  M|---------|EORA      M|ADCA M|ORA  M|ADDA M|CMPX  M|BSR  R|LDX M|------
| 9 |SUBA D|CMPA D|SBCA D|SUBD D|ANDA    D|BITA    D|LDA  D|STA     D|EORA      D|ADCA D|ORA  D|ADDA D|CMPX  D|JSR  D|LDX D|STX  D
| A |SUBA X|CMPA X|SBCA X|SUBD X|ANDA    X|BITA    X|LDA  X|STA     X|EORA      X|ADCA X|ORA  X|ADDA X|CMPX  X|JSR  X|LDX X|STX  X
| B |SUBA E|CMPA E|SBCA E|SUBD E|ANDA    E|BITA    E|LDA  E|STA     E|EORA      E|ADCA E|ORA  E|ADDA E|CMPX  E|JSR  E|LDX E|STX  E
| C |SUBB M|CMPB M|SBCB M|ADDD M|ANDB    M|BITB    M|LDB  M|---------|EORB      M|ADCB M|ORB  M|ADDB M|LDD   M|------|LDU M|------
| D |SUBB D|CMPB D|SBCB D|ADDD D|ANDB    D|BITB    D|LDB  D|STB D    |EORB      D|ADCB D|ORB  D|ADDB D|LDD   D|STD  D|LDU D|STU  D
| E |SUBB X|CMPB X|SBCB X|ADDD X|ANDB    X|BITB    X|LDB  X|STB X    |EORB      X|ADCB X|ORB  X|ADDB X|LDD   X|STD  X|LDU X|STU  X
| F |SUBB E|CMPB E|SBCB E|ADDD E|ANDB    E|BITB    E|LDB  E|STB E    |EORB      E|ADCB E|ORB  E|ADDB E|LDD   E|STD  E|LDU E|STU  E

</span>

**D** - DIRECT
**I** - INHERENT
**M** - IMMEDIATE
**X** - INDEXED
**R** - RELATIVE

</span>

## Sources
* [MC6809E Technical Specifications](https://ia803209.us.archive.org/33/items/Motorola_MC6809E_HMOS_8_Bit_Microprocessor_1984_Motorola/Motorola_MC6809E_HMOS_8_Bit_Microprocessor_1984_Motorola_text.pdf)
* [Machine Code to Instruction Cross Reference](https://www.maddes.net/m6809pm/appendix_c.htm)