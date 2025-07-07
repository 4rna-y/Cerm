
using System;
using Cerm.Ui.Screen.Widget.Abstruct;

namespace Cerm.Ui.Widget
{
    public class Border : WidgetBase
    {
        internal readonly char[] BorderLeftTopCornerChar = ['┌', '╭', '╔'];
        internal readonly char[] BorderHorizontalLineChar = ['─', '─', '═'];
        internal readonly char[] BorderRightTopCornerChar = ['┐', '╮', '╗'];
        internal readonly char[] BorderVerticalLineChar = ['│', '│', '║'];
        internal readonly char[] BorderLeftBottomCornerChar = ['└', '╰', '╚'];
        internal readonly char[] BorderRightBottomCornerChar = ['┘', '╯', '╝'];
        internal readonly char[] BorderVerticalLineRightBranchChar = ['├', '├', '╠'];
        internal readonly char[] BorderVerticalLineLeftBranchChar = ['┤', '┤', '╣'];
        internal readonly char[] BorderHorizontalLineBottomBranchChar = ['┬', '┬', '╦'];
        internal readonly char[] BorderHorizontalLineTopBranchChar = ['┴', '┴', '╩'];
        internal readonly char[] BorderCrossLineChar = ['┼', '┼', '╬'];

        private BorderType borderType;
        public BorderType BorderType
        {
            get => borderType;
            set => SetProperty(ref borderType, value);
        }

        private ConsoleColor background;
        public ConsoleColor Background
        {
            get => background;
            set => SetProperty(ref background, value);
        }

        private ConsoleColor foreground;
        public ConsoleColor Foreground
        {
            get => foreground;
            set => SetProperty(ref foreground, value);
        }

        public Border(
            int x, int y,
            int width, int height,
            BorderType border,
            ConsoleColor backgroundColor,
            ConsoleColor foregroundColor
        ) : base(x, y, width, height)
        {
            BorderType = border;
            Background = backgroundColor;
            Foreground = foregroundColor;
        }

        public override void OnUpdate()
        {

        }

        public override void OnRender()
        {
            ShouldRerender = false;

            int lastX = this.Width - 1;
            int lastY = this.Height - 1;

            Console.BackgroundColor = Background;
            Console.ForegroundColor = Foreground;
            Console.SetCursorPosition(this.X, this.Y);

            for (int dy = 0; dy < this.Height; dy++)
            {
                Console.SetCursorPosition(this.X, this.Y + dy);
                for (int dx = 0; dx < this.Width; dx++)
                {
                    Console.SetCursorPosition(this.X + dx, this.Y + dy);
                    if (dy == 0)
                    {
                        if (dx == 0)
                            Console.Write(BorderLeftTopCornerChar[(int)BorderType]);
                        else
                        if (dx == lastX)
                            Console.Write(BorderRightTopCornerChar[(int)BorderType]);
                        else
                            Console.Write(BorderHorizontalLineChar[(int)BorderType]);
                    }
                    else
                    if (dy == lastY)
                    {
                        if (dx == 0)
                            Console.Write(BorderLeftBottomCornerChar[(int)BorderType]);
                        else
                        if (dx == lastX)
                            Console.Write(BorderRightBottomCornerChar[(int)BorderType]);
                        else
                            Console.Write(BorderHorizontalLineChar[(int)BorderType]);
                    }
                    else
                    {
                        if (dx == 0 || dx == lastX)
                            Console.Write(BorderVerticalLineChar[(int)BorderType]);
                        else
                            Console.Write(" ");
                    }
                }
            }
        }
    }
}