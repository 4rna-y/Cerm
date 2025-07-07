using System;
using System.Diagnostics;
using Cerm.Ui.Manager;
using Cerm.Ui.Screen.Abstruct;
using Cerm.Ui.Widget;
using Cerm.Ui.Widget.Abstruct;
using Cerm.Ui.Widget.Utils;

namespace Cerm.Ui.Screen
{
    public class WelcomeScreen : ScreenBase
    {
        private readonly IScreenManager screen;

        public WelcomeScreen(IScreenManager screen)
        {
            this.screen = screen;
        }

        public override void OnEnter()
        {
            Console.Clear();

            string title = "Cerm";
            int titleWidth = title.Length;

            //Console.SetCursorPosition((this.Width / 2) - (titleWidth / 2), (this.Height / 2) - (1 / 2));
            //Console.WriteLine(title);

            this.Append(new Button("I love this button", WidgetUtils.GetCenteredX(30), 3, 30, 3, BorderType.Round, ConsoleColor.Black, ConsoleColor.White, ConsoleColor.Black, ConsoleColor.DarkGray, () => { }));
            this.Append(new Button("I love this shit!", WidgetUtils.GetCenteredX(30), 7, 30, 3, BorderType.Round, ConsoleColor.Black, ConsoleColor.White, ConsoleColor.Black, ConsoleColor.DarkGray, () => { }));
        }

        public override void OnResized()
        {

        }

        public override void Update()
        {
            
        }

        public override void HandleInput(ConsoleKeyInfo key)
        {
            if (key.Modifiers == ConsoleModifiers.Control && key.Key == ConsoleKey.C)
            {
                Console.Clear();
                screen.IsRunning = false;
            }
            else
            if (key.Key == ConsoleKey.DownArrow)
            {
                SwapNextFocus();
            }
            else
            if (key.Key == ConsoleKey.UpArrow)
            {
                SwapPreviousFocus();
            }
        }
    }
}