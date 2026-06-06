using System.ComponentModel;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;



namespace OsmiumNucleus;



public sealed class Window() : GameWindow(GameWindowSettings.Default, new NativeWindowSettings() {DepthBits = 24})
{
    
}