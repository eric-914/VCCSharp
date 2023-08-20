namespace VCCSharp.Configuration.Models
{
    public interface IVersionConfiguration
    {
        string Release { set; }

        //TODO: Probably not configuration persistence related.
        string GetRelease();
    }
}