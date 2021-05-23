using VCCSharp.Models;

namespace VCCSharp.Configuration
{
    public interface IConfiguration
    {
        unsafe void ShowDialog(ConfigState* state, JoystickModel* left, JoystickModel* right);
    }
}
