namespace VCCSharp.Models.Configuration
{
    public class Joystick
    {
        public string Device { get; set; } = "";
        public bool UseMouse { get; set; } = true;

        public int Left { get; set; } = 5;
        public int Right { get; set; } = 7;
        public int Up { get; set; } = 9;
        public int Down { get; set; } = 3;
        public int Fire1 { get; set; } = 6;
        public int Fire2 { get; set; } = 28;
    }
}