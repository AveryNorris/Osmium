using System.Drawing;
using System.Numerics;
using OsmiumNucleus;

namespace OsmiumRadium;



/// <summary> Represents an instance of a simple ASCII textbox! </summary>
public class Image : GUIElement
{

    public Texture texture;
    
    
    /// <summary> Creates a new box element. Allows you to control many different options </summary>
    /// <param name="size"> size of the box characters</param>
    /// <param name="pos"> Position of the box</param>
    /// <param name="center"> center of the box, mutually exclusive to pos </param>
    /// <param name="color"> Color of the box </param>
    public Image(string path, Vector2? size = null, Vector2? pos = null, Vector2? center = null, Palette? color = null, int z = 0) {
        this.texture = new Texture(path);
        this.size = size ?? Vector2.One;
        this.pos = pos ?? Vector2.Zero;
        this.color = color ?? DefaultColor;
        this.z = z;

        if (center != null) {
            if (pos == null)
                this.center = (Vector2)center;
            else Debug.LogError("A given Text element has definitions of center and pos!");
        }
    }


    
    /// <summary> size of the box</summary>
    public Vector2 size;

    
    
    /// <summary> The color of the text </summary>
    public Palette color;
    /// <summary> The default color of new instances of Text, that do not set it implicitly. </summary>
    public static Palette DefaultColor = Palette.Unknown;
    
    
    
    /// <summary> Position of the text, from the top left corner</summary>
    public Vector2 pos;



    /// <summary> Smallest corner of the box </summary>
    public Vector2 min => pos;
    /// <summary> Largest corner of the box </summary>
    public Vector2 max => pos + size;

    
    
    /// <summary> The center position of the text element </summary>
    public Vector2 center {
        get => pos + size / 2;
        set => pos = value - size / 2;
    }
    
    
    
    protected internal override void Draw() {

        Vector4 screenPos = new Vector4(
            pos.X,
            pos.Y,
            pos.X + size.X,
            pos.Y + size.Y
        );
        
        Backend.DrawElement(texture.Handle, Radium.ColorPalette[(int) color],
            screenPos.Z, screenPos.W, 1, 1,
            screenPos.X, screenPos.W, 0, 1,
            screenPos.Z, screenPos.Y, 1, 0,
            screenPos.X, screenPos.Y, 0, 0
        );
        
    }
}