﻿using VCCSharp.BitBanger;
using VCCSharp.Configuration;
using VCCSharp.TapePlayer;

namespace VCCSharp.IoC;

public interface IOptions
{
    IConfigurationWindowManager Configuration { get; }
    ITapePlayer TapePlayer { get; }
    IBitBanger BitBanger { get; }
}

public class Options : IOptions
{
    private readonly IFactory _factory;

    public Options(IFactory factory)
    {
        _factory = factory;
    }

    public IConfigurationWindowManager Configuration => _factory.Get<IConfigurationWindowManager>();
    public ITapePlayer TapePlayer => _factory.Get<ITapePlayer>();
    public IBitBanger BitBanger => _factory.Get<IBitBanger>();
}
