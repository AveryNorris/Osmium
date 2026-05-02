
using System.Numerics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OsmiumNucleus;
using OsmiumRadium;


namespace OsmiumRadium;

public abstract partial class RadiumElement
{

    protected internal virtual void Update() {}
    
    protected internal virtual void Draw() {}

    protected internal const float r169 = 16f / 9f;
    
    
    
    
    /// <summary> Returns the mouse position in Radium coordinates </summary>
    public static Vector2 MousePos => new Vector2(100 * Osmium.Context.MousePosition.X / Osmium.Context.ClientSize.X, 100 * Osmium.Context.MousePosition.Y / Osmium.Context.ClientSize.Y);
    

    protected static bool MouseDown(Bounds bounds, MouseButton button) => 
            MouseInBounds(bounds) && Osmium.Context.MouseState.IsButtonPressed(button);
    
    protected static bool MouseUp(Bounds bounds, MouseButton button) => 
            MouseInBounds(bounds) && Osmium.Context.MouseState.IsButtonReleased(button);

    protected static bool MouseHeld(Bounds bounds, MouseButton button) => 
            MouseInBounds(bounds) && Osmium.Context.MouseState.IsButtonDown(button);

    protected static bool MouseInBounds(Bounds bounds) => 
            MousePos.X >= bounds.min.X && MousePos.Y >= bounds.min.Y && MousePos.X <= bounds.max.X && MousePos.Y <= bounds.max.Y;
    
    //todo: rename _bounds
    
    public static void SetClippingBounds(Bounds __pos) {
        //todo: add _bounds casting and implicit converts to vectors
        Backend.SetClipping(new Vector4(__pos.min.X, __pos.min.Y, __pos.max.X, __pos.max.Y));
    }
    
    
    //todo: fix osmium nucleus naming convetions
    protected Bounds Box(Bounds bounds, Color? color = null, Texture? texture = null) {
        Backend.DrawElement(texture ?? Backend.DefaultTexture, color ?? Palette.Secondary,
            bounds.max.X, bounds.max.Y, 1, 1,
            bounds.min.X, bounds.max.Y, 0, 1,
            bounds.max.X, bounds.min.Y, 1, 0,
            bounds.min.X, bounds.min.Y, 0, 0
        );
        
        return bounds;
    }

    protected BoxData Box() {
        //todo: change to official palette system rework
        BoxData returnValue = new BoxData();
        returnValue.Introduce();
        return returnValue;
    }

    //rename variables lol
    //todo: optimize parameter order, maybe make overloads
    protected Bounds Button(string text, Bounds bounds, float? textSize = null, Font? font = null, Color? textColor = null, Vector2? spacing = null, Color? backgroundColor = null, Color? hoverColor = null, Color? heldColor = null, MouseButton interactButton = MouseButton.Left, Anchor anchor = Anchor.Center) {
        Color boxColor = backgroundColor ?? OsmiumRadium.Button.DefaultBackgroundColor;
        if (MouseHeld(bounds, MouseButton.Left)) {
            boxColor = heldColor?? OsmiumRadium.Button.DefaultBackgroundHeldColor;
        }else if (MouseInBounds(bounds)) {
            boxColor = hoverColor?? OsmiumRadium.Button.DefaultBackgroundHoverColor;
        }
        
        Box(bounds, boxColor);
        
        Text(text, bounds, textSize, font, textColor, spacing, anchor);
        
        return bounds;
    }

    //todo: fix syntax?
    protected void Image(Texture texture, Bounds bounds, Color? color = null) => Box(bounds, color, texture);
}