using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Libraries;
using VCCSharp.Models;

namespace VCCSharp.TapePlayer
{
    public class TapePlayerManager : ITapePlayer
    {
        private readonly IModules _modules;

        private readonly TapePlayerViewModel _viewModel = new TapePlayerViewModel();
        private TapePlayerWindow _view;

        private static unsafe ConfigState* _configState;

        public TapePlayerManager(IModules modules)
        {
            _modules = modules;

            _viewModel.PropertyChanged += (sender, args) =>
            {
                switch (args.PropertyName)
                {
                    case "FilePath":
                        break;

                    case "Mode":
                        break;

                    case "Counter":
                        break;
                }
            };
        }

        public unsafe void ShowDialog(ConfigState* state)
        {
            _configState = state;
            _viewModel.State = state;

            _view ??= new TapePlayerWindow(_viewModel, this);

            _view.Show();
        }

        public void Browse()
        {
            _modules.Cassette.LoadTape();

            unsafe
            {
                ConfigState* configState = _modules.Config.GetConfigState();

                configState->TapeCounter = 0;
            }

            _modules.Cassette.SetTapeCounter(0);

            _viewModel.Counter = 0;
        }

        public void Record()
        {
            _viewModel.Mode = TapeModes.REC;

            unsafe
            {
                ConfigState* configState = _modules.Config.GetConfigState();

                configState->TapeMode = Define.REC;
            }

            _modules.Cassette.SetTapeMode(Define.REC);
        }

        public void Play()
        {
            _viewModel.Mode = TapeModes.PLAY;

            unsafe
            {
                ConfigState* configState = _modules.Config.GetConfigState();

                configState->TapeMode = Define.PLAY;
            }

            _modules.Cassette.SetTapeMode(Define.PLAY);
        }

        public void Stop()
        {
            _viewModel.Mode = TapeModes.STOP;

            unsafe
            {
                ConfigState* configState = _modules.Config.GetConfigState();

                configState->TapeMode = Define.STOP;
            }

            _modules.Cassette.SetTapeMode(Define.STOP);
        }

        public void Eject()
        {
            _viewModel.Mode = TapeModes.EJECT;
            
            unsafe
            {
                ConfigState* configState = _modules.Config.GetConfigState();

                configState->TapeMode = Define.EJECT;
            }

            _modules.Cassette.SetTapeMode(Define.EJECT);
        }

        public void Rewind()
        {
            _viewModel.Counter = 0;

            unsafe
            {
                ConfigState* configState = _modules.Config.GetConfigState();

                configState->TapeCounter = 0;

                _modules.Cassette.SetTapeCounter(0);
            }
        }
    }
}
