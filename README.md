# VCCSharp
__Virtual Color Computer in C#__

This is to be a port of the open source VCC ( https://github.com/VCCE/VCC ) into C#.

Plans are:
* Develop in .NET (Core) and WPF.
* To move all modules in the C library into C#
* To separate the U/I from the emulation as much as possible.

I've already moved all of VCC main into a C library, and have been able to attach my C# program to it.  Over time, I've been replacing parts of it in C#.  It's still following the original C-Module format for now, as I migrate it over to C#.

Eventually I'll try converting the other VCC modules as well.  If you build my [VCC](https://github.com/eric-914/VCC) fork, you can load the modules it generates into VCCSharp.

_coco3.rom_ can be found in [Resources](https://github.com/eric-914/VCCSharp/tree/main/resources/resources).  For now, copy it to the executable folder to run the emulator.

Markdown files (.md): 
https://www.markdownguide.org/getting-started
https://stackabuse.com/markdown-by-example