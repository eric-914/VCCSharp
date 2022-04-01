using DX8.Internal.Interfaces;

namespace DX8.Models;

/// <summary>
/// Not really a Dx class.  Need a way to hold the Device COM object and its name together;
/// </summary>
internal class DxDevice
{

    public int Index { get; }
    public string InstanceName { get; }

    public IDirectInputDevice Device { get; }

    public DxDevice(int index, string instanceName, IDirectInputDevice device)
    {
        Index = index;
        InstanceName = instanceName;
        Device = device;
    }
}
