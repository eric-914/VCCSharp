using Ninject;
using VCCSharp.Configuration;
using VCCSharp.DX8;
using VCCSharp.Modules;
using VCCSharp.Shared.Dx;

namespace VCCSharp.IoC;

/// <summary>
/// Dependency injection services
/// </summary>
public interface IFactory
{
    TInterface Get<TInterface>();
}

/// <summary>
/// Binding functionality is only required on startup
/// </summary>
public interface IBinder
{
    IBinder SelfBind();

    IBinder ModuleBind();
    IBinder ConfigurationBind();
    IBinder DxBind();
    IBinder WindowsLibraryBind();

    IBinder Singleton<TInterface, TClass>() where TClass : class, TInterface;
    IBinder Singleton<TInterface, TClass>(TClass instance) where TClass : class, TInterface;

    IBinder Bind<TInterface, TClass>() where TClass : class, TInterface;
    IBinder Bind<TInterface>(Func<TInterface> fn);

    IBinder InitializeModules();
    IBinder InitializeDxManager();
}

/// <summary>
/// A wrapper around NInject to handle dependency injection
/// </summary>
public class Factory : IFactory, IBinder
{
    // ReSharper disable once InconsistentNaming
    private static readonly Factory _instance = new();

    //--NInject IoC service
    private readonly IKernel _kernel = new StandardKernel(); 

    public static IFactory Instance => _instance;
    public static IBinder Binder => _instance;

    #region Factory

    public TInterface Get<TInterface>() => _kernel.Get<TInterface>();

    #endregion

    #region Binder

    public IBinder SelfBind() 
        => BindThis(() => _kernel.Bind<IFactory>().ToMethod(_ => Instance));

    public IBinder Bind<TInterface, TClass>() where TClass : class, TInterface 
        => BindThis(() => _kernel.Bind<TInterface>().To<TClass>());

    public IBinder Bind<TInterface>(Func<TInterface> fn) 
        => BindThis(() => _kernel.Bind<TInterface>().ToMethod(_ => fn()));

    public IBinder Singleton<TInterface, TClass>() where TClass : class, TInterface 
        => BindThis(() => _kernel.Bind<TInterface>().To<TClass>().InSingletonScope());

    public IBinder Singleton<TInterface,TClass>(TClass instance) where TClass : class, TInterface 
        => BindThis(() => _kernel.Bind<TInterface>().ToMethod(_ => instance));

    public IBinder InitializeModules() 
        => BindThis(() => ((Modules)Get<IModules>()).Initialize());

    public IBinder InitializeDxManager() 
        => BindThis(() => Get<IDxManager>().Initialize().EnumerateDevices());

    #region Binding Delegates

    public IBinder ModuleBind() 
        => ModuleBinding.Initialize(this);

    public IBinder ConfigurationBind() 
        => ConfigurationBinding.Initialize(this);

    public IBinder DxBind() 
        => BindThis(() => DxBinding.Initialize(this));

    public IBinder WindowsLibraryBind() 
        => BindThis(() => WindowsLibraryBinding.Initialize(this));

    #endregion

    private IBinder BindThis(Action action)
    {
        action();

        return this;
    }

    #endregion
}
