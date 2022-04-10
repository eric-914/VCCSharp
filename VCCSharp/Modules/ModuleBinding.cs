﻿using VCCSharp.IoC;
using VCCSharp.Modules.TC1014;

namespace VCCSharp.Modules;

internal static class ModuleBinding
{
    public static IBinder Initialize(IBinder  binder)
    {
        binder
            .Singleton<IAudio, Audio>()
            .Singleton<ICPU, CPU>()
            .Singleton<ICassette, Cassette>()
            .Singleton<IClipboard, Clipboard>()
            .Singleton<ICoCo, CoCo>()
            .Singleton<IConfigurationManager, ConfigurationManager>()
            .Singleton<IDraw, Draw>()
            .Singleton<IEmu, Emu>()
            .Singleton<IEvents, Events>()
            .Singleton<IGraphics, Graphics>()
            .Singleton<IIOBus, IOBus>()
            .Singleton<IJoysticks, Joysticks>()
            .Singleton<IKeyboard, Keyboard>()
            .Singleton<IMC6821, MC6821>()
            .Singleton<IMenuCallbacks, MenuCallbacks>()
            .Singleton<IPAKInterface, PAKInterface>()
            .Singleton<IQuickLoad, QuickLoad>()
            .Singleton<ITC1014, TC1014.TC1014>()
            .Singleton<IThrottle, Throttle>()
            .Singleton<IVcc, Vcc>()
            ;

        return binder;
    }
}