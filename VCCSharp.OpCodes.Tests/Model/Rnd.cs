namespace VCCSharp.OpCodes.Tests.Model;

internal static class Rnd
{
    private static Random _rnd = new Random();

    public static byte B() => (byte) _rnd.Next();
    public static ushort W() => (ushort) _rnd.Next();
}
