namespace VCCSharp.IoC;

/// <summary>
/// Dependency injection services
/// </summary>
public interface IFactory
{
    TInterface Get<TInterface>();
}
