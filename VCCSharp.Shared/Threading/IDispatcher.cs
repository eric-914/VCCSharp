namespace VCCSharp.Shared.Threading;

/// <summary>
/// The Dispatcher is a Windows object, which isn't an available class.
/// So, let's wrap it behind an interface
/// </summary>
public interface IDispatcher
{
    void Invoke(Action callback);
}
