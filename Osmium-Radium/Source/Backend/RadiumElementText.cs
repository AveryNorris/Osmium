using System.Numerics;
using OsmiumRadium;


namespace OsmiumRadium;

public abstract partial class RadiumElement
{
    
    
    /// <summary>
    /// Creates a text box that fits within a given bounds
    /// </summary>
    /// <param name="text"></param>
    /// <param name="transform"></param>
    /// <param name="textSizeze"></param>
    /// <param name="font"></param>
    /// <param name="color"></param>
    /// <param name="spacing"></param>
    protected void Text(string text, Transform transform, float? textSize = null, Font? font = null, Color? color = null, Vector2? spacing = null) {
        Vector2 currentPos = transform.pos;
        float size = textSize ?? OsmiumRadium.Text.DefaultTextSize;
        
        //todo: rename? bad method
        Vector2 chosenSpacing = spacing ?? OsmiumRadium.Text.DefaultSpacingFactor;
        Font chosenFont = font ?? OsmiumRadium.Text.DefaultFont;
        Color chosenColor = color ?? OsmiumRadium.Text.DefaultColor;
        
        currentPos.X -= chosenSpacing.X * size;
        
        SetClippingBounds(transform);
        
        foreach(char c in text) {

            if (c == '\n') {
                currentPos.Y += chosenSpacing.Y * size;
                currentPos.X = transform.pos.X - chosenSpacing.X * size;
                continue;
            }
            
            Vector4 screenPos = new Vector4(
                currentPos.X, 
                currentPos.Y, 
                currentPos.X + size / Backend.WindowWidthHeightRatio, 
                currentPos.Y + size
            );

            Vector4 uv = chosenFont[c];

            Backend.DrawElement(chosenFont.texture.Handle, chosenColor,
                screenPos.Z, screenPos.W, uv.Z, uv.W,
                screenPos.X, screenPos.W, uv.X, uv.W,
                screenPos.Z, screenPos.Y, uv.Z, uv.Y,
                screenPos.X, screenPos.Y, uv.X, uv.Y
            );
            
            currentPos.X += chosenSpacing.X * size;
        }
    }
    
    
}