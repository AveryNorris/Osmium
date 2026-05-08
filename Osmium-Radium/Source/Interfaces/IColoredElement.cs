namespace OsmiumRadium;


public interface IColoredElement<out TSelf> where TSelf : IElement, IColoredElement<TSelf>
{
    /// <summary> Color of the element </summary>
    public Color _color { get; set; }
}


public static class IColoredElementExtensions
{
    public static T Color<T>(this T element, Color color) where T : IElement, IColoredElement<T> {
        element._color = color;

        return element;
    }
    
    public static T Color<T>(this T element, int r, int g, int b) where T : IElement, IColoredElement<T> {
        element._color = OsmiumRadium.Color.FromRgb(r, g, b);

        return element;
    }
    
    public static T Color<T>(this T element, int r, int g, int b, int a) where T : IElement, IColoredElement<T> {
        element._color = OsmiumRadium.Color.FromRgba(r, g, b, a);

        return element;
    }
}