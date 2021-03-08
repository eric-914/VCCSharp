namespace VCCSharp.Models
{
    public struct SoundCardList
    {
        public unsafe fixed byte CardName[64];
        public unsafe _GUID* Guid;
    }
}
