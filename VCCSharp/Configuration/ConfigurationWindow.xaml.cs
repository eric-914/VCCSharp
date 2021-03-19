using System.Diagnostics;

namespace VCCSharp.Configuration
{
    public partial class ConfigurationWindow
    {
        public ConfigurationWindow(ConfigurationViewModel model) 
        {
            Debug.WriteLine("ConfigurationWindow()");
            DataContext = model;
            InitializeComponent();
            Debug.WriteLine("ConfigurationWindow(model)");
        }
    }
}
