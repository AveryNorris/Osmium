using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OsmiumNucleus;
using OsmiumRadium;
using Vector2 = System.Numerics.Vector2;

namespace OsmiumEditor;

public class EditorOverhead : RetainedElement
{

    public EditorOverhead() {
        Osmium.Window.ClientSize = Osmium.Window.CurrentMonitor.ClientArea.Size;
        Osmium.Window.ClientLocation = Vector2i.Zero;
        Osmium.Window.WindowState = WindowState.Maximized;
        
        Osmium.Window.WindowBorder = WindowBorder.Resizable;
        
        Debug.Log("RETAINED ELEMENTS : " + string.Join(',', Backend.RetainedElements));

        //todo: make sure that windows are all static and support multiple of them!, and make Osmium Multithreaded
        Backend.Add<Inspector>().Rect = Rect.FromPosSize(85, 0, 15, 100);
        Backend.Add<ComponentHierarchy>().Rect = Rect.FromPosSize(65, 0, 20, 100 - 36.5625f);
        Backend.Add<SceneHierarchy>().Rect = Rect.FromPosSize(65,100 - 36.5625f, size: new OsmiumRadium.Vector2(20, 36.5625f));
        Backend.Add<DebugConsole>().Rect = Rect.FromPosSize(0,100 - 36.5625f, 65,36.5625f);
        
        //renderer should be 65 x 36.5625

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