namespace VCCSharp.OpCodes.Tests.Model;

internal static class Rnd
{
    private static Random _rnd = new Random();

    /// <summary>
    /// Byte
    /// </summary>
    /// <returns>Returns a random byte</returns>
    public static byte B() => (byte) _rnd.Next();

    /// <summary>
    /// Word
    /// </summary>
    /// <returns>Returns a random word</returns>
    public static ushort W() => (ushort) _rnd.Next();

    //public static uint D() => (uint) _rnd.Next();
}
