namespace VCCSharp.Models.Graphics
{
    /// <summary>
    /// TODO: Temporary class
    /// </summary>
    public class UintArray
    {
        private readonly uint[] _array;

        public UintArray(int size)
        {
            _array = new uint[size];
        }

        public uint this[int index]
        {
            get => _array[index];
            set => _array[index] = value;
        }
    }
}
