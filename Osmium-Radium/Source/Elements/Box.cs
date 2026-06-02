namespace OsmiumRadium;



/// <summary> Describes an immediate mode GUI box that is drawn for one frame. </summary>
public class Box : ImmediateElement, IBoundedElement<Box>, ITexturedElement<Box>, IColoredElement<Box>, IBoundedElement, ITexturedElement, IColoredElement, IDepthElement<Box>, IDepthElement
{
    
    
    
    /// <inheritdoc cref="IBoundedElement{TSelf}._bounds"/>
    public Bounds _bounds { get; set; }
    
    /// <inheritdoc cref="ITexturedElement{TSelf}._texture"/>
    public Texture _texture { get; set; }
    
    /// <inheritdoc cref="IColoredElement{TSelf}._color"/>
    public Color _color { get; set; }
    
    public float _depth { get; set; }
    
    

    internal Box() {
        _bounds = new Bounds();
        _texture = Backend.DefaultTexture;
        _color = Palette.Secondary;
    }
    
    //todo: change IboundedElement to Iboundedobject? remove element requirmeent?


    protected internal override void Draw() {
        Backend.DrawElement(_texture, _color, _depth,
            _bounds.max.x, _bounds.max.y, 1, 1,
            _bounds.min.x, _bounds.max.y, 0, 1,
            _bounds.max.x, _bounds.min.y, 1, 0,
            _bounds.min.x, _bounds.min.y, 0, 0
        );
    }
    
    
    
}