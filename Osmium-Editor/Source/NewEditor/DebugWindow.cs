using System.Numerics;
using ImGuiNET;
using OsmiumEditor.Source.DearImGUINET.Structure;
using OsmiumNucleus;
namespace OsmiumEditor.Source.NewEditor;

public class DebugWindow : EditorWindow
{
    
    int selectedIndex = 0;
    
    protected internal override void Draw() {
        ImGui.Begin("Hello World");
        
        List<string> debugOrder = [];
        foreach (var t in Debug.Stack) {
            debugOrder.Add(t.Key.CallSign + " : " + t.Key.Message + " : " + t.Value);
        }
        
        ImGui.ListBox("listBox", ref selectedIndex, debugOrder.ToArray(), debugOrder.Count);
        
        ImGui.End();
    }
}