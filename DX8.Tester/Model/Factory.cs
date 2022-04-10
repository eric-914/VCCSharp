using System.Windows;
using VCCSharp.Shared.Dx;
using VCCSharp.Shared.Enums;
using VCCSharp.Shared.Models;
using VCCSharp.Shared.Threading;
using VCCSharp.Shared.ViewModels;

namespace DX8.Tester.Model;

internal class Factory
{
    public static Factory Instance { get; } = new();

    public TestWindowViewModel CreateViewModel(IDxConfiguration configuration)
    {
        IDxManager manager = CreateManager(configuration);

        JoystickPairViewModel joysticks = CreateJoystickPairViewModel(manager, configuration);

        return new TestWindowViewModel(joysticks);
    }

    private static JoystickPairViewModel CreateJoystickPairViewModel(IDxManager manager, IDxConfiguration configuration)
    {
        JoystickIntervalViewModel interval = CreateJoystickIntervalViewModel(manager, configuration);

        JoystickConfigurationViewModel left = CreateJoystickConfigurationViewModel(manager, configuration, interval, JoystickSides.Left);
        JoystickConfigurationViewModel right = CreateJoystickConfigurationViewModel(manager, configuration, interval, JoystickSides.Right);

        return new JoystickPairViewModel(left, right);
    }

    private static JoystickConfigurationViewModel CreateJoystickConfigurationViewModel(IDxManager manager, IDxConfiguration configuration, JoystickIntervalViewModel interval, JoystickSides side)
    {
        JoystickSourceViewModel source = CreateJoystickSourceViewModel(manager, configuration, interval, side);

        return new JoystickConfigurationViewModel(side, source);
    }

    private static JoystickSourceViewModel CreateJoystickSourceViewModel(IDxManager manager, IDxConfiguration configuration, JoystickIntervalViewModel interval, JoystickSides side)
    {
        JoystickSourceModel model = CreateJoystickSourceModel(side, manager);
        JoystickStateViewModel state = CreateJoystickStateViewModel(manager, configuration, side);

        return new JoystickSourceViewModel(model, state, interval);
    }

    private static JoystickStateViewModel CreateJoystickStateViewModel(IDxManager manager, IDeviceIndex configuration, JoystickSides side)
    {
        return new JoystickStateViewModel(manager, configuration, side);
    }

    private static JoystickIntervalViewModel CreateJoystickIntervalViewModel(IDxManager manager, IInterval configuration)
    {
        return new JoystickIntervalViewModel(configuration, manager);
    }
    private static JoystickSourceModel CreateJoystickSourceModel(JoystickSides side, IDxManager manager)
    {
        return new JoystickSourceModel(manager, new NullDeviceIndex(), side);
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
