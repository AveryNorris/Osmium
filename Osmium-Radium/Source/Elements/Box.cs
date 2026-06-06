namespace OsmiumRadium;



/// <summary> Describes an immediate mode GUI box that is drawn for one frame. </summary>
public class Box : ImmediateElement, IBoundedElement, ITexturedElement, IColoredElement
{
    
    
    
    /// <inheritdoc cref="IBoundedElement.Rect"/>
    public Rect Rect { get; set; }
    
    /// <inheritdoc cref="ITexturedElement{TSelf}._texture"/>
    public Texture _texture { get; set; }
    
    /// <inheritdoc cref="IColoredElement{TSelf}._color"/>
    public Color _color { get; set; }
    

    internal Box() {
        Rect = new Rect();
        _texture = Backend.DefaultTexture;
        _color = Palette.Secondary;
    }
    
    //todo: change IboundedElement to Iboundedobject? remove element requirmeent?


    protected internal override void Draw() {
        Backend.DrawElement(_texture, _color, _depth,
            Rect.max.x, Rect.max.y, 1, 1,
            Rect.min.x, Rect.max.y, 0, 1,
            Rect.max.x, Rect.min.y, 1, 0,
            Rect.min.x, Rect.min.y, 0, 0
        );
    }
    
    
    
}