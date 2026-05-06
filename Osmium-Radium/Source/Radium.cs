using System.Numerics;
using OsmiumRadium;


public static class Radium
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
    
    public static Vector2 BoundsMin { get; private set; }
    public static Vector2 BoundsMax { get; private set; }

    public static void SetColor(Palette color, Color value) {
        ColorPalette[(int) color] = value;
    }
    
    //todo: file customization for palettes?

    public static RadiumElement Add<T>() where T : RadiumElement, new() {
        T newElement = new T();
        Backend.RetainedElements.Add(newElement);
        
        return newElement;
    }

    public static void RemoveElement(RadiumElement __element) => Backend.RetainedElements.Remove(__element);
    
    public static void Remove<T>() where T : RadiumElement, new() {
        Backend.RetainedElements.Remove(Backend.RetainedElements.First(x => x.GetType() == typeof(T)));
    }
    
    public static void SetClippingBounds(Bounds __bounds) {
        BoundsMin = __bounds.min;
        BoundsMax = __bounds.max;
        
        Vector4 value = new Vector4(__bounds.min.X, __bounds.min.Y, __bounds.max.X, __bounds.max.Y);
        int index = Backend.IMGUIElements.Count - 1;
        
        if (!Backend.ClippingRects.TryAdd(index, value)) {
            Backend.ClippingRects[index] = value;
        }
    }

    public static void RevertSubclippingBounds() {
        if (Subclips.Count > 0)
        {
            SetClippingBounds(Subclips[^1]);
            Subclips.RemoveAt(Subclips.Count - 1);
        }
    }

    private static List<Bounds> Subclips = [];

    public static void Subclip(Bounds __bounds) {
        Vector2 OverlapMin = new Vector2(MathF.Max(__bounds.min.X, BoundsMin.X), MathF.Max(__bounds.min.Y, BoundsMin.Y));
        Vector2 OverlapMax = new Vector2(MathF.Min(__bounds.max.X, BoundsMax.X), MathF.Min(__bounds.max.Y, BoundsMax.Y));
        
        Subclips.Add(new Bounds(min: BoundsMin, max: BoundsMax));
        SetClippingBounds(new Bounds(min: OverlapMin, max: OverlapMax));
    }

    public static bool InsideClippingBounds(Vector2 __point) {
        return __point.X >= BoundsMin.X && __point.X <= BoundsMax.X && __point.Y >= BoundsMin.Y && __point.Y <= BoundsMax.Y;
    }
}