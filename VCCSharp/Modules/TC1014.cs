using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using VCCSharp.IoC;
using VCCSharp.Libraries;
using VCCSharp.Models;

namespace VCCSharp.Modules
{
    public interface ITC1014
    {
        void MC6883Reset();
        void CopyRom();
        void MmuReset();
        byte MmuInit(byte ramSizeOption);
        void MemWrite8(byte data, ushort address);
        void GimeAssertVertInterrupt();
        void GimeAssertHorzInterrupt();
        void GimeAssertTimerInterrupt();
    }

    public class TC1014 : ITC1014
    {
        private readonly IModules _modules;

        public TC1014(IModules modules)
        {
            _modules = modules;
        }

        public void MC6883Reset()
        {
            Library.TC1014.MC6883Reset();
        }

        //TODO: Used by MmuInit()
        public void CopyRom()
        {
            const string ROM = "coco3.rom";

            unsafe
            {
                ConfigState* configState = _modules.Config.GetConfigState();
                
                //--Try loading from Vcc.ini >> CoCoRomPath
                string cocoRomPath = Converter.ToString(configState->Model->CoCoRomPath);

                string path = Path.Combine(Converter.ToString(configState->Model->CoCoRomPath), ROM);

                if (LoadInternalRom(path) == Define.TRUE)
                {
                    Debug.WriteLine($"Found {ROM} in CoCoRomPath");
                    return;
                }

                //--Try loading from Vcc.inin >> ExternalBasicImage
                string externalBasicImage = _modules.Config.ExternalBasicImage();

                if (!string.IsNullOrEmpty(externalBasicImage) && LoadInternalRom(externalBasicImage) == Define.TRUE)
                {
                    Debug.WriteLine($"Found {ROM} in ExternalBasicImage");
                    return;
                }

                //--Try loading from current executable folder
                string exePath = Path.GetDirectoryName(GetExecPath());
                string exeFile = Path.Combine(exePath, ROM);

                if (LoadInternalRom(exeFile) != Define.FALSE)
                {
                    Debug.WriteLine($"Found {ROM} in executable folder");
                    return;
                }

                //--Give up...
                string message = @$"
Could not locate {ROM} in any of these locations:
* Vcc.ini >> CoCoRomPath=""{cocoRomPath}""
* Vcc.ini >> ExternalBasicImage=""{externalBasicImage}""
* In the same folder as the executable: ""{exePath}""
";

                MessageBox.Show(message, "Error");

                Environment.Exit(0);
            }
        }

        public void MmuReset()
        {
            Library.TC1014.MmuReset();
        }

        public byte MmuInit(byte ramSizeOption)
        {
            //Library.TC1014.CopyRom();

            return Library.TC1014.MmuInit(ramSizeOption);
        }

        public void MemWrite8(byte data, ushort address)
        {
            Library.TC1014.MemWrite8(data, address);
        }

        public void GimeAssertVertInterrupt()
        {
            Library.TC1014.GimeAssertVertInterrupt();
        }

        public void GimeAssertHorzInterrupt()
        {
            Library.TC1014.GimeAssertHorzInterrupt();
        }

        public void GimeAssertTimerInterrupt()
        {
            Library.TC1014.GimeAssertTimerInterrupt();
        }

        public ushort LoadInternalRom(string filename)
        {
            return Library.TC1014.LoadInternalRom(filename);
        }

        public string GetExecPath()
        {
            unsafe
            {
                byte[] buffer = new byte[Define.MAX_PATH];

                fixed (byte* b = buffer)
                {
                    Library.TC1014.GetExecPath(b);
                }

                return Converter.ToString(buffer);
            }
        }
    }
}
