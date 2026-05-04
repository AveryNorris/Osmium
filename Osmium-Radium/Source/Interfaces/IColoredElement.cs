namespace OsmiumRadium;


public interface IColoredElement<out TSelf> where TSelf : Element, IColoredElement<TSelf>
{
    /// <summary> Color of the element </summary>
    public Color _color { get; set; }
}


public static class IColoredElementExtensions
{
    public static T Color<T>(this T element, Color color) where T : Element, IColoredElement<T> {
        element._color = color;

        return element;
    }
    
    public static T Color<T>(this T element, int r, int g, int b) where T : Element, IColoredElement<T> {
        element._color = OsmiumRadium.Color.FromRgb(r, g, b);

        return element;
    }
    
    public static T Color<T>(this T element, int r, int g, int b, int a) where T : Element, IColoredElement<T> {
        element._color = OsmiumRadium.Color.FromRgba(r, g, b, a);

        return element;
    }
}