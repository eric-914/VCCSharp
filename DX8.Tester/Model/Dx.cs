namespace DX8.Tester.Model;

internal class Dx
{
    public DxManager Joystick { get; }

    public Dx(IDxFactory factory)
    {
        Joystick = new DxManager(factory.CreateDxInput());
    }

    public Dx() : this(DxFactory.Instance) { }
}