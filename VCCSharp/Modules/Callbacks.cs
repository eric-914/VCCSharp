using VCCSharp.Libraries;
using HWND = System.IntPtr;

namespace VCCSharp.Modules
{
    public interface ICallbacks
    {
        void SetDialogAudioBars(HWND hDlg, ushort left, ushort right);
        void SetDialogCpuMultiplier(HWND hDlg, byte cpuMultiplier);
    }

    public class Callbacks : ICallbacks
    {
        public void SetDialogAudioBars(HWND hDlg, ushort left, ushort right)
        {
            Library.Callbacks.SetDialogAudioBars(hDlg, left, right);
        }

        public void SetDialogCpuMultiplier(HWND hDlg, byte cpuMultiplier)
        {
            Library.Callbacks.SetDialogCpuMultiplier(hDlg, cpuMultiplier);
        }
    }
}
