# VCCSharp / DirectX / COM / IDirectX[...] interfaces

__DirectX COM interfaces__
I'm using this file to record things I've learned while trying to access the DirectX COM interfaces

* COM interfaces have a defined GUID attached to them, and is required to make these interfaces bind properly.
* Apparently, these DirectX GUID's are "well known" to Windows.  Must be standard to Windows by now.
* Looks like a COM interface is really picky about the order of the available methods.
* Doesn't seem to care what you call an interface method, only that it is listed in the correct position in the interface.
* Seems ok regarding parameter definition if not used, which is why so many are just empty.
* Still not sure why ComInterfaceType.InterfaceIsIUnknown is required.
* Not sure why Microsoft would've deprecated ComInterfaceType.InterfaceIsDual, as it's supposedly the 'default'.

