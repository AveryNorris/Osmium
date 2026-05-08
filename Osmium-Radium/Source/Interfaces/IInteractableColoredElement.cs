namespace OsmiumRadium;


public interface IInteractableColoredElement<out TSelf> where TSelf : IElement, IInteractableColoredElement<TSelf>
{
    /// <summary> Normal color of the element </summary>
    public Color _normalColor { get; set; }
    
    /// <summary> Color of the element when it is hovered </summary>
    public Color _hoverColor { get; set; }
    
    /// <summary> Color of the element when it is held </summary>
    public Color _heldColor { get; set; }
    
    /// <summary> Color of the element when it is first pressed </summary>
    public Color _downColor { get; set; }
    
    /// <summary> Color of the element when it is released </summary>
    public Color _upColor { get; set; }
}


public static class IInteractableColoredElement
{
    public static T ActiveColor<T>(this T element, Color color) where T : IElement, IInteractableColoredElement<T> {
        element._downColor = color;
        element._heldColor = color;
        element._upColor = color;

        return element;
    }
    
    public static T ActiveColor<T>(this T element, int r, int g, int b) where T : IElement, IInteractableColoredElement<T> {
        Color color = Color.FromRgb(r, g, b);
        
        element._downColor = color;
        element._heldColor = color;
        element._upColor = color;

        return element;
    }
    
    public static T ActiveColor<T>(this T element, int r, int g, int b, int a) where T : IElement, IInteractableColoredElement<T> {
        Color color = Color.FromRgba(r, g, b, a);
        
        element._downColor = color;
        element._heldColor = color;
        element._upColor = color;

        return element;
    }
    
    
    public static T HeldColor<T>(this T element, Color color) where T : IElement, IInteractableColoredElement<T> {
        element._heldColor = color;

        return element;
    }
    
    public static T HeldColor<T>(this T element, int r, int g, int b) where T : IElement, IInteractableColoredElement<T> {
        element._heldColor = Color.FromRgb(r, g, b);

        return element;
    }
    
    public static T HeldColor<T>(this T element, int r, int g, int b, int a) where T : IElement, IInteractableColoredElement<T> {
        element._heldColor = Color.FromRgba(r, g, b, a);

        return element;
    }
    
    public static T DownColor<T>(this T element, Color color) where T : IElement, IInteractableColoredElement<T> {
        element._downColor = color;

        return element;
    }
    
    public static T DownColor<T>(this T element, int r, int g, int b) where T : IElement, IInteractableColoredElement<T> {
        element._downColor = Color.FromRgb(r, g, b);

        return element;
    }
    
    public static T DownColor<T>(this T element, int r, int g, int b, int a) where T : IElement, IInteractableColoredElement<T> {
        element._downColor = Color.FromRgba(r, g, b, a);

        return element;
    }
    
    public static T UpColor<T>(this T element, Color color) where T : IElement, IInteractableColoredElement<T> {
        element._upColor = color;

        return element;
    }
    
    public static T UpColor<T>(this T element, int r, int g, int b) where T : IElement, IInteractableColoredElement<T> {
        element._upColor = Color.FromRgb(r, g, b);

        return element;
    }
    
    public static T UpColor<T>(this T element, int r, int g, int b, int a) where T : IElement, IInteractableColoredElement<T> {
        element._upColor = Color.FromRgba(r, g, b, a);

        return element;
    }
    
    
    
    public static T HoverColor<T>(this T element, Color color) where T : IElement, IInteractableColoredElement<T> {
        element._hoverColor = color;

        return element;
    }
    
    public static T HoverColor<T>(this T element, int r, int g, int b) where T : IElement, IInteractableColoredElement<T> {
        element._hoverColor = Color.FromRgb(r, g, b);

        return element;
    }
    
    public static T HoverColor<T>(this T element, int r, int g, int b, int a) where T : IElement, IInteractableColoredElement<T> {
        element._hoverColor = Color.FromRgba(r, g, b, a);

        return element;
    }
    
    
    
    public static T NormalColor<T>(this T element, Color color) where T : IElement, IInteractableColoredElement<T> {
        element._normalColor = color;

        return element;
    }
    
    public static T NormalColor<T>(this T element, int r, int g, int b) where T : IElement, IInteractableColoredElement<T> {
        element._normalColor = Color.FromRgb(r, g, b);

        return element;
    }
    
    public static T NormalColor<T>(this T element, int r, int g, int b, int a) where T : IElement, IInteractableColoredElement<T> {
        element._normalColor = Color.FromRgba(r, g, b, a);

        return element;
    }
    
    
    
    public static T AllColors<T>(this T element, Color color) where T : IElement, IInteractableColoredElement<T> {
        element._downColor = color;
        element._upColor = color;
        element._heldColor = color;
        element._hoverColor = color;
        element._normalColor = color;

        return element;
    }
    
    public static T AllColors<T>(this T element, int r, int g, int b) where T : IElement, IInteractableColoredElement<T> {
        Color color = Color.FromRgb(r, g, b);
        
        element._downColor = color;
        element._upColor = color;
        element._heldColor = color;
        element._hoverColor = color;
        element._normalColor = color;

        return element;
    }
    
    public static T AllColors<T>(this T element, int r, int g, int b, int a) where T : IElement, IInteractableColoredElement<T> {
        Color color = Color.FromRgba(r, g, b, a);
        
        element._downColor = color;
        element._upColor = color;
        element._heldColor = color;
        element._hoverColor = color;
        element._normalColor = color;

        return element;
    }
}