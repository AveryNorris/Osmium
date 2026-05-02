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

    public static void DefaultColor(Palette color, Color value) {
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

    public static void SetClippingBounds(Vector2 __min, Vector2 __max) {
        Backend.ClippingRects.Add((Backend.elementCount, new Vector4(__min.X, __min.Y, __max.X, __max.Y)));
    }
    
    public static void SetClippingBounds(Bounds __bounds) {
        Backend.SetClipping(new Vector4(__bounds.min.X, __bounds.min.Y, __bounds.max.X, __bounds.max.Y));
    }
}