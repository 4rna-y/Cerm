using Cerm.Ui.Screen.Widget.Abstruct;
using Cerm.Ui.Widget.Abstruct;

namespace Cerm.Ui.Widget
{
    public class StackPanel : WidgetBase, IParentable
    {
        private WidgetCollection children;

        public WidgetCollection Children => children;

        public StackPanel(int x, int y, int width, int height) : base(x, y, width, height)
        {
            children = new WidgetCollection(128);
        }

        public override void OnUpdate()
        {
            throw new System.NotImplementedException();
        }

        public override void OnRender()
        {
            throw new System.NotImplementedException();
        }
    }
}