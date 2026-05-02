using System.Diagnostics;
using System.Numerics;
using OsmiumRadium.Source.Interfaces;
using Debug = OsmiumNucleus.Debug;


namespace OsmiumRadium;



/// <summary> Describes a box drawn for one frame </summary>
public class TextData : Element, IBoundedElement<TextData>, ITextElement<TextData>
{
    
    private static readonly Vector2[] Orientations = [
        new Vector2(0,0),
        new Vector2(1, 0),
        new Vector2(0, 1),
        new Vector2(1, 1),
        new Vector2(0, .5f),
        new Vector2(1, .5f),
        new Vector2(.5f, 0),
        new Vector2(.5f, 1),
        new Vector2(.5f, .5f),
    ];

    private static readonly Vector2[] BoundsOffsets = [
        new Vector2(0, 0),
        new Vector2(-1, 0),
        new Vector2(0, -1),
        new Vector2(-1, -1),
        new Vector2(0, -.5f),
        new Vector2(-1, -.5f),
        new Vector2(-.5f, 0),
        new Vector2(-.5f, -1),
        new Vector2(-.5f, -.5f),
    ];
    
    
    
    /// <inheritdoc cref="IBoundedElement{TSelf}._bounds"/>
    public Bounds _bounds { get; set; }
    
    /// <inheritdoc cref="IColoredElement{TSelf}._textColor"/>
    public Color _textColor { get; set; }
    
    
    
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
    
    
    //todo: figure out default colors
    /// <summary> Default _color of the _text unless explicitly stated otherwise </summary>
    public static Color DefaultColor = Color.Error;
    
    public static Font DefaultFont = Backend.BaseFont;

    internal TextData() {
        _bounds = new Bounds();
        _font = DefaultFont;
        _textColor = DefaultColor;
        _spacing = Vector2.Zero;
        _textSize = 1;
        _text = string.Empty;
    }
    
    //todo: make text spacing a ratio of text size and screen proportions relative to the ideal ratio. and optimize it
    protected internal override void Draw() {
        List<float> lengths = [_spacing.X];
        float yBound = _spacing.Y * _textSize;
        
        int lineBreaks = 0;
        for(int i = 0; i < _text.Length; i++) {
            if (_text[i] != '\n') {
                lengths[lineBreaks] += _spacing.X * _textSize;
            } else {
                lineBreaks++;
                lengths.Add(_spacing.X);
                yBound += _spacing.Y * _textSize;
            }
        }
        
        
        lineBreaks = 0;
        
        //todo: buffer _text a little bit? kind of on the edge
        
        Vector2 basePos = _bounds.pos 
                          + (_bounds.size * Orientations[(int)_anchor]) 
                          + (new Vector2(lengths[0], yBound) * BoundsOffsets[(int)_anchor]);
        
        Vector2 currentPos = basePos;

        List<float> vertexData = [];

        Radium.SetClippingBounds(_bounds);
        
        for(int i = 0; i < _text.Length; i++) {
            
            if (_text[i] == '\n') {
                
                lineBreaks++;
    
                currentPos = basePos
                             + new Vector2(
                                 (lengths[lineBreaks] - lengths[0]) * BoundsOffsets[(int)_anchor].X,
                                 lineBreaks * _spacing.Y * _textSize
                             );
                
                continue;
            }
            
            //todo: osmium.allchildren is a function not a property. same for scene

            Vector2 max = new Vector2(
                currentPos.X + _textSize / Backend.WindowWidthHeightRatio,
                currentPos.Y + _textSize
            );
            
            Vector4 uv = _font[_text[i]];   

            Backend.DrawElement(_font.texture.Handle, _textColor,
                max.X, max.Y, uv.Z, uv.W,
                currentPos.X, max.Y, uv.X, uv.W,
                max.X, currentPos.Y, uv.Z, uv.Y,
                currentPos.X, currentPos.Y, uv.X, uv.Y
            );
            
            currentPos.X += _spacing.X * _textSize;
        }
        
        //todo: draw glyphs in bulk like so! bad though annoying to make
        
        Radium.SetClippingBounds(new Bounds(max: new Vector2(100)));
    }
}