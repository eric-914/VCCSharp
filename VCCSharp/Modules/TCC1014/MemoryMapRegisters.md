# Memory Map Registers
https://ia600502.us.archive.org/15/items/TandyServiceManualColorComputer3/coco3_text.pdf

This section itemizes the following registers:

* [I/O Control Register](#IOCR)
* [Chip Control Register](#CCR)
* [68B09E Vector Register](#VR)


## <a id="IOCR">I/O Control Registers</a>
+:----------------+:---:+:---:+
| FF00 - FF03     | PIA | IC5 |
+:----------------+:---:+:---:+

### FF00:
```
    BIT 0 = KEYBOARD ROW 1 and right joystick switch 1
    BIT 1 = KEYBOARD ROW 2 and left  joystick switch 1
    BIT 2 = KEYBOARD ROW 3 and right joystick switch 2
    BIT 3 = KEYBOARD ROW 4 and left  joystick switch 2
    BIT 4 = KEYBOARD ROW 5
    BIT 5 = KEYBOARD ROW 6
    BIT 6 = KEYBOARD ROW 7 
    BIT 7 = JOYSTICK COMPARISON INPUT
```

### FF01:
```
    BIT 0   Control of HSYNC (63.5us) Interrupt  0 = IRQ* to CPU Disabled
                                                 1 = IRQ* to CPU Enabled
    BIT 1   Control of Interrupt Polarity        0 = Flag set on the falling edge of HS
                                                 1 = Flag set on the rising edge of HS
    BIT 2   Normally 1:     0 = Changes FFOO to the data direction register
    BIT 3   SEL 1:          LSB of the two analog MUX select lines
    BIT 4   1 Always
    BIT 5   1 Always
    BIT 6   Not used
    BIT 7   Horizontal sync interrupt flag
```

### FF02:
```
    BIT 0 = KEYBOARD COLUMN 1
    BIT 1 = KEYBOARD COLUMN 2
    BIT 2 = KEYBOARD COLUMN 3
    BIT 3 = KEYBOARD COLUMN 4
    BIT 4 = KEYBOARD COLUMN 5
    BIT 5 = KEYBOARD COLUMN 6
    BIT 6 = KEYBOARD COLUMN 7 / RAM SIZE OUTPUT
    BIT 7 = KEYBOARD COLUMN 8
```

### FF03:
```
    BIT 0   Control of VSYNC (16.667ms) Interrupt  0 = IRQ* to CPU Disabled
                                                   1 = IRQ* to CPU Enabled
    BIT 1   Control of Interrupt Polarity          0 = sets flag on falling edge FS
                                                   1 = sets flag on rising edge FS
    BIT 2 = NORMALLY 1;    0 = changes FF02 to the data direction register
    BIT 3 = SEL 2:         MSB of the two analog MUX select lines
    BIT 4 = 1 Always
    BIT 5 = 1 Always
    BIT 6 = Not used
    BIT 7 = Field sync interrupt flag
```

+:----------------+:---:+:---:+
| FF20 - FF23     | PIA | IC4 |
+:----------------+:---:+:---:+

### FF20:
```
    BIT 0 = CASSETTE DATA INPUT
    BIT 1 = RS-232C DATA OUTPUT
    BIT 2 = 6 BIT D/A LSB
    BIT 3 = 6 BIT D/A
    BIT 4 = 6 BIT D/A
    BIT 5 = 6 BIT D/A
    BIT 6 = 6 BIT D/A
    BIT 7 = 6 BIT D/A MSB
```

### FF21:
```
    BIT 0   Control of the CD (RS-232C status) Interrupt   0 = FIRQ* to CPU Disabled
                                                           1 = FIRQ* to CPU Enabled
    BIT 1   Control of Interrupt Polarity                  0 = sets flag on falling edge CD
                                                           1 = sets flag on rising edge CD
    BIT 2   Normally 1:   0 = changes FF20 to the data direction register
    BIT 3   Cassette Motor Control:   0 = OFF   1 = ON
    BIT 4   1 Always
    BIT 5   1 Always
    BIT 6   Not Used
    BIT 7   CD Interrupt Flag
```

### FF22:
```
    BIT 0 = RS-232C DATA INPUT
    BIT 1 = SINGLE BIT SOUND OUTPUT
    BIT 2 = RAM SIZE INPUT
    BIT 3 = RGB Monitor Sensing INPUT      CSS
    BIT 4 = VDG CONTROL OUTPUT             GMO & UP PER/ LOWER CASE*
    BIT 5 = VDG CONTROL OUTPU              GM1 & INVERT
    BIT 6 = VDG CONTROL OUTPU              GM2
    BIT 7 = VDG CONTROL OUTPU              A*/G
```

### FF23:
```
    BIT 0   Control of the Cartridge Interrupt   0 = FIRQ* to CPU Disabled
                                                 1 = FIRQ* to CPU Enabled
    BIT 1   Control of Interrupt Polarity        0 = sets flag on falling edge CART*
                                                 1 = sets flag on rising edge CART*
    BIT 2 = Normally 1:     0 = changes FF22 to the data direction register
    BIT 3 = Sound Enable
    BIT 4 = 1 Always
    BIT 5 = 1 Always
    BIT 6 = Not used
    BIT 7 = Cartridge Interrupt Flag
```

#### FF40 - FFBF: Not used
```
Note: FF22, FF23 are duplicated in tcclOH (VC2645QC), and V.D.G Control Bit
(Bit 3 through Bit 7) affects this IC (TCC1014) only.
```

## <a id="CCR">Chip Control Registers</a>

+:----------------+:----:+:---:+
| FF90 - FFDF     | ACVC | IC6 |
+:----------------+:----:+:---:+

### FF90: Initialization Register (INITO)
```
    BIT 7 = COCO   1 = Color Computer 1 and 2 Compatible
    BIT 6 = M/P    1 = MMU enabled
    BIT 5 = IEN    1 = Chip IRQ output enabled
    BIT 4 = FEN    1 = Chip FIRQ output enabled
    BIT 3 = MC3    1 = DRAM at XFEXX is constant
    BIT 2 = MC2    1 = Standard SCS
    BIT 1 = MCI    ROM map control (See table below)
    BIT 0 = MCO    ROM map control (See table below)
```

+:---:+:---:+:---------------------------------:+
| MC1 | MC0 | ROM mapping                       |
+-----+-----+-----------------------------------+
| 0   | x   | 16K Internal, 16K External        |
+-----+-----+-----------------------------------+
| 1   | 0   | 32K Internal                      |
+-----+-----+-----------------------------------+
| 1   | 1   | 32K External (except for vectors) |
+-----+-----+-----------------------------------+

### FF91: Initialization Register 1 (INIT1)
```
    BIT 7	-
    BIT 6	-
    BIT 5 = TINS    Timer Input Select: 1 = 70 nsec / 0 = 63µsec
    BIT 4	- 
    BIT 3	-
    BIT 2	-
    BIT 1	-
    BIT 0 = TR      MMU Task Register Select
```

### FF92: Interrupt Request Enable Register (IRQENR)
```
    BIT 7	- 
    BIT 6	- 
    BIT 5 = TMR     Interrupt from Timer enabled
    BIT 4 = HBORD   Horizontal Border FIRQ enabled
    BIT 3 = VBORD   Vertical Border FIRQ enabled
    BIT 2 = EI2     Serial Data FIRQ enabled
    BIT 1 = Ell     Keyboard FIRQ enabled
    BIT 0 = EI0     Cartridge FIRQ enabled
```

### FF93: Fast Interrupt Request Enable Register (FIRQENR)
```
    BIT 7	- 
    BIT 6	- 
    BIT 5 = TMR     Interrupt from Timer enabled
    BIT 4 = HBORD   Horizontal Border FIRQ enabled
    BIT 3 = VBORD   Vertical Border FIRQ enabled
    BIT 2 = EI2     Serial Data FIRQ enabled
    BIT 1 = Ell     Keyboard FIRQ enabled
    BIT 0 = EI0     Cartridge FIRQ enabled
```

### FF94: Timer Most Significant Nibble<br>FF95: Timer Least Significant Byte
```
    TIMER:  This is a 12-bit interval timer. When a value is loaded into the
            MSB, the count is automatically begun. The input clock is either
            14 MHz or horizontal sync, as selected by TINS (bit 5 of FF91). As
            the count falls through zero, an interrupt is generated (if
            enabled), and the count is automatically reloaded.
```

#### FF96: Reserved<br>FF97: Reserved

### FF98: Video Mode Register
```
BIT 7 = BP      0 = alphanumeric, 1 = bit plane
BIT 6	-
BIT 5 = BPI     1 = Burst phase inverted
BIT 4 = MOCH    1 = monochrome (on composite)
BIT 3 = H50     1 = 50 Hz vertical sync
BIT 2 = LPR2    Lines per row (See table below)
BIT 1 = LPR1    Lines per row (See table below)
BIT 0 =	LPR0    Lines per row (See table below)
```

+:----:+:----:+:----:+:------------------------+:-------------------------+
| LPR2 | LPR1 | LPR0 | Lines per character row |                          |
+------+------+------+-------------------------+--------------------------+
| 0    | 0    | 0    | one                     | (Graphics modes)         |
+------+------+------+-------------------------+--------------------------+
| 0    | 0    | 1    | two                     | (CoCo 1 and CoCo 2 only) |
+------+------+------+-------------------------+--------------------------+
| 0    | 1    | 0    | three                   | (CoCo 1 and CoCo 2 only) |
+------+------+------+-------------------------+--------------------------+
| 0    | 1    | 1    | eight                   |                          |
+------+------+------+-------------------------+--------------------------+
| 1    | 0    | 0    | nine                    |                          |
+------+------+------+-------------------------+--------------------------+
| 1    | 0    | 1    | (reserved)              |                          |
+------+------+------+-------------------------+--------------------------+
| 1    | 1    | 0    | twelve                  | (CoCo 1 and CoCo 2 only) |
+------+------+------+-------------------------+--------------------------+
| 1    | 1    | 1    | (reserved)              |                          |
+------+------+------+-------------------------+--------------------------+

### FF99: Video Resolution Register
```
    BIT 7	-
    BIT 6 = LPF1    Lines per field (See table below)
    BIT 5 = LPF0    Lines per field
    BIT 4 = HRES2   Horizontal resolution (See Video resolution on page 17) <== TODO:
    BIT 3 = HRES1   Horizontal resolution
    BIT 2 = HRES0   Horizontal resolution
    BIT 1 = CRES1   Color resolution (See Video resolution)
    BIT 0 = CRES0   Color resolution
```

+:----:+:----:+:---------------:+
| LPF1 | LPF0 | Lines per field |
+------+------+-----------------+
| 0    | 0    | 192             |
+------+------+-----------------+
| 0    | 1    | 200             |
+------+------+-----------------+
| 1    | 0    | Reservered      |
+------+------+-----------------+
| 1    | 1    | 225             |
+------+------+-----------------+

### FF9A: Border Register 
(All bits are 0 for CoCo 1 and CoCo 2 compatibility).
```
    BIT 7   -
    BIT 6   -
    BIT 5 = RED1 	Most significant red bit
    BIT 4 = GRN1 	Most significant green bit
    BIT 3 = BLU1 	Most significant blue bit
    BIT 2 = RED0 	Least significant red bit
    BIT 1 = GRN0 	Least significant green bit
    BIT 0 = BLU0	Least significant blue bit
```

#### FF9B: Reserved

### FF9C: Vertical Scroll Register
```
    BIT 7   -
    BIT 6   -
    BIT 5   -
    BIT 4   -
    BIT 3 = VSC3   (Vert. Scroll)
    BIT 2 = VSC2
    BIT 1 = VSC1
    BIT 0 = VSC0
```
NOTE: In the CoCo mode, the VSC ' s must be initialized to OF hex.

### FF9D: Vertical Offset 1 Register
```
    BIT 7 = Y18   (Vert. Offset)
    BIT 6 = Y17
    BIT 5 = Y16
    BIT 4 = Y15
    BIT 3 = Y14
    BIT 2 = Y13
    BIT 1 = Y12
    BIT 0 = Yll
```

### FF9E: Vertical Offset Register
```
    BIT 7 = Y10   (Vert. Offset)
    BIT 6 = Y9
    BIT 5 = Y8
    BIT 4 = Y7
    BIT 3 = Y6
    BIT 2 = Y5
    BIT 1 = Y4
    BIT 0 = Y3
```

NOTE: In CoCo mode, Y15 - Y9 are not effective, and are controlled by<br>
SAM bits F6 - F0. Also in CoCo mode, Y18 - Y16 should be 1, all others 0.

### FF9F: Horizontal Offset Register
```
    BIT 7 = HVEN   Horizontal Virtual Enable
    BIT 6 = X6     Horizontal Offset address
    BIT 5 = X5     Horizontal Offset address
    BIT 4 = X4     Horizontal Offset address
    BIT 3 = X3     Horizontal Offset address
    BIT 2 = X2     Horizontal Offset address
    BIT 1 = X1     Horizontal Offset address
    BIT 0 = X0     Horizontal Offset address
```

NOTE: HVEN enables a horizontal screen width of 128 bytes regardless of<br>
the HRES bits and CRES bits selected. This will allow a "virtual" screen<br>
somewhat larger than the displayed screen. The user can move the "window"<br>
(the displayed screen) by means of the horizontal offset bits. In<br>
character mode, the screen width is 128 characters regardless of<br>
attribute (or 64, if double-wide is selected).

## <a id="VR">Vector Registers</a>
