using System;

namespace Cerm.Ui.Widget.Abstruct
{
    public interface IFocusable : IInputable
    {
        bool IsFocused { get; }
        ConsoleColor UnfocusedBackground { get; }
        ConsoleColor UnfocusedForeground { get; }
        ConsoleColor FocusedBackground { get; }
        ConsoleColor FocusedForeground { get; }
        void Focus();
        void Unfocus();  
    }
}