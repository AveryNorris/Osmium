namespace OsmiumRadium;



/// <summary> Represents an instance of a simple box/rect! </summary>
public class Box : ImGUI
{
    

    /// <summary> Transform of the box </summary>
    public Transform transform;


    /// <summary> The color of the box </summary>
    public Color color;
    /// <summary> The default color of new instances of box that do not set it implicitly. </summary>
    public static Color DefaultColor = Palette.Primary;
    
    
    /// <summary> Creates a new box for that frame </summary>
    /// <param name="transform"> Transform of the box, it will be invisible if this is not set</param>
    /// <param name="color"> Color of the box </param>
    /// <param name="z"> Z of the box </param>
    public Box(Transform transform = new Transform(), Color? color = null, int z = 0) {
        this.transform = transform;
        this.color = color ?? DefaultColor;
        this.z = z;
    }
    
    
    
    protected internal override void Draw() { 
        Backend.DrawElement(Backend.DefaultTexture, color,
            transform.max.X, transform.max.Y, 1, 1,
            transform.min.X, transform.max.Y, 0, 1,
            transform.max.X, transform.min.Y, 1, 0,
            transform.min.X, transform.min.Y, 0, 0
        );
    }
    
    
}