using Cerm.Render.Component;

namespace Cerm.Render.Interfaces
{
    public interface IFocusable
    {
        public Color FocusedBackground { get; }
        public Color FocusedForeground { get; }
        public Color UnfocusedBackground { get; }
        public Color UnfocusedForeground { get; } 
        public void Unfocus();
        public void Focus();
        
    }
}