using System.Diagnostics;

using Debug = OsmiumNucleus.Debug;


namespace OsmiumRadium;



/// <summary> Describes a box drawn for one frame </summary>
public class TextBox : ImmediateElement, IBoundedElement, IBoundedElement<TextBox>, ITextElement<TextBox>, ITextElement, IDepthElement<TextBox>, IDepthElement
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

    /// <inheritdoc cref="ITextElement{TSelf}._textAnchor"/>
    public TextAnchor _textAnchor { get; set; }
    
    /// <inheritdoc cref="IDepthElement._depth"/>
    public float _depth { get; set; }
    
    
    //todo: figure out default colors
    /// <summary> Default _color of the _text unless explicitly stated otherwise </summary>
    public static Color DefaultColor = Color.Error;
    
    public static Font DefaultFont = Backend.DefaultFont;

    internal TextBox() {
        _bounds = new Bounds();
        _font = DefaultFont;
        _textColor = DefaultColor;
        _spacing = new Vector2(.4f, 1);
        _textSize = 1;
        _text = string.Empty;
    }
    
    //todo: make text spacing a ratio of text size and screen proportions relative to the ideal ratio. and optimize it
    protected internal override void Draw() {
        List<float> lengths = [_spacing.x];
        float yBound = _spacing.y * _textSize;

        _spacing = _spacing with { x = _spacing.x / Backend.WindowWidthHeightRatio };
        
        int lineBreaks = 0;
        for(int i = 0; i < _text.Length; i++) {
            if (_text[i] != '\n') {
                lengths[lineBreaks] += _spacing.x * _textSize;
            } else {
                lineBreaks++;
                lengths.Add(_spacing.x);
                yBound += _spacing.y * _textSize;
            }
        }
        
        
        lineBreaks = 0;
        
        
        //todo: text wrap
        
        //todo: buffer _text a little bit? kind of on the edge
        
        Vector2 basePos = _bounds.pos 
                          + (_bounds.size * Orientations[(int)_textAnchor]) 
                          + (new Vector2(lengths[0], yBound) * BoundsOffsets[(int)_textAnchor]);
        
        Vector2 currentPos = basePos;

        int characters = 0;
        List<float> vertexData = new List<float>(_text.Length);

        //todo: SUBCLIPPING! text boxes reset clipping with this commented out line, make it so that it resets the clipping bounds to what portion is shared by the two bounds
        Backend.UploadSubclippingUniform(_bounds);
        
        for(int i = 0; i < _text.Length; i++) {
            
            if (_text[i] == '\n') {
                
                lineBreaks++;
    
                currentPos = basePos
                             + new Vector2(
                                 (lengths[lineBreaks] - lengths[0]) * BoundsOffsets[(int)_textAnchor].x,
                                 lineBreaks * _spacing.y * _textSize
                             );
                
                continue;
            }
            
            //todo: osmium.allchildren is a function not a property. same for scene

            Vector2 max = new Vector2(
                currentPos.x + _textSize / Backend.WindowWidthHeightRatio,
                currentPos.y + _textSize
            );
            
            Vector4 uv = _font[_text[i]];
            
            vertexData.AddRange(max.x, max.y, uv.z, uv.w,
                currentPos.x, max.y, uv.x, uv.w,
                max.x, currentPos.y, uv.z, uv.y,
                currentPos.x, currentPos.y, uv.x, uv.y);
            
            currentPos.x += _spacing.x * _textSize;
            characters++;
        }

        if (vertexData.Count > Backend.MaxElementsPerDraw) {
            Debug.Error("Text's length exceeds the maximum allowed characters! It may be rendered incorrectly; you can increase the limit in the config.");
        }
        
        //todo: this guy is suspicious!
        Backend.DrawElements(_font.texture, characters, _textColor, _depth, vertexData.ToArray());
        

        
        //todo: draw glyphs in bulk like so! bad though annoying to make
        
        //Radium.SetClippingBounds(new Bounds(max: new Vector2(100)));
    }
}