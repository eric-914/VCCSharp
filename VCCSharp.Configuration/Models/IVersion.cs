namespace VCCSharp.Configuration.Models
{
    public interface IVersion
    {
        string Release { set; }

        string GetRelease();
    }
}