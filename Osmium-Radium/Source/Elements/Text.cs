using System.Drawing;
using System.Numerics;
using OsmiumNucleus;

namespace OsmiumRadium;



/// <summary> Represents an instance of a simple ASCII textbox! </summary>
public class Text : ImGUI
{

    
    
    /// <summary> Creates a new text element. Allows you to control many different options </summary>
    /// <param name="text"> Text to be written</param>
    /// <param name="size"> size of the text characters</param>
    /// <param name="pos"> Position of the text</param>
    /// <param name="center"> center of the text, mutually exclusive to pos </param>
    /// <param name="spacing"> Spacing between the chars, relative to font size (Not in absolute coordinates) (realWorldSpacing = fontSize * Spacing)</param>
    /// <param name="color"> Color of the text</param>
    /// <param name="font"> Font of the text</param>
    /// <param name="z"> Font of the text</param>
    public Text(string? text = null, float? size = null, Vector2? pos = null, Vector2? center = null, Vector2? spacing = null, Color? color = null, Font? font = null, int z = 0) {
        this.text = text ?? string.Empty;
        this.size = size ?? DefaultTextSize;
        this.pos = pos ?? Vector2.Zero;
        this.spacing = spacing ?? DefaultSpacingFactor;
        this.color = color ?? DefaultColor;

        if (font != null) {
            this.font = font;
        }else if (DefaultFont == null) {
            Debug.Error("A default font does not exist! Either set one implicit or configure a default font.");
        }else
            this.font = DefaultFont;

        this.z = z;

        if (center != null) {
            this.center = (Vector2)center;

            if (pos != null)
                Debug.Error("A given Text element has definitions of both center and pos! Which causes one of them to be overridden.");
        }
    }

    
    
    /// <inheritdoc cref="Transform.pos"/>
    public Vector2 pos;
    /// <inheritdoc cref="Transform.center"/>
    public Vector2 center {
        get => pos + bounds / 2;
        set => pos = value - bounds / 2;
    }
    

    /// <summary> The text the element will render </summary>
    public string text;
    
    
    /// <summary> The element's text size </summary>
    public float size;
    /// <summary> The default text size for new instances of text that do not set it implicitly </summary>
    public static float DefaultTextSize = 3;

    
    /// <summary> The color of the text </summary>
    public Color color;
    /// <summary> The default color of new instances of Text, that do not set it implicitly. </summary>
    public static Color DefaultColor = Palette.White;
    
    
    /// <summary> The spacing between consecutive characters in the text </summary>
    public Vector2 spacing;
    /// <summary> The default text spacing for new instances of text that do not set it implicitly;
    /// represented as a relative factor of text size for relative scaling </summary>
    public static Vector2 DefaultSpacingFactor = new Vector2(.2f, 1);
    
    
    /// <summary> Position of the text, from the top left corner</summary>
    public Font font;
    /// <summary> The default position of new instances of text that do not set it implicitly. </summary>
    public static Font DefaultFont;
    
    
    /// <summary> The predicted size of the text element in its respective dimensions. </summary>
    public Vector2 bounds {
        get {
            float Y = spacing.Y * size;
            List<float> xSpacing = [0];
            foreach (char c in text) {
                if (c == '\n') {
                    Y += spacing.Y * size;
                    xSpacing.Add(0);
                    continue;
                }
            
                if ((int)c is < 32 or > 126) {
                    return Vector2.Zero;
                }

                xSpacing[^1] += spacing.X * size;
            }
        
            return new Vector2(xSpacing.Max(), Y);
        }
    }
    
    
    
    protected internal override void Draw() {
        Vector2 currentPos = pos;
        currentPos.X -= spacing.X * size;
        
        foreach(char c in text) {

            if (c == '\n') {
                currentPos.Y += spacing.Y * size;
                currentPos.X = pos.X - spacing.X * size;
                continue;
            }
            
            Vector4 screenPos = new Vector4(
                currentPos.X, 
                currentPos.Y, 
                currentPos.X + size / Backend.WindowWidthHeightRatio, 
                currentPos.Y + size
            );

            Vector4 uv = font[c];

            Backend.DrawElement(font.texture.Handle, color,
                screenPos.Z, screenPos.W, uv.Z, uv.W,
                screenPos.X, screenPos.W, uv.X, uv.W,
                screenPos.Z, screenPos.Y, uv.Z, uv.Y,
                screenPos.X, screenPos.Y, uv.X, uv.Y
            );
            
            currentPos.X += spacing.X * size;
        }
    }
}