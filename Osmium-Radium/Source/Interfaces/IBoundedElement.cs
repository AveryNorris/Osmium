using System.Numerics;


namespace OsmiumRadium.Source.Interfaces;


public interface IBoundedElement<out TSelf> where TSelf : Element, IBoundedElement<TSelf>
{
    /// <summary> Bounds of the element; describes a 2D rectangle. </summary>
    public Bounds _bounds { get; set; }
}


public static class IBoundsExtensions
{
    public static T Bounds<T>(this T element, Bounds bounds) where T : Element, IBoundedElement<T> {
        element._bounds = bounds;
        
        return element;
    }
    
    public static T Size<T>(this T element, Vector2 size) where T : Element, IBoundedElement<T> {
        element._bounds = element._bounds with { size = size };
        
        return element;
    }
    
    public static T Size<T>(this T element, float x, float y) where T : Element, IBoundedElement<T> {
        element._bounds = element._bounds with { size = new Vector2(x, y) };
        
        return element;
    }
    
    public static T Size<T>(this T element, float v) where T : Element, IBoundedElement<T> {
        element._bounds = element._bounds with { size = new Vector2(v) };
        
        return element;
    }
    
    public static T Pos<T>(this T element, Vector2 pos) where T : Element, IBoundedElement<T> {
        element._bounds = element._bounds with { pos = pos };
        
        return element;
    }
    
    public static T Pos<T>(this T element, float x, float y) where T : Element, IBoundedElement<T> {
        element._bounds = element._bounds with { pos = new Vector2(x, y) };
        
        return element;
    }
    
    public static T Pos<T>(this T element, float v) where T : Element, IBoundedElement<T> {
        element._bounds = element._bounds with { pos = new Vector2(v) };
        
        return element;
    }
    
    public static T Min<T>(this T element, Vector2 min) where T : Element, IBoundedElement<T> {
        element._bounds = element._bounds with { min = min };
        
        return element;
    }
    
    public static T Min<T>(this T element, float x, float y) where T : Element, IBoundedElement<T> {
        element._bounds = element._bounds with { min = new Vector2(x, y) };
        
        return element;
    }
    
    public static T Min<T>(this T element, float v) where T : Element, IBoundedElement<T> {
        element._bounds = element._bounds with { min = new Vector2(v) };
        
        return element;
    }
    
    public static T Max<T>(this T element, Vector2 max) where T : Element, IBoundedElement<T> {
        element._bounds = element._bounds with { max = max };
        
        return element;
    }
    
    public static T Max<T>(this T element, float x, float y) where T : Element, IBoundedElement<T> {
        element._bounds = element._bounds with { max = new Vector2(x, y) };
        
        return element;
    }
    
    public static T Max<T>(this T element, float v) where T : Element, IBoundedElement<T> {
        element._bounds = element._bounds with { max = new Vector2(v) };
        
        return element;
    }
    
    public static T Center<T>(this T element, Vector2 center) where T : Element, IBoundedElement<T> {
        element._bounds = element._bounds with { center = center };
        
        return element;
    }
    
    public static T Center<T>(this T element, float x, float y) where T : Element, IBoundedElement<T> {
        element._bounds = element._bounds with { center = new Vector2(x, y) };
        
        return element;
    }
    
    public static T Center<T>(this T element, float v) where T : Element, IBoundedElement<T> {
        element._bounds = element._bounds with { center = new Vector2(v) };
        
        return element;
    }
}