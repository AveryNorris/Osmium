using MouseButton = OpenTK.Windowing.GraphicsLibraryFramework.MouseButton;
using Rect = OsmiumRadium.Rect;
using OsmiumNucleus;


namespace OsmiumRadium;




public interface IBoundedElement
{
    /// <summary> Bounds of the element; describes a 2D rectangle. </summary>
    public Rect Rect { get; set; }
}



/// <summary> Provides extension methods to any element with bounds which allows for manipulation </summary>
public static class IRectElement
{
    
    
    
    
    
    /// <summary> Updates the bounds of a given element to a new value </summary>
    /// <param name="element"> Element to update </param>
    /// <param name="rect"> Value to set the Element's bounds to </param>
    /// <returns> The element </returns>
    public static T Rect<T>(this T element, Rect rect) where T : ImmediateElement, IBoundedElement {
        element.Rect = rect;
        
        return element;
    }
    
    
    
    
    
    /// <summary> Updates the size of a given element to a new value </summary>
    /// <param name="element"> Element to update </param>
    /// <param name="size"> Value to set the Element's size to </param>
    /// <returns> The element </returns>
    public static T Size<T>(this T element, Vector2 size) where T : ImmediateElement, IBoundedElement {
        element.Rect = element.Rect with { size = size };
        
        return element;
    }
    
    
    /// <summary> Updates the size of a given element to a new value </summary>
    /// <param name="element"> Element to update </param>
    /// <param name="x"> X value of the size </param>
    /// <param name="y"> Y value of the size </param>
    /// <returns> The element </returns>
    public static T Size<T>(this T element, float x, float y) where T : ImmediateElement, IBoundedElement {
        element.Rect = element.Rect with { size = new Vector2(x, y) };
        
        return element;
    }
    
    
    /// <summary> Updates the size of a given element to a new value </summary>
    /// <param name="element"> Element to update </param>
    /// <param name="v"> the X and Y of the new size </param>
    /// <returns> The element </returns>
    public static T Size<T>(this T element, float v) where T : ImmediateElement, IBoundedElement {
        element.Rect = element.Rect with { size = new Vector2(v) };
        
        return element;
    }
    
    
    
    
    
    /// <summary> Updates the pos of a given element to a new value </summary>
    /// <param name="element"> Element to update </param>
    /// <param name="pos"> Value to set the Element's pos to </param>
    /// <returns> The element </returns>
    public static T Pos<T>(this T element, Vector2 pos) where T : ImmediateElement, IBoundedElement {
        element.Rect = element.Rect with { pos = pos };
        
        return element;
    }
    
    
    /// <summary> Updates the pos of a given element to a new value </summary>
    /// <param name="element"> Element to update </param>
    /// <param name="x"> X value of the pos </param>
    /// <param name="y"> Y value of the pos </param>
    /// <returns> The element </returns>
    public static T Pos<T>(this T element, float x, float y) where T : ImmediateElement, IBoundedElement {
        element.Rect = element.Rect with { pos = new Vector2(x, y) };
        
        return element;
    }
    
    
    /// <summary> Updates the pos of a given element to a new value </summary>
    /// <param name="element"> Element to update </param>
    /// <param name="v"> X and Y value of the pos </param>
    /// <returns> The element </returns>
    public static T Pos<T>(this T element, float v) where T : ImmediateElement, IBoundedElement {
        element.Rect = element.Rect with { pos = new Vector2(v) };
        
        return element;
    }
    
    
    
    
    
    /// <summary> Updates the center of a given element to a new value </summary>
    /// <param name="element"> Element to update </param>
    /// <param name="center"> Value to set the Element's center to </param>
    /// <returns> The element </returns>
    public static T Center<T>(this T element, Vector2 center) where T : ImmediateElement, IBoundedElement {
        element.Rect = element.Rect with { center = center };
        
        return element;
    }
    
    
    /// <summary> Updates the center of a given element to a new value </summary>
    /// <param name="element"> Element to update </param>
    /// <param name="x"> X of the center </param>
    /// <param name="y"> Y of the center </param>
    /// <returns> The element </returns>
    public static T Center<T>(this T element, float x, float y) where T : ImmediateElement, IBoundedElement {
        element.Rect = element.Rect with { center = new Vector2(x, y) };
        
        return element;
    }
    
    
    /// <summary> Updates the center of a given element to a new value </summary>
    /// <param name="element"> Element to update </param>
    /// <param name="v"> X and Y of the center </param>
    /// <returns> The element </returns>
    public static T Center<T>(this T element, float v) where T : ImmediateElement, IBoundedElement {
        element.Rect = element.Rect with { center = new Vector2(v) };
        
        return element;
    }

    
    
    public static T Corners<T>(this T element, Vector2 a, Vector2 b) where T : ImmediateElement, IBoundedElement {
        element.Rect = OsmiumRadium.Rect.FromCorners(a, b);
        
        return element;
    }
    
    
    public static T Corners<T>(this T element, float ax, float ay, Vector2 b) where T : ImmediateElement, IBoundedElement {
        element.Rect = OsmiumRadium.Rect.FromCorners(ax, ay, b);
        
        return element;
    }
    
    
    public static T Corners<T>(this T element, Vector2 a, float bx, float by) where T : ImmediateElement, IBoundedElement {
        element.Rect = OsmiumRadium.Rect.FromCorners(a, bx, by);
        
        return element;
    }
    
    public static T Corners<T>(this T element, float ax, float ay, float bx, float by) where T : ImmediateElement, IBoundedElement {
        element.Rect = OsmiumRadium.Rect.FromCorners(ax, ay, bx, by);
        
        return element;
    }



    public static bool MouseDown(this IBoundedElement element, MouseButton button) => element.Rect.MouseDown(button);
    
    public static bool MouseUp(this IBoundedElement element, MouseButton button) => element.Rect.MouseUp(button);

    public static bool MouseHeld(this IBoundedElement element, MouseButton button) => element.Rect.MouseHeld(button);

    public static bool MouseInBounds(this IBoundedElement element) => element.Rect.MouseInBounds();
}