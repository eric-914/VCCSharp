using VCCSharp.Main.ViewModels;

namespace VCCSharp.Models;

public class AudioSpectrum : NotifyViewModel
{
    private int _left = 500;
    private int _right = 600;

    public int LeftSpeaker
    {
        get => _left;
        set
        {
            _left = value;
            OnPropertyChanged();
        }
    }

    public int RightSpeaker
    {
        get => _right;
        set
        {
            _right = value;
            OnPropertyChanged();
        }
    }

    public void UpdateSoundBar(int left, int right)
    {
        LeftSpeaker = left;
        RightSpeaker = right;
        //OnPropertyChanged("LeftSpeaker");
        //OnPropertyChanged("RightSpeaker");
    }
}