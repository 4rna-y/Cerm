using Cerm.Ui.Screen.Widget.Abstruct;

namespace Cerm.Ui.Widget.Abstruct
{
    public interface IParentable
    {
        WidgetCollection Children { get; }
    }
}