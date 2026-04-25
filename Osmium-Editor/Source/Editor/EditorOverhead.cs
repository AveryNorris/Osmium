using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OsmiumNucleus;
using OsmiumRadium;
using RadiumTest2;
using Vector2 = System.Numerics.Vector2;

namespace OsmiumEditor;

public class EditorOverhead : RadiumElement
{

    public EditorOverhead() {
        Osmium.Context.ClientSize = Osmium.Context.CurrentMonitor.ClientArea.Size;
        Osmium.Context.ClientLocation = Vector2i.Zero;
        Osmium.Context.WindowState = WindowState.Maximized;
        
        Osmium.Context.WindowBorder = WindowBorder.Resizable;

        Radium.Add<Inspector>();
        Radium.Add<ComponentHierarchy>();
        Radium.Add<PlayMenu>();
        Radium.Add<DebugConsole>();
        Radium.Add<SceneHierarchy>();
    }
    
    protected override void Draw() {
        ConfigureWindow();
        
        //todo: load editor config
    }
    
    public void ConfigureWindow() {
        
    }
}