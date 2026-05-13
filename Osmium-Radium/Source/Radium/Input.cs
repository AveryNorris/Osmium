using System.Numerics;
using OpenTK.Platform;
using OsmiumNucleus;
using MouseButton = OpenTK.Windowing.GraphicsLibraryFramework.MouseButton;
using TextInputEventArgs = OpenTK.Windowing.Common.TextInputEventArgs;


namespace OsmiumRadium;

public static partial class Radium
{
    //coordinates are based in 0-100 percentages of X and Y
    
    
    //todo: use vectors and add scroll and keyboard
    public static float VerticalScroll => Osmium.Context.MouseState.ScrollDelta.Y;
    public static float HorizontalScroll => Osmium.Context.MouseState.ScrollDelta.X;

    public static Vector2 MousePos { get; internal set; }

    public static string TextInput = "";



    private static void OnTextInput(TextInputEventArgs textInputEventArgs) {
        TextInput += textInputEventArgs.AsString;
    }
    
}