using DX8.Tester.Model;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using DX8.Tester.Converters;

namespace DX8.Tester.Controls
{
    public partial class ButtonControl 
    {
        public static readonly DependencyProperty TextProperty
            = DependencyProperty.Register("Text", typeof(string), typeof(ButtonControl));

        public ButtonControl()
        {
            InitializeComponent();
        }

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public Binding Button
        {
            set
            {
                value.Mode = BindingMode.OneWay;
                value.Converter = new BooleanToBorderColorConverter();
                BorderControl.SetBinding(Border.BorderBrushProperty, value);
            }
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.Property == TextProperty)
            {
                LabelControl.Content = Text;
            }
        }
    }
}
