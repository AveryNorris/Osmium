

using System.Numerics;

namespace OsmiumRadium;

public class Color
{
    
    public static List<Color> ColorPalette = [
        
        Color.FromRgb(0,255,255), //unkown
        
        Color.FromRgb(48, 122, 255), //primary
        Color.FromRgb(35, 68, 108), //secondary
        Color.FromRgb(66, 150, 250), //secondary hover
        Color.FromRgb(15, 135, 250), //secondary active
        
        Color.FromRgb(24, 24, 24), //background highest
        Color.FromRgb(20, 20, 20), //background high
        Color.FromRgb(16, 16, 16), //background medium
        Color.FromRgb(14, 14, 14), //background low
        
        Color.FromRgb(255, 255, 255), //_text high
        Color.FromRgb(50, 50, 50), //_text low
        
        Color.White,
        Color.Clear,
        
        //Box
        Color.White,
    ];
    
    
    public static void SetColor(Palette color, Color value) {
        ColorPalette[(int) color] = value;
    }


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
    
    public static Color FromRgba<T>(T __r, T __g, T __b, T __a) where T : INumber<T> {
        return new Color(byte.CreateTruncating(__r), byte.CreateTruncating(__g), byte.CreateTruncating(__b), byte.CreateTruncating(__a));
    }

    public static Color FromPalette(Palette _palette) {
        return _palette;
    }

    public static implicit operator Color(Palette __palette) {
        return ColorPalette[(int) __palette];
    }

    public static Color White => new Color(255, 255, 255, 255);
    public static Color Black => new Color(255, 0, 0, 0);
    
    public static Color Clear => new Color(0, 0, 0, 0);

    public static Color Error => Color.FromRgb(255, 0, 255);

    public override string ToString() {
        return "<R:" + r + ",G:" + g + ",B:" + b + ",A:" + a + ">";
    }

}