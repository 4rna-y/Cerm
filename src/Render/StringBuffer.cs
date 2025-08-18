using System.Text;

namespace Cerm.Render
{
    public class StringBuffer : TextWriter
    {
        private StringBuilder sb;

        public StringBuffer(int capacity)
        {
            sb = new StringBuilder(capacity);
        }

        public override Encoding Encoding => Encoding.UTF8;
        public int Length => sb.Length;

        public override void Write(char value) => sb.Append(value);
        public override void Write(string? value) => sb.Append(value);
        public override void Write(ReadOnlySpan<char> buffer) => sb.Append(buffer);
        public void Clear() => sb.Clear();

        public override string ToString() => sb.ToString();

    }
}