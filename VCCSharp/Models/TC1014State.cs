namespace VCCSharp.Models
{
    public struct TC1014State
    {
        //--TODO: This is really byte* MemPages[1024]
        //public unsafe byte** MemPages; //[1024];
        public unsafe fixed long MemPages[1024];
    }
}
