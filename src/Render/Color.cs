namespace Cerm.Render
{
    public struct Color
    {
        public static readonly Color White = new Color(255, 255, 255);
        public static readonly Color Black = new Color(0, 0, 0);

        public byte Red { get; }
        public byte Green { get; }
        public byte Blue { get; }
        private string foreground = "";
        private string background = "";

        public static Color FromHsv(int hue, double saturation, double value)
        {
            hue = hue % 360;
            if (saturation < 0) saturation = 0;
            if (saturation > 1) saturation = 1;
            if (value < 0) value = 0;
            if (value > 1) value = 1;

            if (value == 0)
            {
                return new Color(0, 0, 0);
            }

            if (saturation == 0)
            {
                byte gray = (byte)Math.Round(value * 255);
                return new Color(gray, gray, gray);
            }

            double c = value * saturation;
            double x = c * (1 - Math.Abs((hue / 60.0) % 2 - 1));
            double m = value - c;

            double r = 0, g = 0, b = 0;

            if (hue >= 0 && hue < 60)
            {
                r = c; g = x; b = 0;
            }
            else if (hue >= 60 && hue < 120)
            {
                r = x; g = c; b = 0;
            }
            else if (hue >= 120 && hue < 180)
            {
                r = 0; g = c; b = x;
            }
            else if (hue >= 180 && hue < 240)
            {
                r = 0; g = x; b = c;
            }
            else if (hue >= 240 && hue < 300)
            {
                r = x; g = 0; b = c;
            }
            else if (hue >= 300 && hue < 360)
            {
                r = c; g = 0; b = x;
            }

            byte red = (byte)Math.Round((r + m) * 255);
            byte green = (byte)Math.Round((g + m) * 255);
            byte blue = (byte)Math.Round((b + m) * 255);

            return new Color(red, green, blue);
        }

        public Color(byte red, byte green, byte blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
            foreground = $"\x1b[38;2;{this.Red};{this.Green};{this.Blue}m";
            background = $"\x1b[48;2;{this.Red};{this.Green};{this.Blue}m";
        }

        public string GetForeground() => foreground;
        public string GetBackground() => background;

        public Color AddBrightness(double value)
        {
            double r = Red / 255.0;
            double g = Green / 255.0;
            double b = Blue / 255.0;
            
            double max = Math.Max(r, Math.Max(g, b));
            double min = Math.Min(r, Math.Min(g, b));
            double delta = max - min;
            
            double hue = 0;
            if (delta != 0)
            {
                if (max == r)
                    hue = 60 * (((g - b) / delta) % 6);
                else if (max == g)
                    hue = 60 * ((b - r) / delta + 2);
                else if (max == b)
                    hue = 60 * ((r - g) / delta + 4);
            }
            
            if (hue < 0) hue += 360;
            
            double saturation = max == 0 ? 0 : delta / max;
            double newBrightness = Math.Max(0, Math.Min(1, max + value));
            
            return FromHsv((int)Math.Round(hue), saturation, newBrightness);
        }
    }
}