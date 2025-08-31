using Cerm.Render.Component.Layout;

namespace Cerm.Render.Component
{
    public class TextLine : ComponentBase
    {
        private string text;
        public string Text
        {
            get => text;
            set
            {
                text = value;
                RequiredRedraw = true;
            }
        }

        public TextLine(string text, LayoutValue x, LayoutValue y, PositionAnchor anchor, Color foreground, Color background) : base(x, y, anchor, LayoutValue.Fixed(text.Length), LayoutValue.Fixed(1), foreground, background)
        {
            this.text = text;

        }

        public override void Render()
        {
            if (buffer.Length == 0 || string.IsNullOrEmpty(text)) return;

            Array.Clear(buffer, 0, buffer.Length);
            Span<char> span = buffer.AsSpan();

            int copyLength = Math.Min(text.Length, span.Length);
            text.AsSpan(0, copyLength).CopyTo(span.Slice(0, copyLength));
        }
    }
}