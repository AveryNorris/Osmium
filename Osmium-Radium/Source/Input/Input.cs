
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OsmiumNucleus;

namespace OsmiumRadium;

public static partial class Input
{

    //todo: verify openTK usage is all the same!
    
    static Input() {
        Osmium.Context.MouseMove += OnMouseMove;
        Osmium.Context.TextInput += OnTextInput;
        Osmium.Context.MouseWheel += OnMouseScroll;
        Osmium.Context.RenderFrame += args => Draw(); 
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

    public static bool GetKeyDown(Keys __key) => Osmium.Context.KeyboardState.IsKeyPressed(__key);
    
    public static bool GetKeyUp(Keys __key) => Osmium.Context.KeyboardState.IsKeyReleased(__key);
    
    public static bool GetKeyHeld(Keys __key) => Osmium.Context.KeyboardState.IsKeyDown(__key);
    
    
    public static bool MouseDown(MouseButton button) => 
        Osmium.Context.MouseState.IsButtonPressed(button);
    
    public static bool MouseUp(MouseButton button) => 
        Osmium.Context.MouseState.IsButtonReleased(button);

    public static bool MouseHeld(MouseButton button) => 
        Osmium.Context.MouseState.IsButtonDown(button);

    
    
    /// <summary> Saves the mouse position as a percentage of the window </summary>
    private static void OnMouseMove(MouseMoveEventArgs __mouseMoveEventArgs) {
        MousePos = new Vector2(100 * __mouseMoveEventArgs.X / Osmium.Context.ClientSize.X, 100 * __mouseMoveEventArgs.Y / Osmium.Context.ClientSize.Y);
        MouseDelta = new Vector2(100 * __mouseMoveEventArgs.DeltaX / Osmium.Context.ClientSize.X, 100 * __mouseMoveEventArgs.DeltaY / Osmium.Context.ClientSize.Y);
    }

    private static void OnMouseScroll(MouseWheelEventArgs __mouseWheelEventArgs) {
        Scroll = new Vector2(__mouseWheelEventArgs.OffsetX, __mouseWheelEventArgs.OffsetY);
    }
    
}