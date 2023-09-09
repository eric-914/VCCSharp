namespace VCCSharp.OpCodes.Tests.Model;

public class Seeds : List<byte>
{
    public Seeds(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Add(Rnd.B());
        }
    }

    public Seeds() { }
}
