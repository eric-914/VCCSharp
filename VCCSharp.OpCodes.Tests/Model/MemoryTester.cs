namespace VCCSharp.OpCodes.Tests.Model;

internal class MemoryTester
{
    private Dictionary<ushort, byte> _memory = new();
    private readonly Seeds _seeds;
    private int _index = 0;

    public MemoryTester(Seeds seeds)
    {
        _seeds = seeds;
    }

    public byte Read(ushort address)
    {
        if (!_memory.ContainsKey(address))
        {
            _memory.Add(address, _seeds[_index++]);
        }

        return _memory[address];
    }

    public void Write(ushort address, byte value)
    {
        if (!_memory.ContainsKey(address))
        {
            _memory.Add(address, value);
        }
        else
        {
            _memory[address] = value;
        }
    }

    public IEnumerable<string> ToList() => _memory.Select(x => $"{x.Key}:{x.Value}");
}
