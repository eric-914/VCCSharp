namespace DX8
{
    public interface IDxJoystickState
    {
        int X { get; }
        int Y { get; }
        int[] Buttons { get; }
    }

    public class DxJoystickState : IDxJoystickState
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int[] Buttons { get; set; } = new int[2];
    }
}
