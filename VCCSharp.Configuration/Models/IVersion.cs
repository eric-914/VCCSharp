namespace VCCSharp.Configuration.Models
{
    public interface IVersion
    {
        string Release { set; }

        //TODO: Probably not configuration persistence related.
        string GetRelease();
    }
}