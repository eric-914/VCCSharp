namespace VCCSharp.Models.Graphics;

public class Palette32
{
    private readonly uint[] _array = new uint[16];
    private readonly int[] _mapping = new int[16];

    public Palette32()
    {
        Reset();
    }

    public uint this[int index] => _array[index];

    public string Contents => string.Join(',', _array);

    public void Map(UintArray source)
    {
        for (var index = 0; index < 16; index++)
        {
            _array[index] = source[_mapping[index]];
        }
    }

    /// <summary>
    /// This gets called by the CPU, and it's rearranging the lookup mapping.
    /// </summary>
    public void Map(UintArray source, byte paletteIndex, byte lookupIndex)
    {
        _array[paletteIndex] = source[lookupIndex];
        _mapping[paletteIndex] = lookupIndex;
    }

    public void Reset()
    {
        for (var index = 0; index < 16; index++)
        {
            _mapping[index] = index;
        }
    }
}
