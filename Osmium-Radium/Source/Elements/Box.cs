

using OsmiumNucleus;


namespace OsmiumRadium;



/// <summary> Represents an instance of a simple box/rect! </summary>
public class Box : ImGUI
{
    

    /// <summary> Bounds of the box </summary>
    public Bounds Bounds;


    /// <summary> The _color of the box </summary>
    public Color color;
    /// <summary> The default _color of new instances of box that do not set it implicitly. </summary>
    public static Color DefaultColor = Palette.Primary;
    
    
    /// <summary> Creates a new box for that frame </summary>
    /// <param name="boundsorm"> Bounds of the box, it will be invisible if this is not set</param>
    /// <param name="color"> Color of the box </param>
    /// <param name="z"> Z of the box </param>
    public Box(Bounds bounds = new Bounds(), Color? color = null, int z = 0) {
        this.Bounds = bounds;
        this.color = color ?? DefaultColor;
        this.z = z;
    }
    
    
    
    protected internal override void Draw() {
        Backend.DrawElement(Backend.DefaultTexture, color,
            Bounds.max.X, Bounds.max.Y, 1, 1,
            Bounds.min.X, Bounds.max.Y, 0, 1,
            Bounds.max.X, Bounds.min.Y, 1, 0,
            Bounds.min.X, Bounds.min.Y, 0, 0
        );
    }
    
    
}