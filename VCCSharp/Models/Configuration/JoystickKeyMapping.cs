using VCCSharp.Models.Configuration.Support;

namespace VCCSharp.Models.Configuration
{
    public class JoystickKeyMapping
    {
        //--Mainly just mapping 'default' to the number-pad.
        public char Left { get; set; } = '4';
        public char Right { get; set; } = '6';
        public char Up { get; set; } = '8';
        public char Down { get; set; } = '2';

        public JoystickButtons Buttons { get; } = new JoystickButtons();
    }
}