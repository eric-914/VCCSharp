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

    public TestWindowViewModel CreateViewModel()
    {
        IDxManager manager = CreateManager();

        var model = CreateModel(manager);
        
        var joysticks = CreateJoystickPairViewModel(manager);

        return new TestWindowViewModel(model, joysticks);
    }

    private static JoystickPairViewModel CreateJoystickPairViewModel(IDxManager manager)
    {
        var left = CreateJoystickConfigurationViewModel(JoystickSides.Left, manager);
        var right = CreateJoystickConfigurationViewModel(JoystickSides.Right, manager);

        return new JoystickPairViewModel(left, right);
    }

    private static JoystickConfigurationViewModel CreateJoystickConfigurationViewModel(JoystickSides side, IDxManager manager)
    {
        int index = side == JoystickSides.Left ? 0 : 1;

        var source = CreateJoystickSourceViewModel(manager, index);

        return new JoystickConfigurationViewModel(side, source);
    }

    private static JoystickSourceViewModel CreateJoystickSourceViewModel(IDxManager manager, int index)
    {
        var model = CreateJoystickSourceModel(manager);
        var state = CreateJoystickStateViewModel(manager, index);

        return new JoystickSourceViewModel(model, state);
    }

    private static JoystickStateViewModel CreateJoystickStateViewModel(IDxManager manager, int index)
    {
        return new JoystickStateViewModel(manager, index);
    }

    private static JoystickSourceModel CreateJoystickSourceModel(IDxManager manager)
    {
        return new JoystickSourceModel(manager);
    }

    private static TestWindowModel CreateModel(IDxManager manager)
    {
        return new TestWindowModel(manager);
    }

    private static IDxManager CreateManager()
    {
        var dispatcher = CreateDispatcher();
        var runner = CreateThreadRunner(dispatcher);

        return CreateManager(runner);
    }

    private static IDxManager CreateManager(IThreadRunner runner)
    {
        IDxInput dxInput = DxFactory.Instance.CreateDxInput();

        return new DxManager(dxInput, runner);
    }

    private static IThreadRunner CreateThreadRunner(IDispatcher dispatcher)
    {
        var runner = new ThreadRunner(dispatcher);

        Application.Current.Exit += (_, _) => runner.Stop();

        return runner;
    }

    private static IDispatcher CreateDispatcher()
    {
        return new DispatcherWrapper(Application.Current.Dispatcher);
    }
}
