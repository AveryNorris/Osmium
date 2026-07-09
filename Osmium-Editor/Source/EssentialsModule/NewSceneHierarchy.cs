using System.Numerics;
using System.Text;
using ImGuiNET;
using OsmiumEditor.Source.DearImGUINET.Structure;
using OsmiumEditor.Source.NewEditor.Serialization;
using OsmiumNucleus;

namespace OsmiumEditor.Source.NewEditor;

public class SceneHierarchy : EditorWindow
{

    public static Scene? SelectedScene;

    public byte[] RenameBuffer = [];

    public const int NameLimit = 512;

    public bool Renaming;

    public bool SetFocus;

    public const float lineSize = 3;


    protected internal override void Draw() {
        
        Editor.Save();

        ImGui.Begin("Scenes");
        
        if (ImGui.IsMouseClicked(ImGuiMouseButton.Left) && ImGui.IsWindowHovered() && !ImGui.IsAnyItemActive()) {
            SelectedScene = null;
        }

        if (ImGui.BeginPopupContextWindow())
        {
            
            if(Renaming)
                ImGui.CloseCurrentPopup();

            if (ImGui.Button("Add"))
            {
                string name = GenerateDefaultName();
                OsmiumNucleus.Scene newScene = Osmium.AddScene(GenerateDefaultName())!;
                
                DockerMap.SetVariable(newScene, "Name", name);

                SelectedScene = newScene;
                
                StartRenaming(newScene);
                ImGui.CloseCurrentPopup();
            }

            ImGui.EndPopup();
        }

        int count = 0;
        foreach (Scene scene in DockerMap.Scenes.ToArray())
        {
            
            if (!(Renaming && SelectedScene == scene))
            {
                if (ImGui.Selectable(scene.Name, SelectedScene == scene))
                {
                    SelectedScene = scene;
                    ComponentHierarchy.SelectedComponent = null;
                }
            }
            else
            {
                if (SetFocus)
                {
                    ImGui.SetKeyboardFocusHere();
                    SetFocus = false;
                }

                if (ImGui.InputText("##SceneInput", RenameBuffer, 512, ImGuiInputTextFlags.EnterReturnsTrue) ||
                    ImGui.IsItemDeactivated()) FinishRenaming();
            }

            if (!Renaming)
            {
                if (ImGui.BeginDragDropSource())
                {
                    int sourceIndex = count;

                    unsafe
                    {
                        ImGui.SetDragDropPayload("SceneSwap", (IntPtr)(&sourceIndex), sizeof(int));
                    }

                    ImGui.EndDragDropSource();
                }

                if (ImGui.BeginDragDropTarget())
                {
                    
                    ImGuiPayloadPtr payload = ImGui.AcceptDragDropPayload("SceneSwap", ImGuiDragDropFlags.AcceptNoDrawDefaultRect);
                    
                    float mouseY = ImGui.GetMousePos().Y;

                    float minY = ImGui.GetItemRectMin().Y;
                    float maxY = ImGui.GetItemRectMax().Y;
                            
                    float t = (mouseY - minY) / (maxY - minY);
                    
                    ImDrawListPtr DrawList = ImGui.GetWindowDrawList();

                    Vector2 min = ImGui.GetItemRectMin();
                    Vector2 max = ImGui.GetItemRectMax();

                    if (t <= .5f)
                    {
                        DrawList.AddLine(min, new Vector2(max.X, min.Y), ImGui.GetColorU32(ImGuiCol.HeaderActive), lineSize);
                    }
                    else
                    {
                        DrawList.AddLine(new Vector2(min.X, max.Y), max, ImGui.GetColorU32(ImGuiCol.HeaderActive), lineSize);
                    }

                    unsafe
                    {
                        if (payload.NativePtr != null)
                        {

                            int targetIndex = count;
                            int grabbingIndex = *(int*)(payload.Data);
                            

                            SerializedScene grabbedSerializedScene = DockerMap.Scenes[grabbingIndex];
                            SerializedScene targetSerializedScene = DockerMap.Scenes[targetIndex];
                                
                            DockerMap.Scenes.RemoveAt(grabbingIndex);
                            
                            int newIndex = DockerMap.Scenes.IndexOf(targetSerializedScene);
                            
                            
                            
                            if (t <= .5f)
                            {
                                if (newIndex < 0)
                                    newIndex = 0;
                                if (newIndex > DockerMap.Scenes.Count)
                                    newIndex = DockerMap.Scenes.Count;
                                
                                DockerMap.Scenes.Insert(newIndex, grabbedSerializedScene);                                
                            }else if (t > .5f)
                            {
                                
                                newIndex++;

                                if (newIndex < 0)
                                    newIndex = 0;
                                if (newIndex > DockerMap.Scenes.Count)
                                    newIndex = DockerMap.Scenes.Count;
                                
                                DockerMap.Scenes.Insert(newIndex, grabbedSerializedScene);  
                            }

                        }
                    }
                    
                    ImGui.EndDragDropTarget();
                }
            }

            if (ImGui.BeginPopupContextItem())
            {
                
                if(Renaming)
                    ImGui.CloseCurrentPopup();

                if (ImGui.Button("Add")) {
                    StartRenaming(Osmium.AddScene(GenerateDefaultName())!);
                    ImGui.CloseCurrentPopup();
                }

                if (ImGui.Button("Rename")) {
                    StartRenaming(scene);
                    ImGui.CloseCurrentPopup();
                }

                if (ImGui.Button("Remove")) {
                    Osmium.RemoveScene(scene);
                    SelectedScene = null;
                    ImGui.CloseCurrentPopup();
                }
                
                ImGui.EndPopup();
            }

            count++;
        }

        ImGui.End();
    }

    public void StartRenaming(OsmiumNucleus.Scene scene) {
        SelectedScene = scene;
        Renaming = true;
        SetFocus = true;

        RenameBuffer = new byte[NameLimit];
        Encoding.UTF8.GetBytes(scene.Name).CopyTo(RenameBuffer);
    }

    public void FinishRenaming() {
        Renaming = false;

        int count = 0;
        for (int i = 0; i < NameLimit; i++)
        {
            if (RenameBuffer[i] != 0)
            {
                count++;
            }
            else break;
        }

        string FinalName = Encoding.UTF8.GetString(RenameBuffer, 0, count);

        if (ValidateName(FinalName))
        {
            //todo:
            //SelectedScene.Name = FinalName;
            DockerMap.SetVariable(SelectedScene, "Name", FinalName);
        }
    }

    public bool ValidateName(string Name) {
        return !Osmium.ContainsScene(Name) && Name != string.Empty && Name.Length <= NameLimit;
    }

    public string GenerateDefaultName() {
        string returnValue = "New Scene";

        uint count = 0;
        while(!ValidateName(returnValue))
        {
            count++;
            returnValue = "New Scene " + "(" + count + ")";
        }
        
        return returnValue;
    }
    
    
}