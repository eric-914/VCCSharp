using System;
using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Libraries;
using VCCSharp.Models;
using HINSTANCE = System.IntPtr;

namespace VCCSharp.Modules
{
    // ReSharper disable once InconsistentNaming
    public interface IPAKInterface
    {
        unsafe PakInterfaceState* GetPakInterfaceState();
        void UnloadDll(byte emulationRunning);
        void GetModuleStatus();
        void ResetBus();
        void UpdateBusPointer();
        int InsertModule(byte emulationRunning, string modulePath);
        void PakTimer();
        int HasHeartBeat();
        void InvokeHeartBeat();
        string GetCurrentModule();
        byte PakPortRead(byte port);
        void PakPortWrite(byte port, byte data);
        ushort PakAudioSample();
        int HasConfigModule();
        void InvokeConfigModule(byte menuItem);
        int UnloadPack(byte emulationRunning);
    }

    // ReSharper disable once InconsistentNaming
    public class PAKInterface : IPAKInterface
    {
        private readonly IModules _modules;
        private readonly IKernel _kernel;

        public PAKInterface(IModules modules, IKernel kernel)
        {
            _modules = modules;
            _kernel = kernel;
        }

        public unsafe PakInterfaceState* GetPakInterfaceState()
        {
            return Library.PAKInterface.GetPakInterfaceState();
        }

        public void PakTimer()
        {
            if (HasHeartBeat() != Define.FALSE)
            {
                InvokeHeartBeat();
            }
        }

        public string GetCurrentModule()
        {
            unsafe
            {
                return Converter.ToString(GetPakInterfaceState()->DllPath);
            }
        }

        //TODO: Used by LoadROMPack(...), UnloadPack(...), InsertModule(...)
        public void UnloadDll(byte emulationRunning)
        {
            //PakInterfaceState* pakInterfaceState = GetPakInterfaceState();

            //if ((pakInterfaceState->DialogOpen == Define.TRUE) && (emuState->EmulationRunning == Define.TRUE))
            //{
            //    MessageBox.Show("Close Configuration Dialog before unloading", "Ok");

            //    return;
            //}

            Library.PAKInterface.UnloadDll(emulationRunning);

            //if (pakInterfaceState->hInstLib != null) {
            //    _kernel.FreeLibrary(pakInterfaceState->hInstLib);
            //}

            //pakInterfaceState->hInstLib = Zero;

            //_modules.MenuCallbacks.DynamicMenuCallback(emuState, null, MenuActions.Refresh, Define.IGNORE);
        }

        public void ResetBus()
        {
            unsafe
            {
                GetPakInterfaceState()->BankedCartOffset = 0;

                if (HasModuleReset() == Define.TRUE)
                {
                    InvokeModuleReset();
                }
            }
        }

        public void UpdateBusPointer()
        {
            if (HasSetInterruptCallPointer() == Define.TRUE)
            {
                InvokeSetInterruptCallPointer();
            }
        }

        public int InsertModule(byte emulationRunning, string modulePath)
        {
            int fileType = FileID(modulePath);

            switch (fileType)
            {
                case 0:		//File doesn't exist
                    return InsertModuleCase0();

                case 1:		//File is a DLL
                    return InsertModuleCase1(emulationRunning, modulePath);

                case 2:		//File is a ROM image
                    return InsertModuleCase2(emulationRunning, modulePath);
            }

            return Define.NOMODULE;
        }

        //File doesn't exist
        public int InsertModuleCase0()
        {
            return Define.NOMODULE;
        }

        //File is a DLL
        public int InsertModuleCase1(byte emulationRunning, string modulePath)
        {
            string catNumber = "";
            string temp = "";
            string text = "";
            string ini = "";

            UnloadDll(emulationRunning);

            unsafe
            {
                PakInterfaceState* instance = GetPakInterfaceState();

                instance->hInstLib = PAKLoadLibrary(modulePath);

                if (instance->hInstLib == HINSTANCE.Zero)
                {
                    return Define.NOMODULE;
                }

                SetCart(0);

                if (SetDelegates(instance->hInstLib) != 0)
                {
                    PAKFreeLibrary(instance->hInstLib);

                    instance->hInstLib = HINSTANCE.Zero;

                    return Define.NOTVCC;
                }

                instance->BankedCartOffset = 0;

                if (HasDmaMemPointer() != 0)
                {
                    InvokeDmaMemPointer();
                }

                if (HasSetInterruptCallPointer() != 0)
                {
                    InvokeSetInterruptCallPointer();
                }

                string modName = Converter.ToString(instance->Modname);
                InvokeGetModuleName(modName, catNumber);  //Instantiate the menus from HERE!

                //sprintf(temp, "Configure %s", instance->Modname);
                temp = $"Configure {modName}";

                //strcat(text, "Module Name: ");
                //strcat(text, instance->Modname);
                //strcat(text, "\n");
                text = $"Module Name: {modName}\n";

                if (HasConfigModule() != 0)
                {
                    instance->ModualParms |= 1;

                    text += "Has Configurable options\n";
                }

                if (HasPakPortWrite() != 0)
                {
                    instance->ModualParms |= 2;

                    text += "Is IO writable\n";
                }

                if (HasPakPortRead() != 0)
                {
                    instance->ModualParms |= 4;

                    text += "Is IO readable\n";
                }

                if (HasSetInterruptCallPointer() != 0)
                {
                    instance->ModualParms |= 8;

                    text += "Generates Interrupts\n";
                }

                if (HasDmaMemPointer() != 0)
                {
                    instance->ModualParms |= 16;

                    text += "Generates DMA Requests\n";
                }

                if (HasHeartBeat() != 0)
                {
                    instance->ModualParms |= 32;

                    text += "Needs Heartbeat\n";
                }

                if (HasModuleAudioSample() != 0)
                {
                    instance->ModualParms |= 64;

                    text += "Analog Audio Outputs\n";
                }

                if (HasPakMemWrite8() != 0)
                {
                    instance->ModualParms |= 128;

                    text += "Needs ChipSelect Write\n";
                }

                if (HasPakMemRead8() != 0)
                {
                    instance->ModualParms |= 256;

                    text += "Needs ChipSelect Read\n";
                }

                if (HasModuleStatus())
                {
                    instance->ModualParms |= 512;

                    text += "Returns Status\n";
                }

                if (HasModuleReset() != 0)
                {
                    instance->ModualParms |= 1024;

                    text += "Needs Reset Notification\n";
                }

                if (HasSetIniPath() != 0)
                {
                    instance->ModualParms |= 2048;

                    //strcpy(ini, GetConfigState()->IniFilePath);
                    ini = Converter.ToString(_modules.Config.GetConfigState()->IniFilePath);

                    InvokeSetIniPath(ini);
                }

                if (HasPakSetCart() != 0)
                {
                    instance->ModualParms |= 4096;

                    text += "Can Assert CART\n";

                    InvokePakSetCart();
                }

                Console.WriteLine(temp);
                Console.WriteLine(text);

                //strcpy(instance->DllPath, modulePath);
                Converter.ToByteArray(modulePath, instance->DllPath);

                return 0;
            }
        }

        //File is a ROM image
        public int InsertModuleCase2(byte emulationRunning, string modulePath)
        {
            UnloadDll(emulationRunning);

            LoadROMPack(emulationRunning, modulePath);

            unsafe
            {
                PakInterfaceState* instance = GetPakInterfaceState();

                //strncpy(instance->Modname, modulePath, MAX_PATH);
                Converter.ToByteArray(modulePath, instance->Modname);

                _modules.FileOperations.FilePathStripPath(instance->Modname);

            }

            _modules.MenuCallbacks.DynamicMenuCallback(null, MenuActions.Refresh, Define.IGNORE);

            SetCart(1);

            return 0;
        }

        public int HasHeartBeat()
        {
            return Library.PAKInterface.HasHeartBeat();
        }

        public void InvokeHeartBeat()
        {
            Library.PAKInterface.InvokeHeartBeat();
        }

        public int HasSetInterruptCallPointer()
        {
            return Library.PAKInterface.HasSetInterruptCallPointer();
        }

        public void InvokeSetInterruptCallPointer()
        {
            Library.PAKInterface.InvokeSetInterruptCallPointer();
        }

        public int HasModuleReset()
        {
            return Library.PAKInterface.HasModuleReset();
        }

        public void InvokeModuleReset()
        {
            Library.PAKInterface.InvokeModuleReset();
        }

        public unsafe void GetModuleStatus()
        {
            if (HasModuleStatus())
            {
                //TODO: Things break here if the status line is empty.
                string text = (_modules.Emu.StatusLine ?? "") + "...";
                byte[] status = Converter.ToByteArray(text);
                fixed (byte* p = status)
                {
                    InvokeModuleStatus(p);
                }
            }
            else
            {
                _modules.Emu.StatusLine = string.Empty;
            }
        }

        public byte PakPortRead(byte port)
        {
            return Library.PAKInterface.PakPortRead(port);
        }

        public void PakPortWrite(byte port, byte data)
        {
            Library.PAKInterface.PakPortWrite(port, data);
        }

        public bool HasModuleStatus()
        {
            return Library.PAKInterface.HasModuleStatus() != 0;
        }

        public unsafe void InvokeModuleStatus(byte* statusLine)
        {
            Library.PAKInterface.InvokeModuleStatus(statusLine);
        }

        public ushort PakAudioSample()
        {
            return Library.PAKInterface.PakAudioSample();
        }

        public int HasConfigModule()
        {
            return Library.PAKInterface.HasConfigModule();
        }

        public void InvokeConfigModule(byte menuItem)
        {
            Library.PAKInterface.InvokeConfigModule(menuItem);
        }

        public int FileID(string filename)
        {
            return Library.PAKInterface.FileID(filename);
        }

        public HINSTANCE PAKLoadLibrary(string modulePath)
        {
            return Library.PAKInterface.PAKLoadLibrary(modulePath);
        }

        public void SetCart(byte cart)
        {
            Library.PAKInterface.SetCart(cart);
        }

        public int SetDelegates(HINSTANCE hInstLib)
        {
            return Library.PAKInterface.SetDelegates(hInstLib);
        }

        public void PAKFreeLibrary(HINSTANCE hInstLib)
        {
            Library.PAKInterface.PAKFreeLibrary(hInstLib);
        }

        public int HasDmaMemPointer()
        {
            return Library.PAKInterface.HasDmaMemPointer();
        }

        public void InvokeDmaMemPointer()
        {
            Library.PAKInterface.InvokeDmaMemPointer();
        }

        public void InvokeGetModuleName(string modName, string catNumber)
        {
            Library.PAKInterface.InvokeGetModuleName(modName, catNumber);
        }

        public int HasPakPortWrite()
        {
            return Library.PAKInterface.HasPakPortWrite();
        }

        public int HasPakPortRead()
        {
            return Library.PAKInterface.HasPakPortRead();
        }

        public int HasModuleAudioSample()
        {
            return Library.PAKInterface.HasModuleAudioSample();
        }

        public int HasPakMemWrite8()
        {
            return Library.PAKInterface.HasPakMemWrite8();
        }

        public int HasPakMemRead8()
        {
            return Library.PAKInterface.HasPakMemRead8();
        }

        public int HasSetIniPath()
        {
            return Library.PAKInterface.HasSetIniPath();
        }

        public int HasPakSetCart()
        {
            return Library.PAKInterface.HasPakSetCart();
        }

        public void InvokeSetIniPath(string ini)
        {
            Library.PAKInterface.InvokeSetIniPath(ini);
        }

        public void InvokePakSetCart()
        {
            Library.PAKInterface.InvokePakSetCart();
        }

        public int LoadROMPack(byte emulationRunning, string filename)
        {
            return Library.PAKInterface.LoadROMPack(emulationRunning, filename);
        }

        public int UnloadPack(byte emulationRunning)
        {
            UnloadDll(emulationRunning);

            unsafe
            {
                PakInterfaceState* instance = GetPakInterfaceState();

                Converter.ToByteArray("", instance->DllPath);
                Converter.ToByteArray("Blank", instance->Modname);

                instance->RomPackLoaded = Define.FALSE;

                SetCart(0);

                FreeMemory(instance->ExternalRomBuffer);

                instance->ExternalRomBuffer = null;

                _modules.MenuCallbacks.DynamicMenuCallback(null, MenuActions.Refresh, Define.IGNORE);

                return Define.NOMODULE;
            }
        }

        public unsafe void FreeMemory(byte* target)
        {
            Library.PAKInterface.FreeMemory(target);
        }

    }
}
