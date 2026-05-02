using System.Diagnostics;
using System.Numerics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OsmiumNucleus;
using OsmiumRadium.Source.Interfaces;
using Debug = System.Diagnostics.Debug;


namespace OsmiumRadium;



/// <summary> Describes a box drawn for one frame </summary>
public class ButtonData : Element, IBoundedElement<ButtonData>, ITextElement<ButtonData>, IInteractableColoredElement<ButtonData>
{
    
    
    public Color _normalColor { get; set; }
    public Color _hoverColor { get; set; }
    public Color _heldColor { get; set; }
    public Color _downColor { get; set; }
    public Color _upColor { get; set; }
    
    
    
    /// <inheritdoc cref="IBoundedElement{TSelf}._bounds"/>
    public Bounds _bounds { get; set; }
    
    
    /// <inheritdoc cref="ITextElement{TSelf}._text"/>
    public string _text { get; set; }
    
    /// <inheritdoc cref="ITextElement{TSelf}._font"/>
    public Font _font { get; set; }

    /// <inheritdoc cref="ITextElement{TSelf}._spacing"/>
    public Vector2 _spacing { get; set; }
    
    /// <inheritdoc cref="ITextElement{TSelf}._textSize"/>
    public float _textSize { get; set; }

    /// <inheritdoc cref="ITextElement{TSelf}._anchor"/>
    public Source.Interfaces.Anchor _anchor { get; set; }
    
    /// <summary> The mouse button the button should listen for</summary>
    public MouseButton _button { get; set; }
    
    public Color _textColor { get; set; }

    
    
    //todo: figure out default colors
    /// <summary> Default _color of the _text unless explicitly stated otherwise </summary>
    public static Font DefaultFont = Backend.BaseFont;

    public static Color DefaultNormalColor = Color.Error;
    
    public static Color DefaultDownColor = Color.Error;
    
    public static Color DefaultUpColor = Color.Error;
    
    public static Color DefaultHeldColor = Color.Error;
    
    public static Color DefaultHoverColor = Color.Error;
    
    public static Color DefaultTextColor = Color.Error;

    public static Color DefaultActiveColor {
        set {
            DefaultDownColor = value;
            DefaultHeldColor = value;
            DefaultUpColor = value;
        }
    }

    internal ButtonData() {
        _bounds = new Bounds();
        _font = DefaultFont;
        _normalColor = DefaultNormalColor;
        _hoverColor = DefaultHoverColor;
        _downColor = DefaultDownColor;
        _upColor = DefaultUpColor;
        _heldColor = DefaultHeldColor;
        _textColor = DefaultTextColor;
        _spacing = Vector2.Zero;
        _textSize = 1; 
        _text = string.Empty;
    }

    public bool Down() => _bounds.MouseDown(_button);

    public bool Up() => _bounds.MouseUp(_button);

    public bool Held() => _bounds.MouseHeld(_button);
    
    public bool Hover() => _bounds.MouseInBounds();
    
    public bool Active() => Down() || Up() || Held();
    
    
     
    //todo: make text spacing a ratio of text size and screen proportions relative to the ideal ratio. and optimize it
    protected internal override void Draw() {
        
        Color boxColor = _normalColor;
        
        if (Down()) boxColor = _downColor;
        else if (Up()) boxColor = _upColor;
        else if (Held()) boxColor = _heldColor;
        else if (Hover()) boxColor = _hoverColor;
        
        //85
        
        new BoxData().Bounds(_bounds).Color(boxColor).Draw();
        new TextData().Bounds(_bounds).Text(_text).TextColor(_textColor).Font(_font).Spacing(_spacing).Anchor(_anchor).TextSize(_textSize).Draw();
    }
}