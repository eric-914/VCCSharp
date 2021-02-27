using System;
using System.Diagnostics;
using System.Text;
using System.Windows;
using VCCSharp.library;

namespace VCCSharp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            string commandLine = "\"c:\\CoCo\\Mega-Bug (1982) (26-3076) (Tandy).ccc\" ";

            IntPtr hInstance = Process.GetCurrentProcess().Handle;

            var retValue = Library.AppRun(hInstance, commandLine);
        }
    }
}
