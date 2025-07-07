using System;

namespace Cerm.Ui.Widget.Utils
{
    public static class WidgetUtils
    {
        public static int GetCenteredX(int width)
        {
            return (Console.WindowWidth - width) / 2;
        }
    }
}