using System.Numerics;
using OsmiumNucleus;
using OsmiumRadium;


namespace OsmiumRadium;

public abstract partial class RadiumElement
{

    public static Vector2[] Orientations = [
        new Vector2(0,0),
        new Vector2(1, 0),
        new Vector2(0, 1),
        new Vector2(1, 1),
        new Vector2(0, .5f),
        new Vector2(1, .5f),
        new Vector2(.5f, 0),
        new Vector2(.5f, 1),
        new Vector2(.5f, .5f),
    ];
    
    //TopLeft,
    //TopRight,
    //BottomLeft,
    //BottomRight,
    //CenterLeft,
    //CenterRight,
    //TopCenter,
    //BottomCenter,
    //Center
    
    public static Vector2[] BoundsOffsets = [
        new Vector2(0, 0),
        new Vector2(-1, 0),
        new Vector2(0, -1),
        new Vector2(-1, -1),
        new Vector2(0, -.5f),
        new Vector2(-1, -.5f),
        new Vector2(-.5f, 0),
        new Vector2(-.5f, -1),
        new Vector2(-.5f, -.5f),
    ];

    public TextData Text() {
        TextData returnValue = new TextData();
        returnValue.Introduce();
        return returnValue;
    }

    public ButtonData Button() {
        ButtonData returnValue = new ButtonData();
        returnValue.Introduce();
        return returnValue;
    }
    
    /// <summary>
    /// Creates a _text box that fits within a given _bounds
    /// </summary>
    /// <param name="text"></param>
    /// <param name="boundsorm"></param>
    /// <param name="textSizeze"></param>
    /// <param name="font"></param>
    /// <param name="color"></param>
    /// <param name="spacing"></param>
    /// todo: fontsize not _text _size, and manage fixing _bounds even if error occurs
    protected void Text(string text, Bounds bounds, float? size = null, Font? font = null, Color? color = null, Vector2? spacing = null, Anchor anchor = Anchor.TopLeft) {
        
        float textSize = size ?? OsmiumRadium.Text.DefaultTextSize;
        
        //todo: rename? bad method
        Vector2 chosenSpacing = spacing ?? OsmiumRadium.Text.DefaultSpacingFactor;
        Font chosenFont = font ?? OsmiumRadium.Text.DefaultFont;
        Color chosenColor = color ?? OsmiumRadium.Text.DefaultColor;
        Vector2 currentPos = bounds.pos;
        
        List<float> lengths = [chosenSpacing.X];
        int lineBreaks = 0;
        Vector2 textBounds = new Vector2(0, chosenSpacing.Y * textSize);
        foreach (char c in text) {
            if (c != '\n') {
                lengths[lineBreaks] += chosenSpacing.X * textSize;
            } else {
                lineBreaks++;
                lengths.Add(chosenSpacing.X);
                textBounds.Y += chosenSpacing.Y * textSize;
            }
        }
        textBounds.X = lengths.Max();
        
        //todo: buffer _text a little bit? kind of on the edge
        currentPos = bounds.pos + (bounds.size * Orientations[(int) anchor]) + (textBounds * BoundsOffsets[(int) anchor]);

        SetClippingBounds(bounds);
        
        foreach(char c in text) {

            if (c == '\n') {
                currentPos.Y += chosenSpacing.Y * textSize;
                currentPos.X = bounds.pos.X - chosenSpacing.X * textSize;
                continue;
            }
            
            Vector4 screenPos = new Vector4(
                currentPos.X, 
                currentPos.Y, 
                currentPos.X + textSize / Backend.WindowWidthHeightRatio, 
                currentPos.Y + textSize
            );

            Vector4 uv = chosenFont[c];

            Backend.DrawElement(chosenFont.texture.Handle, chosenColor,
                screenPos.Z, screenPos.W, uv.Z, uv.W,
                screenPos.X, screenPos.W, uv.X, uv.W,
                screenPos.Z, screenPos.Y, uv.Z, uv.Y,
                screenPos.X, screenPos.Y, uv.X, uv.Y
            );
            
            currentPos.X += chosenSpacing.X * textSize;
        }
        
        SetClippingBounds(new Bounds(max: new Vector2(100)));
    }
    
    
}