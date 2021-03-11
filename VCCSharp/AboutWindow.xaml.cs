using System.Windows;

namespace VCCSharp
{
    public partial class AboutWindow
    {
        public AboutWindow()
        {
            InitializeComponent();
        }

        public void OnOkClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            Close();
        }
    }
}
