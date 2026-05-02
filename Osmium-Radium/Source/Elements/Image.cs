using System.Drawing;
using System.Numerics;
using OsmiumNucleus;

namespace OsmiumRadium;



/// <summary> Represents an instance of a simple ASCII textbox! </summary>
public class Image : ImGUI
{

    public Texture texture;

    public Bounds Bounds;
    
    
    /// <summary> Creates a new box element. Allows you to control many different options </summary>
    /// <param name="size"> _size of the box characters</param>
    /// <param name="color"> Color of the box </param>
    public Image(Texture __texture, Bounds bounds = new Bounds(), Color? color = null, int z = 0) {
        this.texture = __texture;
        this.color = color ?? DefaultColor;
        this.z = z;
        this.Bounds = bounds;
    }


    
    /// <summary> _size of the box</summary>
    public Vector2 size;

    
    
    /// <summary> The _color of the _text </summary>
    public Color color;
    /// <summary> The default _color of new instances of Text, that do not set it implicitly. </summary>
    public static Color DefaultColor = Palette.White;
    
    
    
    protected internal override void Draw() {
        
        Backend.DrawElement(texture.Handle, color,
            Bounds.max.X , Bounds.max.Y, 1, 1,
            Bounds.min.X, Bounds.max.Y, 0, 1,
            Bounds.max.X, Bounds.min.Y, 1, 0,
            Bounds.min.X, Bounds.min.Y, 0, 0
        );
    }   
}