using System.Numerics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OsmiumNucleus;


namespace OsmiumRadium;




public interface IBoundedElement
{
    /// <summary> Bounds of the element; describes a 2D rectangle. </summary>
    public Bounds _bounds { get; set; }
}

/// <summary> Describes an element that has bounds. Several extension methods are provided which allow manipulation of said bounds </summary>
/// <typeparam name="TSelf"></typeparam>
public interface IBoundedElement<out TSelf> where TSelf : IElement, IBoundedElement<TSelf>, IBoundedElement
{
    
}



/// <summary> Provides extension methods to any element with bounds which allows for manipulation </summary>
public static class IBoundsExtensions
{
    
    
    
    
    
    /// <summary> Updates the bounds of a given element to a new value </summary>
    /// <param name="element"> Element to update </param>
    /// <param name="bounds"> Value to set the Element's bounds to </param>
    /// <returns> The element </returns>
    public static T Bounds<T>(this T element, Bounds bounds) where T : IElement, IBoundedElement<T>, IBoundedElement {
        element._bounds = bounds;
        
        return element;
    }
    
    
    
    
    
    /// <summary> Updates the size of a given element to a new value </summary>
    /// <param name="element"> Element to update </param>
    /// <param name="size"> Value to set the Element's size to </param>
    /// <returns> The element </returns>
    public static T Size<T>(this T element, Vector2 size) where T : IElement, IBoundedElement<T>, IBoundedElement {
        element._bounds = element._bounds with { size = size };
        
        return element;
    }
    
    
    /// <summary> Updates the size of a given element to a new value </summary>
    /// <param name="element"> Element to update </param>
    /// <param name="x"> X value of the size </param>
    /// <param name="y"> Y value of the size </param>
    /// <returns> The element </returns>
    public static T Size<T>(this T element, float x, float y) where T : IElement, IBoundedElement<T>, IBoundedElement {
        element._bounds = element._bounds with { size = new Vector2(x, y) };
        
        return element;
    }
    
    
    /// <summary> Updates the size of a given element to a new value </summary>
    /// <param name="element"> Element to update </param>
    /// <param name="v"> the X and Y of the new size </param>
    /// <returns> The element </returns>
    public static T Size<T>(this T element, float v) where T : IElement, IBoundedElement<T>, IBoundedElement {
        element._bounds = element._bounds with { size = new Vector2(v) };
        
        return element;
    }
    
    
    
    
    
    /// <summary> Updates the pos of a given element to a new value </summary>
    /// <param name="element"> Element to update </param>
    /// <param name="pos"> Value to set the Element's pos to </param>
    /// <returns> The element </returns>
    public static T Pos<T>(this T element, Vector2 pos) where T : IElement, IBoundedElement<T>, IBoundedElement {
        element._bounds = element._bounds with { pos = pos };
        
        return element;
    }
    
    
    /// <summary> Updates the pos of a given element to a new value </summary>
    /// <param name="element"> Element to update </param>
    /// <param name="x"> X value of the pos </param>
    /// <param name="y"> Y value of the pos </param>
    /// <returns> The element </returns>
    public static T Pos<T>(this T element, float x, float y) where T : IElement, IBoundedElement<T>, IBoundedElement {
        element._bounds = element._bounds with { pos = new Vector2(x, y) };
        
        return element;
    }
    
    
    /// <summary> Updates the pos of a given element to a new value </summary>
    /// <param name="element"> Element to update </param>
    /// <param name="v"> X and Y value of the pos </param>
    /// <returns> The element </returns>
    public static T Pos<T>(this T element, float v) where T : IElement, IBoundedElement<T>, IBoundedElement {
        element._bounds = element._bounds with { pos = new Vector2(v) };
        
        return element;
    }
    
    
    
    
    
    /// <summary> Updates the min of a given element to a new value </summary>
    /// <param name="element"> Element to update </param>
    /// <param name="min"> Value to set the Element's min to </param>
    /// <returns> The element </returns>
    public static T Min<T>(this T element, Vector2 min) where T : IElement, IBoundedElement<T>, IBoundedElement {
        element._bounds = element._bounds with { min = min };
        
        return element;
    }
    
    
    /// <summary> Updates the min of a given element to a new value </summary>
    /// <param name="element"> Element to update </param>
    /// <param name="x"> X of the min </param>
    /// <param name="y"> Y of the min </param>
    /// <returns> The element </returns>
    public static T Min<T>(this T element, float x, float y) where T : IElement, IBoundedElement<T>, IBoundedElement {
        element._bounds = element._bounds with { min = new Vector2(x, y) };
        
        return element;
    }
    
    
    /// <summary> Updates the min of a given element to a new value </summary>
    /// <param name="element"> Element to update </param>
    /// <param name="v"> X and Y of the min </param>
    /// <returns> The element </returns>
    public static T Min<T>(this T element, float v) where T : IElement, IBoundedElement<T>, IBoundedElement {
        element._bounds = element._bounds with { min = new Vector2(v) };
        
        return element;
    }
    
    
    
    
    
    /// <summary> Updates the max of a given element to a new value </summary>
    /// <param name="element"> Element to update </param>
    /// <param name="max"> Value to set the Elements max to </param>
    /// <returns> The element </returns>
    public static T Max<T>(this T element, Vector2 max) where T : IElement, IBoundedElement<T>, IBoundedElement {
        element._bounds = element._bounds with { max = max };
        
        return element;
    }
    
    
    /// <summary> Updates the max of a given element to a new value </summary>
    /// <param name="element"> Element to update </param>
    /// <param name="x"> X of the max </param>
    /// <param name="y"> Y of the max </param>
    /// <returns> The element </returns>
    public static T Max<T>(this T element, float x, float y) where T : IElement, IBoundedElement<T>, IBoundedElement {
        element._bounds = element._bounds with { max = new Vector2(x, y) };
        
        return element;
    }
    
    
    /// <summary> Updates the max of a given element to a new value </summary>
    /// <param name="element"> Element to update </param>
    /// <param name="v"> X and Y of the max </param>
    /// <returns> The element </returns>
    public static T Max<T>(this T element, float v) where T : IElement, IBoundedElement<T>, IBoundedElement {
        element._bounds = element._bounds with { max = new Vector2(v) };
        
        return element;
    }
    
    
    
    
    
    /// <summary> Updates the center of a given element to a new value </summary>
    /// <param name="element"> Element to update </param>
    /// <param name="center"> Value to set the Element's center to </param>
    /// <returns> The element </returns>
    public static T Center<T>(this T element, Vector2 center) where T : IElement, IBoundedElement<T>, IBoundedElement {
        element._bounds = element._bounds with { center = center };
        
        return element;
    }
    
    
    /// <summary> Updates the center of a given element to a new value </summary>
    /// <param name="element"> Element to update </param>
    /// <param name="x"> X of the center </param>
    /// <param name="y"> Y of the center </param>
    /// <returns> The element </returns>
    public static T Center<T>(this T element, float x, float y) where T : IElement, IBoundedElement<T>, IBoundedElement {
        element._bounds = element._bounds with { center = new Vector2(x, y) };
        
        return element;
    }
    
    
    /// <summary> Updates the center of a given element to a new value </summary>
    /// <param name="element"> Element to update </param>
    /// <param name="v"> X and Y of the center </param>
    /// <returns> The element </returns>
    public static T Center<T>(this T element, float v) where T : IElement, IBoundedElement<T>, IBoundedElement {
        element._bounds = element._bounds with { center = new Vector2(v) };
        
        return element;
    }
    
    
    
    
    public static bool MouseDown(this IBoundedElement element, MouseButton button) => 
            MouseInBounds(element) && Osmium.Context.MouseState.IsButtonPressed(button);
    
    public static bool MouseUp(this IBoundedElement element, MouseButton button) => 
            MouseInBounds(element) && Osmium.Context.MouseState.IsButtonReleased(button);

    public static bool MouseHeld(this IBoundedElement element, MouseButton button) => 
            MouseInBounds(element) && Osmium.Context.MouseState.IsButtonDown(button);

    public static bool MouseInBounds(this IBoundedElement element) =>
        Radium.MousePos.X >= element._bounds.min.X && Radium.MousePos.Y >= element._bounds.min.Y &&
        Radium.MousePos.X <= element._bounds.max.X && Radium.MousePos.Y <= element._bounds.max.Y
        && Radium.MousePos.X >= Radium.Clipping.X && Radium.MousePos.Y >= Radium.Clipping.Y &&
        Radium.MousePos.X <= Radium.Clipping.Z && Radium.MousePos.Y <= Radium.Clipping.W;
}