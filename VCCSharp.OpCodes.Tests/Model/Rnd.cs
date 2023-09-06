namespace VCCSharp.OpCodes.Tests.Model;

internal static class Rnd
{
    private static Random _rnd = new Random(12345);

    public static byte B() => (byte) _rnd.Next();
    public static ushort W() => (ushort) _rnd.Next();
}
