using VCCSharp.Models;
using VCCSharp.Modules;

namespace VCCSharp.Configuration
{
    public interface IConfiguration
    {
        void ShowDialog(IConfig state, ConfigModel model, JoystickModel left, JoystickModel right);
    }
}
