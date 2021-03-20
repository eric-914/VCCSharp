using System;
using System.Diagnostics;
using System.Windows;

namespace VCCSharp.Configuration
{
    public partial class ConfigurationWindow
    {
        public Action Apply { get;set; }

        public ConfigurationWindow(ConfigurationViewModel model)
        {
            Debug.WriteLine("ConfigurationWindow()");
            DataContext = model;
            InitializeComponent();
            Debug.WriteLine("ConfigurationWindow(model)");
        }

        private void OnOkClick(object sender, RoutedEventArgs e)
        {
            OnApplyClick(sender, e);
            OnCancelClick(sender, e);
        }

        private void OnApplyClick(object sender, RoutedEventArgs e)
        {
            Apply();
        }

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
