namespace VCCSharp.IoC;

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
    IBinder Singleton<TInterface>(Func<IBinder, TInterface> fn);

    IBinder Bind<TInterface, TClass>() where TClass : class, TInterface;
    IBinder Bind<TInterface>(Func<TInterface> fn);
}
