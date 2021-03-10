using Ninject;
using VCCSharp.Menu;

namespace VCCSharp.IoC
{
    public interface IFactory
    {
        IFactory Bind<TInterface, TClass>() where TClass : class, TInterface;
        IFactory Singleton<TInterface, TClass>() where TClass : class, TInterface;

        TInterface Get<TInterface>();

        MainMenu MenuItems { get; }
    }

    public class Factory : IFactory
    {
        public static Factory Instance { get; } = new Factory();

        private readonly IKernel _kernel = new StandardKernel();
        
        public IFactory SelfBind()
        {
            _kernel.Bind<IFactory>().ToMethod(context  => Instance);

            return this;
        }

        public IFactory Bind<TInterface, TClass>() where TClass : class, TInterface
        {
            _kernel.Bind<TInterface>().To<TClass>();

            return this;
        }

        public IFactory Singleton<TInterface, TClass>() where TClass : class, TInterface
        {
            _kernel.Bind<TInterface>().To<TClass>().InSingletonScope();

            return this;
        }

        public TInterface Get<TInterface>()
        {
            return _kernel.Get<TInterface>();
        }

        public MainMenu MenuItems => new MainMenu(new Actions());
    }
}
