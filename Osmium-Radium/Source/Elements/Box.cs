using System.Drawing;
using System.Numerics;
using OsmiumNucleus;

namespace OsmiumRadium;



/// <summary> Represents an instance of a simple box/rect! </summary>
public class Box : GUIElement
{

    
    
    /// <summary> Creates a new box element that lasts for the duration of the frame </summary>
    /// <param name="transform"> Transform of the box's rect</param>
    /// <param name="color"> Color of the box</param>
    /// <param name="z"> Draw order of the box</param>
    public Box(Transform? transform = null, Palette? color = null, int z = 0) {
        this.transform = transform ?? new Transform();
        this.color = color ?? DefaultColor;
        this.z = z;
    }



    /// <summary> Transform of the box </summary>
    public Transform transform;


    /// <summary> The color of the text </summary>
    public Palette color;
    /// <summary> The default color of new instances of Text, that do not set it implicitly. </summary>
    public static Palette DefaultColor = Palette.Unknown;
    
    
    
    protected internal override void Draw() { 
        Backend.DrawElement(Backend.DefaultTexture, Radium.ColorPalette[(int) color],
            transform.max.X, transform.max.Y, 1, 1,
            transform.min.X, transform.max.Y, 0, 1,
            transform.max.X, transform.min.Y, 1, 0,
            transform.min.X, transform.min.Y, 0, 0
        );
    }
    
    
}