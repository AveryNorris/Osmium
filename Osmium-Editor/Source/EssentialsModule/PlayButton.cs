using ImGuiNET;
using OsmiumEditor.Source.DearImGUINET.Structure;
using OsmiumNucleus;

namespace OsmiumEditor.Source.EssentialsModule;

public class PlayButton : EditorWindow
{
    protected internal override void Draw() {
        ImGui.Begin("Menu");

        if (!Osmium.IsRunning)
        {
            if (ImGui.Button("Run"))
            {
               Osmium.VirtualRun();
            }
        }
        else
        {
            if (ImGui.Button("Close"))
            {
                Osmium.VirtualClose();
                //todo: REINITIALIZE??
            }
        }
        
        
        ImGui.End();
    }
}