using System.Diagnostics;
using System.Numerics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OsmiumNucleus;
using Debug = System.Diagnostics.Debug;


namespace OsmiumRadium;



/// <summary> Describes a box drawn for one frame </summary>
public class Button : IElement, IBoundedElement, IBoundedElement<Button>, ITextElement<Button>, IInteractableColoredElement<Button>
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

    /// <inheritdoc cref="ITextElement{TSelf}._textAnchor"/>
    public TextAnchor _textAnchor { get; set; }
    
    /// <summary> The mouse button the button should listen for</summary>
    public MouseButton _mouseButton { get; set; }
    
    public Color _textColor { get; set; }
    
    

    public Button MouseButton(MouseButton mouseButton) {
        _mouseButton = mouseButton;
        return this;
    }

    internal Button() {
        _bounds = new Bounds();
        _font = Backend.BaseFont;
        
        _normalColor = Palette.Secondary;
        _hoverColor = Palette.SecondaryHover;
        _downColor = Palette.SecondaryActive;
        _upColor = Palette.SecondaryActive;
        _heldColor = Palette.SecondaryActive;
        _textColor = Palette.TextHigh;
        
        //todo: find a good default spacing once code compiles
        _spacing = new Vector2(.3f, 1);
        
        _textSize = 5; 
        _text = string.Empty;
    }

    public bool Down() => this.MouseDown(_mouseButton);

    public bool Up() => this.MouseUp(_mouseButton);

    public bool Held() => this.MouseHeld(_mouseButton);
    
    public bool Hover() => this.MouseInBounds();
    
    public bool Active() => Down() || Up() || Held();
    
    
    
    public void Draw() {
        
        Color boxColor = _normalColor;
        
        if (Down()) boxColor = _downColor;
        else if (Up()) boxColor = _upColor;
        else if (Held()) boxColor = _heldColor;
        else if (Hover()) boxColor = _hoverColor;
        
        //85
        
        new Box().Bounds(_bounds).Color(boxColor).Draw();
        new TextBox().Bounds(_bounds).Text(_text).TextColor(_textColor).Font(_font).Spacing(_spacing).TextAnchor(_textAnchor).TextSize(_textSize).Draw();
    }
}