using VCCSharp.Libraries;
using HWND = System.IntPtr;

namespace VCCSharp.Modules
{
    public interface ICallbacks
    {
        void SetDialogAudioBars(HWND hDlg, ushort left, ushort right);
    }

    public class Callbacks : ICallbacks
    {
        public void SetDialogAudioBars(HWND hDlg, ushort left, ushort right)
        {
            Library.Callbacks.SetDialogAudioBars(hDlg, left, right);
        }
    }
}
