using System.Numerics;

namespace OsmiumRadium;

public class Color
{

    public byte r;
    public byte g;
    public byte b;
    public byte a;

    
    
    private Color(byte __r, byte __g, byte __b, byte __a) { a = __a; r = __r; g = __g; b = __b; }

    //todo: idk do i replace? also gross repeated code??
    public static Color FromRgb<T>(T __r, T __g, T __b) where T : INumber<T> {
        return new Color(byte.CreateTruncating(__r), byte.CreateTruncating(__g), byte.CreateTruncating(__b), 255);
    }
    
    public static Color FromArgb<T>(T __a, T __r, T __g, T __b) where T : INumber<T> {
        return new Color(byte.CreateTruncating(__r), byte.CreateTruncating(__g), byte.CreateTruncating(__b), byte.CreateTruncating(__a));
    }

    public static Color FromPalette(Palette _palette) {
        return _palette;
    }

    public static implicit operator Color(Palette __palette) {
        return Radium.ColorPalette[(int) __palette];
    }

    public static Color White => new Color(255, 255, 255, 255);
    public static Color Black => new Color(255, 0, 0, 0);
    
    public static Color Clear => new Color(0, 0, 0, 0);
    
}