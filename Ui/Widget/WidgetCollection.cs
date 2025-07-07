using Cerm.Ui.Screen.Widget.Abstruct;

namespace Cerm.Ui.Widget
{
    public class WidgetCollection
    {
        private WidgetBase[] widgets;
        private int lastIndex = 0;

        public int Count { get; }

        public WidgetCollection(int capacity)
        {
            widgets = new WidgetBase[capacity];
            Count = capacity;
        }

        public WidgetBase Get(int index)
        {
            if (lastIndex <= index) return null;
            return widgets[index];
        }

        public void Append(WidgetBase widget)
        {
            widgets[lastIndex++] = widget;
        }
    }
}