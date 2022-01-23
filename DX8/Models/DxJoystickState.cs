namespace DX8.Models
{
    internal class DxJoystickState : IDxJoystickState
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int[] Buttons { get; set; } = new int[2];
    }
}
