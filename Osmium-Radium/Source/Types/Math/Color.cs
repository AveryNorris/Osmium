using System.Numerics;
using OsmiumNucleus;

namespace OsmiumRadium;

public struct Color
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
    
    public static Color FromRgba<T>(T __r, T __g, T __b, T __a) where T : INumber<T> {
        return new Color(byte.CreateTruncating(__r), byte.CreateTruncating(__g), byte.CreateTruncating(__b), byte.CreateTruncating(__a));
    }

    public static Color FromPalette(Palette __palette) {
        if (PaletteDictionary.ColorDictionary.TryGetValue(__palette, out Color color)) {
            return color;
        }else if (PaletteDictionary.ColorDictionary.TryGetValue(__palette, out Color error)) {
            Debug.Error("Failed to find color inside the palette dictionary! ", ["Color"], [__palette.ToString()]);
            return error;
        }
        
        Debug.Error("Color not found in PaletteDictionary! NOR is there a backing error color!", ["Color"], [__palette.ToString()]);
        return Color.Error;    
    }

    
    
    public static implicit operator Color(Palette __palette) => FromPalette(__palette);

    public static Color Error => FromRgb(255, 0, 255);

    public override string ToString() => "<R:" + r + ",G:" + g + ",B:" + b + ",A:" + a + ">";

}