using System.Numerics;
using OpenTK.Platform;
using OsmiumNucleus;
using MouseButton = OpenTK.Windowing.GraphicsLibraryFramework.MouseButton;
using TextInputEventArgs = OpenTK.Windowing.Common.TextInputEventArgs;


namespace OsmiumRadium;

public static partial class Backend
{
    //coordinates are based in 0-100 percentages of X and Y

    public static float WindowWidth => Osmium.Context.Size.X;
    
    public static float WindowHeight => Osmium.Context.Size.Y;
    
    public static float WindowWidthHeightRatio => WindowWidth / WindowHeight;
    
    //todo: use vectors and add scroll and keyboard
    public static float ScrollDeltaY => Osmium.Context.MouseState.ScrollDelta.Y;



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
    
    
    
    public static bool MouseInBounds(Vector2 __min, Vector2 __max) =>
            MousePos.X >= __min.X && MousePos.Y >= __min.Y && MousePos.X <= __max.X && MousePos.Y <= __max.Y;
    
    public static Vector2 MousePos => new Vector2(100 * Osmium.Context.MousePosition.X / Osmium.Context.ClientSize.X, 100 * Osmium.Context.MousePosition.Y / Osmium.Context.ClientSize.Y);
    


    public static string TextInput = "";



    public static void OnTextInput(TextInputEventArgs textInputEventArgs) {
        TextInput += textInputEventArgs.AsString;
    }
    
    //todo: component docker inherits list?
    
}