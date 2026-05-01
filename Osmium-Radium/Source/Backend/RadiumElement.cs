
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
    

    protected static bool GetMouseDown(Transform transform, MouseButton button) => 
            MouseInBounds(transform) && Osmium.Context.MouseState.IsButtonPressed(button);
    
    protected static bool GetMouseUp(Transform transform, MouseButton button) => 
            MouseInBounds(transform) && Osmium.Context.MouseState.IsButtonReleased(button);

    protected static bool GetMouseHeld(Transform transform, MouseButton button) => 
            MouseInBounds(transform) && Osmium.Context.MouseState.IsButtonDown(button);

    protected static bool MouseInBounds(Transform transform) => 
            MousePos.X >= transform.min.X && MousePos.Y >= transform.min.Y && MousePos.X <= transform.max.X && MousePos.Y <= transform.max.Y;

    protected void Button(Transform transform, Text text) {
        
    }
    
    public static void SetClippingBounds(Transform __pos) {
        Backend.ClippingRects.Add((Backend.elementCount, new Vector4(__pos.min.X, __pos.min.Y, __pos.max.X, __pos.max.Y)));
    }
    
    
    //todo: fix osmium nucleus naming convetions
    protected void Box(Transform transform, Color? color = null, Texture? texture = null) {
        Backend.DrawElement(texture ?? Backend.DefaultTexture, color ?? Palette.Secondary,
            transform.max.X, transform.max.Y, 1, 1,
            transform.min.X, transform.max.Y, 0, 1,
            transform.max.X, transform.min.Y, 1, 0,
            transform.min.X, transform.min.Y, 0, 0
        );
    }
}