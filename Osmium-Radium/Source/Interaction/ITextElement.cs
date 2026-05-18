


namespace OsmiumRadium;

public interface ITextElement
{
    /// <summary> Contents of the _text </summary>
    public string _text { get; set; }
    
    /// <summary> Font of the _text </summary>
    public Font _font { get; set; }
    
    //todo: add screen ratio to _spacing
    /// <summary> Spacing of the element; represented as a ratio to _text _size </summary>
    public Vector2 _spacing { get; set; }

    /// <summary> Size of the _text </summary>
    public float _textSize { get; set; }
    
    /// <summary> Anchor of the _text </summary>
    public TextAnchor _textAnchor { get; set; }
    
    /// <summary> Color of the _text </summary>
    public Color _textColor { get; set; }
}

public interface ITextElement<out TSelf> where TSelf : ImmediateElement, ITextElement<TSelf>, ITextElement;


public static class ITextElementExtensions
{
    
    public static T Text<T>(this T element, string text) where T : ImmediateElement, ITextElement<T>, ITextElement {
        element._text = text;

        return element;
    }
    
    public static T Text<T>(this T element, char c) where T : ImmediateElement, ITextElement<T>, ITextElement {
        element._text = c.ToString();

        return element;
    }
    
    public static T Font<T>(this T element, Font font) where T : ImmediateElement, ITextElement<T>, ITextElement {
        element._font = font;

        return element;
    }
    
    public static T Spacing<T>(this T element, Vector2 spacing) where T : ImmediateElement, ITextElement<T>, ITextElement {
        element._spacing = spacing;

        return element;
    }
    
    public static T Spacing<T>(this T element, float x, float y) where T : ImmediateElement, ITextElement<T>, ITextElement {
        element._spacing = new Vector2(x, y);

        return element;
    }
    
    public static T TextSize<T>(this T element, float size) where T : ImmediateElement, ITextElement<T>, ITextElement {
        element._textSize = size;

        return element;
    }
    
    public static T TextAnchor<T>(this T element, TextAnchor textAnchor) where T : ImmediateElement, ITextElement<T>, ITextElement {
        element._textAnchor = textAnchor;

        return element;
    }
    
    public static T TextColor<T>(this T element, Color color) where T : ImmediateElement, ITextElement<T>, ITextElement {
        element._textColor = color;

        return element;
    }
    
    public static T TextColor<T>(this T element, int r, int g, int b) where T : ImmediateElement, ITextElement<T>, ITextElement {
        element._textColor = Color.FromRgb(r, g, b);

        return element;
    }
    
    public static T TextColor<T>(this T element, int r, int g, int b, int a) where T : ImmediateElement, ITextElement<T>, ITextElement {
        element._textColor = Color.FromRgba(r, g, b, a);

        return element;
    }
}