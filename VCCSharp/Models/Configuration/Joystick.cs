namespace VCCSharp.Models.Configuration
{
    public class Joystick
    {
        public string Device { get; set; } = "";
        public bool UseMouse { get; set; } = true;

        public JoystickKeyMapping KeyMap { get; } = new JoystickKeyMapping();
    }
}