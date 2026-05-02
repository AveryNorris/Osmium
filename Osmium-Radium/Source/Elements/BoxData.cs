using OsmiumNucleus;
using OsmiumRadium.Source.Interfaces;


namespace OsmiumRadium;



/// <summary> Describes a box drawn for one frame </summary>
public class BoxData : Element, IBoundedElement<BoxData>, ITexturedElement<BoxData>, IColoredElement<BoxData>
{
    
    
    
    /// <inheritdoc cref="IBoundedElement{TSelf}._bounds"/>
    public Bounds _bounds { get; set; }
    
    /// <inheritdoc cref="ITexturedElement{TSelf}._texture"/>
    public Texture _texture { get; set; }
    
    /// <inheritdoc cref="IColoredElement{TSelf}._color"/>
    public Color _color { get; set; }
    
    
    
    /// <summary> Default _color of the box, if it is not implicitly set </summary>
    public Color DefaultColor => Color.Error;
    
    

    internal BoxData() {
        _bounds = new Bounds();
        _texture = Backend.BaseTexture;
        _color = DefaultColor;
    }
    
    protected internal override void Draw() {
        //todo: color to string;
        Backend.DrawElement(_texture, _color,
            _bounds.max.X, _bounds.max.Y, 1, 1,
            _bounds.min.X, _bounds.max.Y, 0, 1,
            _bounds.max.X, _bounds.min.Y, 1, 0,
            _bounds.min.X, _bounds.min.Y, 0, 0
        );
    }
}