using VCCSharp.Enums;

namespace VCCSharp.Models.Graphics
{
    /// <summary>
    /// Manages the palette array for the different monitor types
    /// </summary>
    public class PaletteLookup32
    {
        private readonly UintArray _composite = new(64);
        private readonly UintArray _rgb = new(64);

        public UintArray this[MonitorTypes monitorType]
        {
            get
            {
                switch (monitorType)
                {
                    case MonitorTypes.Composite: return _composite;
                    case MonitorTypes.RGB: return _rgb;
                }

                return null;
            }
        }
    }
}