namespace VCCSharp.Libraries.Models
{
    [Serializable]
    public struct MSG
    {
        public IntPtr hwnd { get; set; }
        public int message { get; set; }
        public IntPtr wParam { get; set; }
        public IntPtr lParam { get; set; }
        public int time { get; set; }
        public int pt_x { get; set; }
        public int pt_y { get; set; }
    }
}
