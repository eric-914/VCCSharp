// ReSharper disable InconsistentNaming
// ReSharper disable CommentTypo
namespace DX8;

public interface IDxJoystickState 
{
    int Horizontal { get; }
    int Vertical { get; }
    bool X { get; }
    bool Y { get; }
    bool A { get; }
    bool B { get; }

    /// <summary>
    /// Left/Back or Left Button (on top of controller)
    /// </summary>
    bool LB { get; }

    /// <summary>
    /// Right/Back or Right Button (on top of controller)
    /// </summary>
    bool RB { get; }

    /// <summary>
    /// Left-center circle button
    /// </summary>
    bool Back { get; }

    /// <summary>
    /// Right-center circle button
    /// </summary>
    bool Start { get; }
}
