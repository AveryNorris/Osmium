using System.Numerics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OsmiumNucleus;

namespace OsmiumRadium;

public static partial class Backend
{
    //coordinates are based in 0-100 percentages of X and Y

    public static float WindowWidth => Osmium.Context.Size.X;
    
    public static float WindowHeight => Osmium.Context.Size.Y;

    //returns 0-100 for the mouse position as a percentage of the screen 0 is left/top and 100 is bottom/right
    public static float MouseX => 100 * Osmium.Context.MousePosition.X / Osmium.Context.ClientSize.X;
    
    public static float MouseY => 100 * Osmium.Context.MousePosition.Y / Osmium.Context.ClientSize.Y;
    
    public static float WindowWidthHeightRatio => WindowWidth / WindowHeight;



    /// <summary>
    /// Tests for the mouse being held down in a certain portion of the screen
    /// </summary>
    /// <param name="__minX">Minimum X of the bounds</param>
    /// <param name="__maxX">Maximum X of the bounds</param>
    /// <param name="__minY">Minimum Y of the bounds</param>
    /// <param name="__maxY">Maximum Y of the bounds</param>
    /// <param name="__buttonType"> Type of mouse button to test for</param>
    public static bool MouseHeld(Vector2 __min, Vector2 __max, MouseButton __buttonType)
    {
        if (!Osmium.Context.MouseState.IsButtonDown(__buttonType)) return false;

        return MouseInBounds(__min, __max);
    }
    
    /// <summary>
    /// Tests for the mouse being held down in a certain portion of the screen
    /// </summary>
    /// <param name="__minX">Minimum X of the bounds</param>
    /// <param name="__maxX">Maximum X of the bounds</param>
    /// <param name="__minY">Minimum Y of the bounds</param>
    /// <param name="__maxY">Maximum Y of the bounds</param>
    /// <param name="__buttonType"> Type of mouse button to test for</param>
    public static bool MouseHeld(MouseButton __buttonType)
    {
        return Osmium.Context.MouseState.IsButtonDown(__buttonType);
    }

    /// <summary>
    /// Tests for the mouse being released in a certain portion of the screen
    /// </summary>
    /// <param name="__minX">Minimum X of the bounds</param>
    /// <param name="__maxX">Maximum X of the bounds</param>
    /// <param name="__minY">Minimum Y of the bounds</param>
    /// <param name="__maxY">Maximum Y of the bounds</param>
    /// <param name="__buttonType"></param>
    public static bool MouseUp(Vector2 __min, Vector2 __max, MouseButton __buttonType)
    {
        if (!Osmium.Context.MouseState.IsButtonReleased(__buttonType)) return false;
        
        return MouseInBounds(__min, __max);
    }
    
    /// <summary>
    /// Tests for the mouse being held down in a certain portion of the screen
    /// </summary>
    /// <param name="__minX">Minimum X of the bounds</param>
    /// <param name="__maxX">Maximum X of the bounds</param>
    /// <param name="__minY">Minimum Y of the bounds</param>
    /// <param name="__maxY">Maximum Y of the bounds</param>
    /// <param name="__buttonType"> Type of mouse button to test for</param>
    public static bool MouseUp(MouseButton __buttonType)
    {
        return Osmium.Context.MouseState.IsButtonReleased(__buttonType);
    }
    
    /// <summary>
    /// Tests for the mouse being pressed in a certain portion of the screen
    /// </summary>
    /// <param name="__minX">Minimum X of the bounds</param>
    /// <param name="__maxX">Maximum X of the bounds</param>
    /// <param name="__minY">Minimum Y of the bounds</param>
    /// <param name="__maxY">Maximum Y of the bounds</param>
    /// <param name="__buttonType"></param>
    public static bool MouseDown(Vector2 __min, Vector2 __max, MouseButton __buttonType)
    {
        if (!Osmium.Context.MouseState.IsButtonPressed(__buttonType)) return false;
        
        return MouseInBounds(__min, __max);
    }
    
    /// <summary>
    /// Tests for the mouse being held down in a certain portion of the screen
    /// </summary>
    /// <param name="__minX">Minimum X of the bounds</param>
    /// <param name="__maxX">Maximum X of the bounds</param>
    /// <param name="__minY">Minimum Y of the bounds</param>
    /// <param name="__maxY">Maximum Y of the bounds</param>
    /// <param name="__buttonType"> Type of mouse button to test for</param>
    public static bool MouseDown(MouseButton __buttonType)
    {
        return Osmium.Context.MouseState.IsButtonPressed(__buttonType);
    }
    
    
    
    public static bool MouseInBounds(Vector2 __min, Vector2 __max) {
        return MouseX >= __min.X && MouseY >= __min.Y && MouseX <= __max.X && MouseY <= __max.Y;
    }
}