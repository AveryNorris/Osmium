
using System.Numerics;
using OpenTK.Platform;
using OsmiumNucleus;
using OsmiumRadium


namespace RadiumTest2;

public abstract class RadiumElement
{

    protected internal virtual void Update() {}
    
    protected internal virtual void Draw() {}

    protected internal const float r169 = 16f / 9f;
    
    
    
    
    /// <summary> Returns the mouse position in Radium coordinates </summary>
    public static Vector2 MousePos => new Vector2(100 * Osmium.Context.MousePosition.X / Osmium.Context.ClientSize.X, 100 * Osmium.Context.MousePosition.Y / Osmium.Context.ClientSize.Y);
    

    protected internal void GetMouseDown(Transform transform = new Transform(), MouseButton button = MouseButton.Button1) {
        
    }
    
    protected internal void GetMouseUp(Transform transform = new Transform(), MouseButton button = MouseButton.Button1) {
        
    }

    protected internal void GetMouseHeld(Transform transform = new Transform(), MouseButton button = MouseButton.Button1) {
        
    }
    
    protected internal void MouseInBounds(Transform transform) => MousePos.X >= transform.min.X && MousePos.Y >= transform.min.Y && MousePos.X <= transform.max.X && MousePos.Y <= transform.

    
    
    protected internal void Box(Transform transform = new Transform(), Color? color = null, Texture? texture = null) {
        Backend.DrawElement(Backend.DefaultTexture, color ?? Palette.Secondary,
            transform.max.X, transform.max.Y, 1, 1,
            transform.min.X, transform.max.Y, 0, 1,
            transform.max.X, transform.min.Y, 1, 0,
            transform.min.X, transform.min.Y, 0, 0
        );
    }
}