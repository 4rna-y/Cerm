using System.Text;
using Cerm.Render.Component;
using Cerm.Render.Interfaces;

namespace Cerm.ScreenTest
{
    public class ColorStatusTextComponent : ComponentBase
    {
        private string text;
        public string Text
        {
            get => text;
            set
            {
                RequiredRedraw = true;
                text = value;
            }
        }
        public ColorStatusTextComponent(string str, LayoutValue x, LayoutValue y, int w, Color fg, Color bg) :
        base(x, y, PositionAnchor.LeftTop, LayoutValue.Fixed(w), LayoutValue.Fixed(1), fg, bg)
        {
            this.text = str;
        }

        public override void Render()
        {
            if (buffer.Length == 0) return;

            Array.Clear(buffer, 0, buffer.Length);

            if (text.Length >= this.ActualWidth)
            {
                int truncateLength = Math.Min(this.ActualWidth, buffer.Length);
                text.AsSpan(0, truncateLength).CopyTo(buffer.AsSpan(0, truncateLength));
                return;
            }

            int spaces = (this.ActualWidth - text.Length) / 2;
            StringBuilder cs = new StringBuilder();
            cs.Append(' ', spaces);
            cs.Append(text);
            cs.Append(' ', this.ActualWidth - spaces - text.Length);

            string csText = cs.ToString();
            int copyLength = Math.Min(csText.Length, buffer.Length);
            csText.AsSpan(0, copyLength).CopyTo(buffer.AsSpan(0, copyLength));
        }
    }
}