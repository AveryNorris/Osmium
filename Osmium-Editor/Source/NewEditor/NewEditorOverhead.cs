using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OsmiumEditor.Source.DearImGUINET.Structure;
using OsmiumNucleus;

namespace OsmiumEditor.Source.NewEditor;

public class NewEditorOverhead : EditorWindow
{
    protected internal override void Create() {
        Osmium.Window.ClientSize = Osmium.Window.CurrentMonitor.ClientArea.Size;
        Osmium.Window.ClientLocation = Vector2i.Zero;
        Osmium.Window.WindowState = WindowState.Maximized;
        
        Osmium.Window.WindowBorder = WindowBorder.Resizable;
        
        EditorWindowHierarchy.Add<NewInspector>();
        EditorWindowHierarchy.Add<NewComponentHierarchy>();
        EditorWindowHierarchy.Add<NewSceneHierarchy>();
    }
}