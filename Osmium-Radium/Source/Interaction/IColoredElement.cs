namespace OsmiumRadium;

public interface IColoredElement
{
    /// <summary> Color of the element </summary>
    public Color _color { get; set; }
}

public interface IColoredElement<out TSelf> where TSelf : ImmediateElement, IColoredElement<TSelf>, IColoredElement;


public static class IColoredElementExtensions
{
    public static T Color<T>(this T element, Color color) where T : ImmediateElement, IColoredElement<T>, IColoredElement {
        element._color = color;

        return element;
    }
    
    public static T Color<T>(this T element, int r, int g, int b) where T : ImmediateElement, IColoredElement<T>, IColoredElement {
        element._color = OsmiumRadium.Color.FromRgb(r, g, b);

        return element;
    }
    
    public static T Color<T>(this T element, int r, int g, int b, int a) where T : ImmediateElement, IColoredElement<T>, IColoredElement {
        element._color = OsmiumRadium.Color.FromRgba(r, g, b, a);

        return element;
    }
}