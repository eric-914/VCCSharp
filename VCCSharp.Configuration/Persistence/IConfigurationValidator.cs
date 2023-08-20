namespace VCCSharp.Configuration.Persistence
{
    public interface IConfigurationValidator
    {
        void Validate(IConfiguration model);
    }
}
