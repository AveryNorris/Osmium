using System.Text;
using ImGuiNET;
using OsmiumEditor.Source.DearImGUINET.Structure;
using OsmiumNucleus;

namespace OsmiumEditor.Source.NewEditor;

public class NewInspector : EditorWindow
{

    private byte[] renameBuffer = new byte[512];

    private const int renameMax = 512;
    
    protected internal override void Draw() {
        ImGui.Begin("Inspector");
        
        ComponentDocker? selectedObject = (ComponentDocker?) ComponentHierarchy.SelectedComponent ?? SceneHierarchy.SelectedScene ?? null;

        if (selectedObject == null)
        {
            ImGui.End();
            return;
        }

        if (ImGui.InputText("##NameInput", renameBuffer, renameMax, ImGuiInputTextFlags.EnterReturnsTrue)) {
            int count = 0;
            for (int i = 0; i < renameMax; i++)
            {
                if (renameBuffer[i] != 0)
                {
                    count++;
                }
                else break;
            }
            
            DockerMap.SetVariable(selectedObject, "Name", Encoding.UTF8.GetString(renameBuffer, 0, count));
        }

        renameBuffer = new byte[512];
        if (selectedObject is Scene scene)
        {
            Encoding.UTF8.GetBytes(scene.Name).CopyTo(renameBuffer);
        }else if (selectedObject is Component component)
        {
            Encoding.UTF8.GetBytes(component.Name).CopyTo(renameBuffer);
        }
        
        ImGui.End();
    }
}