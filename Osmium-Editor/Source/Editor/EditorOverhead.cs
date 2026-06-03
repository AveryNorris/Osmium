using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
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
        
        Debug.Log("RETAINED ELEMENTS : " + string.Join(',', Backend.RetainedElements));

        //todo: make sure that windows are all static and support multiple of them!, and make Osmium Multithreaded
        Backend.Add<Inspector>().bounds = new Bounds(pos: new OsmiumRadium.Vector2(85,0), size: new OsmiumRadium.Vector2(15, 100));
        Backend.Add<ComponentHierarchy>().bounds = new Bounds(pos: new OsmiumRadium.Vector2(65,0), size: new OsmiumRadium.Vector2(20, 75));
        Backend.Add<SceneHierarchy>().bounds = new Bounds(pos: new OsmiumRadium.Vector2(65,75), size: new OsmiumRadium.Vector2(20, 25));

        //Backend.Add<CompileTesting>();
        
        //todo: change backend.add to just add through retained element and make backend radium
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