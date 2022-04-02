namespace DX8.Models;

public interface IDxDevice
{
    int Index { get; }
    string InstanceName { get; }
}

/// <summary>
/// Not really a Dx class.  Need a way to hold the Device COM object and its name together;
/// </summary>
internal class DxDevice : IDxDevice
{

    public int Index { get; }
    public string InstanceName { get; }

    public DxDevice(int index, string instanceName)
    {
        Index = index;
        InstanceName = instanceName;
    }
}
