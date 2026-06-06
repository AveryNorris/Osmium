using System.Diagnostics;
using OpenTK.Windowing.Common.Input;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OsmiumNucleus;
using Debug = System.Diagnostics.Debug;


namespace OsmiumRadium;



/// <summary> Describes a button drawn for one frame, that comprises a Box and a Textbox inside </summary>
public class Button : ImmediateElement, IBoundedElement, ITextElement, IInteractableColoredElement
{
    
    
    
    public Color _normalColor { get; set; }
    public Color _hoverColor { get; set; }
    public Color _heldColor { get; set; }
    public Color _downColor { get; set; }
    public Color _upColor { get; set; }
    
    
    
    /// <inheritdoc cref="IBoundedElement.Rect"/>
    public Rect Rect { get; set; }
    
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
    
    /// <inheritdoc cref="ITextElement._textColor"/>
    public Color _textColor { get; set; }

    
    

    public Button MouseButton(MouseButton mouseButton) {
        _mouseButton = mouseButton;
        return this;
    }

    internal Button() {
        Rect = new Rect();
        _font = Backend.DefaultFont;
        
        _normalColor = Palette.Secondary;
        _hoverColor = Palette.Secondary | Palette.Selected;
        _downColor = Palette.Secondary | Palette.Active;
        _upColor = Palette.Secondary | Palette.Active;
        _heldColor = Palette.Secondary | Palette.Active;
        _textColor = Palette.Text;
        
        //todo: find a good default spacing once code compiles
        _spacing = new Vector2(.4f, 1);
        
        _textSize = 5; 
        _text = string.Empty;
    }

    public bool Down() => this.MouseDown(_mouseButton);

    public bool Up() => this.MouseUp(_mouseButton);

    public bool Held() => this.MouseHeld(_mouseButton);
    
    public bool Hover() => this.MouseInBounds();
    
    public bool Active() => Down() || Up() || Held();


    protected internal override void Draw() {
        
        Color boxColor = _normalColor;
        
        //todo: make cursor manager class and increase API

        if (Down()) {
            Osmium.Window.Cursor = MouseCursor.Default;
            boxColor = _downColor;
        }
        else if (Up()) {
            boxColor = _upColor;
        }
        else if (Held()) {
            boxColor = _heldColor;
        }
        else if (Hover())
        {
            Osmium.Window.Cursor = MouseCursor.Default;
            boxColor = _hoverColor;
        }
        
        new Box().Rect(Rect).Color(boxColor).Depth(_depth).Draw();
        new TextBox().Rect(Rect).Text(_text).TextColor(_textColor).Font(_font).Spacing(_spacing).TextAnchor(_textAnchor).TextSize(_textSize).Depth(_depth).Draw();
        
        //todo: put unit tests in seperate projects for each core library, in a solution folder NOT a normal directory but a solution folder!
    }
}