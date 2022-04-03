using Ninject;
using VCCSharp.Menu;
using VCCSharp.Shared.Dx;

namespace VCCSharp.IoC;

/// <summary>
/// Dependency injection services
/// </summary>
public interface IFactory
{
    IFactory Bind<TInterface, TClass>() where TClass : class, TInterface;
    IFactory Bind<TInterface>(Func<TInterface> fn);

    IFactory Singleton<TInterface, TClass>() where TClass : class, TInterface;
    IFactory Singleton<TInterface, TClass>(TClass instance) where TClass : class, TInterface;

    TInterface Get<TInterface>();

    MainWindowCommands MainWindowCommands { get; }

    IFactory InitializeModules();
    IFactory InitializeDxManager();
}

/// <summary>
/// A wrapper around NInject to handle dependency injection
/// </summary>
public class Factory : IFactory
{
    public static Factory Instance { get; } = new();
    private readonly IKernel _kernel = new StandardKernel();

    public IFactory SelfBind()
    {
        _kernel.Bind<IFactory>().ToMethod(_ => Instance);

        return this;
    }

    public IFactory Bind<TInterface, TClass>() where TClass : class, TInterface
    {
        _kernel.Bind<TInterface>().To<TClass>();

        return this;
    }
    public IFactory Bind<TInterface>(Func<TInterface> fn) 
    {
        _kernel.Bind<TInterface>().ToMethod(_ => fn());

        return this;
    }

    public IFactory Singleton<TInterface, TClass>() where TClass : class, TInterface
    {
        _kernel.Bind<TInterface>().To<TClass>().InSingletonScope();

        return this;
    }

    public IFactory Singleton<TInterface,TClass>(TClass instance) where TClass : class, TInterface
    {
        _kernel.Bind<TInterface>().ToMethod(_ => instance);

        return this;
    }

    public TInterface Get<TInterface>()
    {
        return _kernel.Get<TInterface>();
    }

    public MainWindowCommands MainWindowCommands => _kernel.Get<MainWindowCommands>();

    public IFactory InitializeModules()
    {
        var modules = (Modules)Get<IModules>();
        modules.Initialize();

        return this;
    }

    public IFactory InitializeDxManager()
    {
        var modules = Get<IDxManager>();
        modules.Initialize();
        modules.EnumerateDevices();

        return this;
    }
}
