using VCCSharp.Models;

namespace VCCSharp.Configuration
{
    public interface IConfiguration
    {
        unsafe void ShowDialog(ConfigState* state, ConfigModel* model, JoystickModel left, JoystickModel right);
    }
}
