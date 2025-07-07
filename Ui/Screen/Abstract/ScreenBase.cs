using System;
using System.Collections.Generic;
using Cerm.Ui.Screen.Widget.Abstruct;
using Cerm.Ui.Widget.Abstruct;

namespace Cerm.Ui.Screen.Abstruct
{
    public abstract class ScreenBase
    {
        internal int lastIndex = 0;

        internal int currentFocusingIndex = -1;

        public int Width { get; set; } = Console.WindowWidth;
        public int Height { get; set; } = Console.WindowHeight;
        public WidgetBase[] Widgets { get; set; }

        public ScreenBase()
        {
            Widgets = new WidgetBase[512];
        }

        public void Append(WidgetBase widget)
        {
            Widgets[lastIndex++] = widget;
        }

        public abstract void OnResized();
        public abstract void Update();
        public abstract void HandleInput(ConsoleKeyInfo key);
        public virtual void OnEnter() { }
        public virtual void OnExit() { }

        public virtual void Render()
        {
            for (int i = 0; i < lastIndex; i++)
            {
                if (currentFocusingIndex == -1)
                {
                    if (Widgets[i] is IFocusable fw)
                    {
                        fw.Focus();
                        currentFocusingIndex = i;
                    }
                }

                if (Widgets[i].ShouldRerender)
                {
                    Widgets[i].OnRender();
                }
            }
        }
        
        internal void SwapNextFocus()
        {
            if (currentFocusingIndex == lastIndex - 1) return;

            IFocusable cfw = Widgets[currentFocusingIndex] as IFocusable;
            cfw.Unfocus();

            for (int i = currentFocusingIndex + 1; i < lastIndex; i++)
            {
                if (Widgets[i] is IFocusable fw)
                {
                    fw.Focus();
                    currentFocusingIndex = i;
                }
            }
        }

        internal void SwapPreviousFocus()
        {
            if (currentFocusingIndex == 0) return;

            IFocusable cfw = Widgets[currentFocusingIndex] as IFocusable;
            cfw.Unfocus();

            for (int i = currentFocusingIndex - 1; i >= 0; i--)
            {
                if (Widgets[i] is IFocusable fw)
                {
                    fw.Focus();
                    currentFocusingIndex = i;
                }
            }
        }
    }
}