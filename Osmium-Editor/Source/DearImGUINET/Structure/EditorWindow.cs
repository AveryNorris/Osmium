using System.Numerics;
using Dear_ImGui_Sample.Backends;
using ImGuiNET;

namespace OsmiumEditor.Source.DearImGUINET.Structure;

public abstract class EditorWindow
{
    
    
    protected static Vector2 ScreenSize => BedrockImGUICompatability.ScreenSize;

    protected static ImGuiIOPtr IO => ImGui.GetIO();

    protected internal virtual void Create() {}
    
    protected internal virtual void Destroy() {}
    
    
    
    protected internal virtual void Update() {}
    
    protected internal virtual void Draw() {}

    protected internal virtual void Unload() {}
}