using VCCSharp.Modules;

namespace VCCSharp.IoC
{
    public interface IModules
    {
        IAudio Audio { get; }
        ICoCo CoCo { get; }
        IConfig Config { get; }
        IDirectDraw DirectDraw { get; }
        IEmu Emu { get; }
        IMenuCallbacks MenuCallbacks { get; }
        IQuickLoad QuickLoad { get; }
        IPAKInterface PAKInterface { get; }
        IResource Resource { get; }
        IVcc Vcc { get; }
    }

    public class Modules : IModules
    {
        private readonly IFactory _factory;

        public Modules(IFactory factory)
        {
            _factory = factory;
        }

        public IAudio Audio => _factory.Get<IAudio>();
        public ICoCo CoCo => _factory.Get<ICoCo>();
        public IConfig Config => _factory.Get<IConfig>();
        public IDirectDraw DirectDraw => _factory.Get<IDirectDraw>();
        public IEmu Emu => _factory.Get<IEmu>();
        public IMenuCallbacks MenuCallbacks => _factory.Get<IMenuCallbacks>();
        public IQuickLoad QuickLoad => _factory.Get<IQuickLoad>();
        public IPAKInterface PAKInterface => _factory.Get<IPAKInterface>();
        public IResource Resource => _factory.Get<IResource>();
        public IVcc Vcc => _factory.Get<IVcc>();
    }
}
