using System.Windows;

namespace VCCSharp
{
    public class Actions
    {
        public void ApplicationExit()
        {
            Application.Current.MainWindow?.Close();
        }

        public void AboutVcc()
        {
            MessageBox.Show("About Vcc");
        }

        public void SaveConfiguration()
        {
            MessageBox.Show("Save Configuration");
        }

        public void LoadConfiguration()
        {
            MessageBox.Show("Load Configuration");
        }

        public void HardReset()
        {
            MessageBox.Show("Hard Reset");
        }

        public void SoftReset()
        {
            MessageBox.Show("Soft Reset");
        }

        public void CopyText()
        {
            MessageBox.Show("Copy Text");
        }

        public void PasteText()
        {
            MessageBox.Show("Paste Text");
        }

        public void PasteBasicCodeMerge()
        {
            MessageBox.Show("Paste BASIC Code > Merge");
        }

        public void PasteBasicCodeNew()
        {
            MessageBox.Show("Paste BASIC Code > New");
        }

        public void FlipArtifactColors()
        {
            MessageBox.Show("Flip Artifact Colors");
        }

        public void OpenConfiguration()
        {
            MessageBox.Show("Open Configuration");
        }

        public void LoadCartridge()
        {
            MessageBox.Show("Load Cartridge");
        }

        public void EjectCartridge()
        {
            MessageBox.Show("Eject Cartridge");
        }
    }
}
