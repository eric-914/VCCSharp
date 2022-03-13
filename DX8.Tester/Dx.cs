namespace DX8.Tester;

internal class Dx
{
    public  IDxInput Input { get; }

    public Dx(IDxFactory factory)
    {
        Input = factory.CreateDxInput();
    }

    public Dx() : this(DxFactory.Instance) { }
}