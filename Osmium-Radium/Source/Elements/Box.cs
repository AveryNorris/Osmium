namespace OsmiumRadium;



/// <summary> Describes an immediate mode GUI box that is drawn for one frame. </summary>
public class Box : Element, IBoundedElement<Box>, ITexturedElement<Box>, IColoredElement<Box>
{
    
    
    
    /// <inheritdoc cref="IBoundedElement{TSelf}._bounds"/>
    public Bounds _bounds { get; set; }
    
    /// <inheritdoc cref="ITexturedElement{TSelf}._texture"/>
    public Texture _texture { get; set; }
    
    /// <inheritdoc cref="IColoredElement{TSelf}._color"/>
    public Color _color { get; set; }
    
    

    internal Box() {
        _bounds = new Bounds();
        _texture = Backend.BaseTexture;
        _color = Palette.Secondary;
    }
    
    
    
    protected internal override void Draw() {
        Backend.DrawElement(_texture, _color,
            _bounds.max.X, _bounds.max.Y, 1, 1,
            _bounds.min.X, _bounds.max.Y, 0, 1,
            _bounds.max.X, _bounds.min.Y, 1, 0,
            _bounds.min.X, _bounds.min.Y, 0, 0
        );
    }
    
    
    
}