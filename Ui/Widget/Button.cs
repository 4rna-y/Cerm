

using System;
using Cerm.Ui.Screen.Widget.Abstruct;
using Cerm.Ui.Widget.Abstruct;

namespace Cerm.Ui.Widget
{
    public class Button : Border, IFocusable
    {
        private string text;
        public string Text
        {
            get => text;
            set => SetProperty(ref text, value);
        }

        private Action onClick;

        public bool IsFocused { get; set; } = false;
        public ConsoleColor UnfocusedBackground { get; set; }
        public ConsoleColor UnfocusedForeground { get; set; }
        public ConsoleColor FocusedBackground { get; set; }
        public ConsoleColor FocusedForeground { get; set; }

        public Button(
            string text,
            int x, int y,
            int width, int height,
            BorderType border,
            Action onClick
        ) : base(x, y, width, height, border, ConsoleColor.Black, ConsoleColor.DarkGray)
        {
            this.text = text;
            this.onClick = onClick;
            FocusedBackground = ConsoleColor.Black;
            FocusedForeground = ConsoleColor.White;
            UnfocusedBackground = ConsoleColor.Black;
            UnfocusedForeground = ConsoleColor.DarkGray;
        }

        public Button(
            string text,
            int x, int y,
            int width, int height,
            BorderType border,
            ConsoleColor focusedBackground, ConsoleColor focusedForeground,
            ConsoleColor unfocusedBackground, ConsoleColor unfocusedForeground,
            Action onClick
            ) : base(x, y, width, height, border, unfocusedBackground, unfocusedForeground)
        {
            this.text = text;
            this.onClick = onClick;
            FocusedBackground = focusedBackground;
            FocusedForeground = focusedForeground;
            UnfocusedBackground = unfocusedBackground;
            UnfocusedForeground = unfocusedForeground;
        }

        public void Focus()
        {
            IsFocused = true;
            this.Background = FocusedBackground;
            this.Foreground = FocusedForeground;
        }

        public void Unfocus()
        {
            IsFocused = false;
            this.Background = UnfocusedBackground;
            this.Foreground = UnfocusedForeground;
        }

        public override void OnRender()
        {
            if (this.ShouldRerender)
            {
                base.OnRender();

                int textSpaceWidth = this.Width - 2;
                string renderText = this.text;

                if (textSpaceWidth < renderText.Length)
                {
                    renderText = renderText.Substring(0, textSpaceWidth - 3);
                    renderText += "...";
                }

                int offset = (textSpaceWidth - renderText.Length) / 2;

                Console.SetCursorPosition(this.X + 1 + offset, this.Y + 1);
                
                Console.Write(renderText);
            }
        }

        public override void OnUpdate()
        {

        }

        public void HandleInput(ConsoleKeyInfo key)
        {
            if (key.Key == ConsoleKey.Enter)
            {
                onClick();
            }
        }
    }
}