namespace DX8
{
    public interface IDxJoystickState
    {
        int X { get; }
        int Y { get; }
        int[] Buttons { get; }
    }
}