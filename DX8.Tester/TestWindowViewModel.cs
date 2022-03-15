using DX8.Tester.Annotations;
using DX8.Tester.Model;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace DX8.Tester
{
    internal class TestWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private readonly TestWindowModel _model = new();
        private readonly ThreadRunner _runner = new();

        public List<string> Joysticks { get; }

        public DPadModel DPad => _model.DPad;

        public ButtonModel Button => _model.Button;

        public TestWindowViewModel()
        {
            Application.Current.Exit += (_, _) => _runner.IsRunning = false;

            Joysticks = _model.FindJoysticks();

            //Task.Run(() => _runner.Run(Refresh));
            Refresh();
        }

        public void Refresh()
        {
            _model.Refresh();

            OnPropertyChanged(nameof(DPad));
            OnPropertyChanged(nameof(Button));
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
