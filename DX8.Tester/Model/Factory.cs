using System.Windows;
using VCCSharp.Configuration.Models;
using VCCSharp.Configuration.Options;
using VCCSharp.Shared.Dx;
using VCCSharp.Shared.Models;
using VCCSharp.Shared.Threading;
using VCCSharp.Shared.ViewModels;

namespace DX8.Tester.Model;

internal interface IFactory
{
    TestWindowViewModel CreateViewModel(IJoysticksConfiguration configuration);
}

internal class Factory : IFactory
{
    public static IFactory Instance { get; } = new Factory();

    public TestWindowViewModel CreateViewModel(IJoysticksConfiguration configuration)
    {
        IDxManager manager = CreateManager(configuration);

        JoystickPairViewModel joysticks = CreateJoystickPairViewModel(manager, configuration);

        return new TestWindowViewModel(joysticks);
    }

    private static JoystickPairViewModel CreateJoystickPairViewModel(IDxManager manager, IJoysticksConfiguration configuration)
    {
        JoystickIntervalViewModel interval = CreateJoystickIntervalViewModel(manager, configuration);

        var left = (LeftJoystickConfigurationViewModel)CreateJoystickConfigurationViewModel(manager, configuration, interval, JoystickSides.Left);
        var right = (RightJoystickConfigurationViewModel)CreateJoystickConfigurationViewModel(manager, configuration, interval, JoystickSides.Right);

        return new JoystickPairViewModel(left, right);
    }

    private static JoystickConfigurationViewModel CreateJoystickConfigurationViewModel(IDxManager manager, IJoysticksConfiguration configuration, JoystickIntervalViewModel interval, JoystickSides side)
    {
        JoystickSourceViewModel joystickSource = CreateJoystickSourceViewModel(manager, configuration, interval, side);
        KeyboardSourceViewModel keyboardSource = new();

        IJoystickConfiguration model = side == JoystickSides.Left ? configuration.Left : configuration.Right;

        return side == JoystickSides.Left
            ? new LeftJoystickConfigurationViewModel(model, joystickSource, keyboardSource)
            : new RightJoystickConfigurationViewModel(model, joystickSource, keyboardSource);
    }

    private static JoystickSourceViewModel CreateJoystickSourceViewModel(IDxManager manager, IJoysticksConfiguration configuration, JoystickIntervalViewModel interval, JoystickSides side)
    {
        JoystickSourceModel model = CreateJoystickSourceModel(side, manager, configuration);
        JoystickStateViewModel state = CreateJoystickStateViewModel(side, manager, configuration);

        return side == JoystickSides.Left
            ? new LeftJoystickSourceViewModel(model, state, interval)
            : new RightJoystickSourceViewModel(model, state, interval);
    }

    private static JoystickStateViewModel CreateJoystickStateViewModel(JoystickSides side, IDxManager manager, IJoysticksConfiguration configuration)
    {
        return side == JoystickSides.Left
         ? new LeftJoystickStateViewModel(manager, configuration)
         : new RightJoystickStateViewModel(manager, configuration);
    }

    private static JoystickIntervalViewModel CreateJoystickIntervalViewModel(IDxManager manager, IInterval configuration)
    {
        return new JoystickIntervalViewModel(configuration, manager);
    }

    private static JoystickSourceModel CreateJoystickSourceModel(JoystickSides side, IDxManager manager, IDeviceIndex device)
    {
        return new JoystickSourceModel(manager, device, side);
    }

    private static IDxManager CreateManager(IInterval configuration)
    {
        IDispatcher dispatcher = CreateDispatcher();
        IThreadRunner runner = CreateThreadRunner(dispatcher, configuration);

        return CreateManager(runner);
    }

    private static IDxManager CreateManager(IThreadRunner runner)
    {
        IDxInput dxInput = DxFactory.Instance.CreateDxInput();

        return new DxManager(dxInput, runner)
            .Initialize()
            .EnumerateDevices();
    }

    private static IThreadRunner CreateThreadRunner(IDispatcher dispatcher, IInterval configuration)
    {
        var runner = new ThreadRunner(dispatcher, configuration);

        Application.Current.Exit += (_, _) => runner.Stop();

        return runner;
    }

    private static IDispatcher CreateDispatcher()
    {
        return new DispatcherWrapper(Application.Current.Dispatcher);
    }
}
