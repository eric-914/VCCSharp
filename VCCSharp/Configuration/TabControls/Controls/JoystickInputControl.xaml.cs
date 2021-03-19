using System.Windows.Controls;

namespace VCCSharp.Configuration.TabControls.Controls
{
    public partial class JoystickInputControl : UserControl
    {
        public string Side { get; set; } = "Left";

        public JoystickInputControl()
        {
            InitializeComponent();

            DataContext = this;
        }
    }
}
