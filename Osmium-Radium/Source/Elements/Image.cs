using System.Drawing;
using System.Numerics;
using OsmiumNucleus;

namespace OsmiumRadium;



/// <summary> Represents an instance of a simple ASCII textbox! </summary>
public class Image : ImGUI
{

    public Texture texture;

    public Transform transform;
    
    
    /// <summary> Creates a new box element. Allows you to control many different options </summary>
    /// <param name="size"> size of the box characters</param>
    /// <param name="color"> Color of the box </param>
    public Image(Texture __texture, Transform transform = new Transform(), Color? color = null, int z = 0) {
        this.texture = __texture;
        this.color = color ?? DefaultColor;
        this.z = z;
        this.transform = transform;
    }


    
    /// <summary> size of the box</summary>
    public Vector2 size;

    
    
    /// <summary> The color of the text </summary>
    public Color color;
    /// <summary> The default color of new instances of Text, that do not set it implicitly. </summary>
    public static Color DefaultColor = Palette.White;
    
    
    
    protected internal override void Draw() {
        
        Backend.DrawElement(texture.Handle, color,
            transform.max.X , transform.max.Y, 1, 1,
            transform.min.X, transform.max.Y, 0, 1,
            transform.max.X, transform.min.Y, 1, 0,
            transform.min.X, transform.min.Y, 0, 0
        );
    }   
}