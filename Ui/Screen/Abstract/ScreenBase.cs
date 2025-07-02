using System;
using System.Collections.Generic;
using Cerm.Ui.Screen.Widget.Abstruct;

namespace Cerm.Ui.Screen.Abstruct
{
    public abstract class ScreenBase
    {
        public int Width { get; set; } = Console.WindowWidth;
        public int Height { get; set; } = Console.WindowHeight;
        public IList<WidgetBase> Widgets { get; set; }
        public abstract void OnResized();
        public abstract void Render();
        public abstract void Update();
        public abstract void HandleInput(ConsoleKeyInfo key);
        public virtual void OnEnter() { }
        public virtual void OnExit() { }
    }
}