using Ninject;

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
public interface IBinder : IFactory
{
    IBinder BindFactory();

    IBinder Include(Action<IBinder> action);
    IBinder Initialize(Action<IFactory> action);

    IBinder Singleton<TInterface, TClass>() where TClass : class, TInterface;
    IBinder Singleton<TInterface, TClass>(TClass instance) where TClass : class, TInterface;

    IBinder Bind<TInterface, TClass>() where TClass : class, TInterface;
    IBinder Bind<TInterface>(Func<TInterface> fn);
}

/// <summary>
/// A wrapper around NInject to handle dependency injection
/// </summary>
public class Factory : IBinder
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

    public IBinder BindFactory() 
        => BindThis(() => _kernel.Bind<IFactory>().ToMethod(_ => Instance));

    public IBinder Bind<TInterface, TClass>() where TClass : class, TInterface 
        => BindThis(() => _kernel.Bind<TInterface>().To<TClass>());

    public IBinder Bind<TInterface>(Func<TInterface> fn) 
        => BindThis(() => _kernel.Bind<TInterface>().ToMethod(_ => fn()));

    public IBinder Singleton<TInterface, TClass>() where TClass : class, TInterface 
        => BindThis(() => _kernel.Bind<TInterface>().To<TClass>().InSingletonScope());

    public IBinder Singleton<TInterface,TClass>(TClass instance) where TClass : class, TInterface 
        => BindThis(() => _kernel.Bind<TInterface>().ToMethod(_ => instance));

    #region Binding Delegates

    public IBinder Include(Action<IBinder> action)
    {
        action(this);
        return this;
    }

    public IBinder Initialize(Action<IFactory> action)
    {
        action(this);
        return this;
    }

    #endregion

    private IBinder BindThis(Action action)
    {
        action();

        return this;
    }

    #endregion
}
