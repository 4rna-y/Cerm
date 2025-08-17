using Cerm.Render.Interfaces;

namespace Cerm.Render.Component
{
    public class TestComponent : ComponentBase
    {
        private char charactor;
        public TestComponent(char c, LayoutValue x, LayoutValue y, Color fg, Color bg) : base(x, y, PositionAnchor.LeftTop, LayoutValue.Fixed(1), LayoutValue.Fixed(1), fg, bg)
        {
            charactor = c;
        }

        public override void Render()
        {
            if (buffer.Length > 0)
            {
                buffer[0] = charactor;
            }
        }
    }
}