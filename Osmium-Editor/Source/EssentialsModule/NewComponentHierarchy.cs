using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using ImGuiNET;
using OsmiumEditor.Source.DearImGUINET.Structure;
using OsmiumEditor.Source.NewEditor.Serialization;
using OsmiumNucleus;

namespace OsmiumEditor.Source.NewEditor;

public class ComponentHierarchy : EditorWindow
{

    public const int CHILD_INDENT_WIDTH = 7;

    public const int SINGLE_COMPONENT_INDENT_WIDTH = 29;

    public static Component? SelectedComponent;

    public byte[] RenameBuffer = [];

    public const int NameLimit = 512;

    public bool Renaming;

    public bool SetFocus;

    private int IDCount = 0;
    
    public const float lineSize = 3;

    private Component draggedComponent;

    private int spaghettiIndent = 0;

    

    protected internal override void Draw() {

        ImGui.Begin("Components");
        
        if (ImGui.IsMouseClicked(ImGuiMouseButton.Left) && ImGui.IsWindowHovered() && !ImGui.IsAnyItemActive()) {
            SelectedComponent = null;
        }

        if (SceneHierarchy.SelectedScene != null)
        {
            if (ImGui.BeginPopupContextWindow())
            {
            
                if(Renaming)
                    ImGui.CloseCurrentPopup();

                if (ImGui.Button("Add"))
                {
                    ImGui.CloseCurrentPopup();
                    AddComponent(SceneHierarchy.SelectedScene);
                }

                ImGui.EndPopup();
            }

            IDCount = 0;
            
            foreach (Component component in DockerMap.FindMappedScene(SceneHierarchy.SelectedScene).Children.ToArray())
            {
                DisplayComponent(component, 0);
            }
        }

        ImGui.End();
    }

    public void DisplayComponent(Component component, int depth) {

        bool treeNode = false;

        for (int i = 0; i < spaghettiIndent; i++) {
            //ImGui.Unindent();
        }
        
        for (int i = 0; i < component.AllParents.Count; i++) {
            ImGui.Indent(CHILD_INDENT_WIDTH);
        }
        
        if (component.Children.Count != 0) {
            treeNode = ImGui.TreeNodeEx("##" + IDCount, ImGuiTreeNodeFlags.DefaultOpen);
            ImGui.SameLine();
        }
        else
        {
            ImGui.Indent(SINGLE_COMPONENT_INDENT_WIDTH);
        }
        
        string name = component.Name == string.Empty ? component.GetType().Name : component.Name;
        
        if(!(Renaming && SelectedComponent == component)) {
            if (ImGui.Selectable(name + "##" + IDCount, SelectedComponent == component)) SelectedComponent = component;
        }else {
            if (SetFocus)
            {
                ImGui.SetKeyboardFocusHere();
                SetFocus = false;
            }
            
            if (ImGui.InputText("##ComponentRenameInput", RenameBuffer, NameLimit, ImGuiInputTextFlags.EnterReturnsTrue) || ImGui.IsItemDeactivated()) {
                FinishRenaming();
            }
        }
        
        if (ImGui.BeginDragDropSource()) {
            int sourceIndex = IDCount;

            draggedComponent = component;
            unsafe
            {
                ImGui.SetDragDropPayload("SceneSwap", IntPtr.Zero, 0);
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
            if (t <= .25f)
            {
                DrawList.AddLine(min, new Vector2(max.X, min.Y), ImGui.GetColorU32(ImGuiCol.HeaderActive), lineSize);
            }
            else if(t >= .75f)
            {
                DrawList.AddLine(new Vector2(min.X, max.Y), max, ImGui.GetColorU32(ImGuiCol.HeaderActive), lineSize);
            }else {
                DrawList.AddRect(min, max, ImGui.GetColorU32(ImGuiCol.HeaderActive));
            }
            unsafe
            {
                if (payload.NativePtr != null)
                {
                    ComponentMapNode draggedSerializedComponent = DockerMap.FindMappedComponent(this.draggedComponent as Component);
                    ComponentMapNode targetSerializedComponent = DockerMap.FindMappedComponent(component);

                    SerializedDocker draggedParent = DockerMap.FindMappedDocker(draggedSerializedComponent.component.Parent);
                    SerializedDocker targetParent = DockerMap.FindMappedDocker(component.Parent);
                    
                    if (t <= .25f)
                    {
                        draggedParent.Children.Remove(draggedSerializedComponent);

                        int index = targetParent.Children.IndexOf(targetSerializedComponent);
                        if (index < 0) index = 0;
                        else if(index > targetParent.Children.Count) index = targetParent.Children.Count;

                        targetParent.Children.Insert(index, draggedSerializedComponent);

                    }else if (t >= .75f)
                    {
                        draggedParent.docker.Destroy(draggedSerializedComponent.component);

                        int index = targetParent.Children.IndexOf(targetSerializedComponent);
                        index++;
                        if (index < 0) index = 0;
                        else if(index > targetParent.Children.Count) index = targetParent.Children.Count;

                        targetParent.Children.Insert(index, draggedSerializedComponent);
                    }else
                    {
                        draggedSerializedComponent.component.Move(targetSerializedComponent.component);
                    }
                }
            }
            
            ImGui.EndDragDropTarget();
        }

        if (ImGui.BeginPopupContextItem())
        {

            if (ImGui.Button("Add")) {
                ImGui.CloseCurrentPopup();
                AddComponent(component);
            }

            if (ImGui.Button("Rename")) {
                ImGui.CloseCurrentPopup();
                StartRenaming(component);
            }
            
            if (ImGui.Button("Pack")) {
                ImGui.CloseCurrentPopup();

                Package newParent = component.Parent.Add<Package>();
                
                component.Move(newParent);

                if (component.Name != string.Empty) {
                    newParent.Name = component.Name;
                }
            }

            if (ImGui.Button("Destroy")) {
                component.Destroy();
            }
            
            ImGui.EndPopup();
        }
        
        IDCount++;
        
        //todo: make coroutines work with disable?
        
        for (int i = 0; i < component.AllParents.Count; i++) {
            ImGui.Unindent(CHILD_INDENT_WIDTH);
        }
        
        if (component.Children.Count == 0) {
            ImGui.Unindent(SINGLE_COMPONENT_INDENT_WIDTH);
        }
        
        if (!treeNode) return;
        


        //if (component.GetType() != typeof(Package)) {
            foreach (Component child in component)
            {
                DisplayComponent(child, depth + 1);
            }
            
        ImGui.TreePop();
        //}
    }

    public void StartRenaming(Component component) {
        SelectedComponent = component;
        Renaming = true;
        SetFocus = true;

        RenameBuffer = new byte[NameLimit];
        Encoding.UTF8.GetBytes(component.Name).CopyTo(RenameBuffer);
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
            //SelectedComponent?.Name = FinalName;
            DockerMap.SetVariable(SelectedComponent, "Name", FinalName);
        }
    }

    public bool ValidateName(string Name) {
        return Name != string.Empty && Name.Length <= NameLimit;
    }

    public void AddComponent(ComponentDocker parent) {
        Component newComponent = parent.Add<Package>();
        newComponent.Name = newComponent.GetType().Name;
        StartRenaming(newComponent);
    }
    
    
}