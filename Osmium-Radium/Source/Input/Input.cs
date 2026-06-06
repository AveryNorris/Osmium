
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OsmiumNucleus;

namespace OsmiumRadium;

public static partial class Input
{

    //todo: verify openTK usage is all the same!
    
    static Input() {
        Osmium.Window.MouseMove += OnMouseMove;
        Osmium.Window.TextInput += OnTextInput;
        Osmium.Window.MouseWheel += OnMouseScroll;
        Osmium.Window.RenderFrame += args => Draw(); 
    }
    
    public static Vector2 Scroll;
    
    public static Vector2 MousePos { get; internal set; }
    
    public static Vector2 MouseDelta { get; internal set; }

    public static string TextInput = "";
    

    private static void Draw() {
        Scroll = new Vector2(0);
        MouseDelta = new Vector2(0);
        TextInput = "";
    }



    //todo: save open windows
    private static void OnTextInput(TextInputEventArgs __textInputEventArgs) => TextInput += __textInputEventArgs.AsString;

    public static bool GetKeyDown(Keys __key) => Osmium.Window.KeyboardState.IsKeyPressed(__key);
    
    public static bool GetKeyUp(Keys __key) => Osmium.Window.KeyboardState.IsKeyReleased(__key);
    
    public static bool GetKeyHeld(Keys __key) => Osmium.Window.KeyboardState.IsKeyDown(__key);
    
    
    public static bool MouseDown(MouseButton button) => 
        Osmium.Window.MouseState.IsButtonPressed(button);
    
    public static bool MouseUp(MouseButton button) => 
        Osmium.Window.MouseState.IsButtonReleased(button);

    public static bool MouseHeld(MouseButton button) => 
        Osmium.Window.MouseState.IsButtonDown(button);

    
    
    /// <summary> Saves the mouse position as a percentage of the window </summary>
    private static void OnMouseMove(MouseMoveEventArgs __mouseMoveEventArgs) {
        MousePos = new Vector2(100 * __mouseMoveEventArgs.X / Osmium.Window.ClientSize.X, 100 * __mouseMoveEventArgs.Y / Osmium.Window.ClientSize.Y);
        MouseDelta = new Vector2(100 * __mouseMoveEventArgs.DeltaX / Osmium.Window.ClientSize.X, 100 * __mouseMoveEventArgs.DeltaY / Osmium.Window.ClientSize.Y);
    }

    private static void OnMouseScroll(MouseWheelEventArgs __mouseWheelEventArgs) {
        Scroll = new Vector2(__mouseWheelEventArgs.OffsetX, __mouseWheelEventArgs.OffsetY);
    }
    
}