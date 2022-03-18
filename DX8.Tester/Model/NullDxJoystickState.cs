namespace DX8.Tester.Model
{
    internal class NullDxJoystickState : IDxJoystickState
    {
        public int Horizontal => 0;
        public int Vertical => 0;
        public bool X => false;
        public bool Y => false;
        public bool A => false;
        public bool B => false;
        public bool LB => false;
        public bool RB => false;
        public bool Back => false;
        public bool Start => false;
    }
}
