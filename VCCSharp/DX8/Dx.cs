namespace VCCSharp.DX8
{
    public interface IDx
    {
        #region DxDraw
        #endregion

        #region DxSound
        #endregion

        #region DxInput
        #endregion
    }

    /// <summary>
    /// Final wrapper of the DX8 objects, with no unsafe in interface
    /// </summary>
    public class Dx
    {
        private readonly IDxDraw _draw;
        private readonly IDxSound _sound;
        private readonly IDxInput _input;

        public Dx(IDxDraw draw, IDxSound sound, IDxInput input)
        {
            _draw = draw;
            _sound = sound;
            _input = input;
        }

        public Dx() : this(new DxDraw(), new DxSound(), new DxInput()) { }
    }
}
