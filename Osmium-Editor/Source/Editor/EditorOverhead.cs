using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OsmiumEditor.Source;
using OsmiumNucleus;
using OsmiumRadium;
using Vector2 = System.Numerics.Vector2;

namespace OsmiumEditor;

public class EditorOverhead : RetainedElement
{

    public EditorOverhead() {
        Osmium.Context.ClientSize = Osmium.Context.CurrentMonitor.ClientArea.Size;
        Osmium.Context.ClientLocation = Vector2i.Zero;
        Osmium.Context.WindowState = WindowState.Maximized;
        
        Osmium.Context.WindowBorder = WindowBorder.Resizable;

        Backend.Add<Inspector>();
        //Radium.Add<ComponentHierarchy>();
        //Radium.Add<PlayMenu>();
        //Radium.Add<DebugConsole>();
        //Radium.Add<SceneHierarchy>();
    }
    
    protected override void Draw() {
        ConfigureWindow();
        
        //Console.WriteLine("Components " + string.Join(',', AssemblyWindow.GetComponents()));
        
        //todo: load editor config
    }
    
    public void ConfigureWindow() {
        
    }
}