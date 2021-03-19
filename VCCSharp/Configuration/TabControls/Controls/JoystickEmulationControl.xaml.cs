using System.Windows.Controls;

namespace VCCSharp.Configuration.TabControls.Controls
{
    public partial class JoystickEmulationControl : UserControl
    {
        public string Side { get; set; } = "Left";

        public JoystickEmulationControl()
        {
            InitializeComponent();

            DataContext = this;
        }
    }
}
