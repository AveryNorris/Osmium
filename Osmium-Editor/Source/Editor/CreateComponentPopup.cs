using System.Numerics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OsmiumEditor.Source;
using OsmiumNucleus;
using OsmiumRadium;


namespace OsmiumEditor;


public class CreateComponentPopup : RadiumElement
{
    public string ComponentName = "";

    protected override void Draw() {
        ComponentName += Backend.TextInput;

        //todo: blinking line for text input??? and make htis a radium thing and fix myu naming conventoins and do everything in the whole wide owr;d
        var BackgroundBox = new Box(new Transform(size: new Vector2(10, 10), center: Vector2.One * 50), Palette.BackgroundLow);

        //todo: make text support transform somehow naming conventions hello
        var OptionText = new Text(ComponentName, center: Vector2.One * 50);
        
        //todo: cache component types in assembly window and unload them
        Type[] componentTypes = AssemblyWindow.GetComponents();
        componentTypes = componentTypes.Append(typeof(Package)).ToArray();

        List<Type> filteredTypes = [];
        
        for (int i = 0; i < componentTypes.Length; i++) {
            if (componentTypes[i].Name.Contains(ComponentName)) {
                filteredTypes.Add(componentTypes[i]);
                var ComponentNameText = new Text(componentTypes[i].Name, center: Vector2.One * 50 + new Vector2(0, 2) * i);
            }
        }

        if (Osmium.Context!.KeyboardState.IsKeyDown(Keys.Enter)) {
            if (SceneHierarchy.SelectedScene != null) {
                //todo: preemtive guard and guard class

                if (filteredTypes.Count != 0)
                {

                    Component createdComponent = (Activator.CreateInstance(filteredTypes[0])! as Component)!;

                    if (ComponentHierarchy.SelectedComponent != null) {
                        ComponentHierarchy.SelectedComponent.Add(createdComponent);
                    }
                    else
                    {
                        SceneHierarchy.SelectedScene.Add(createdComponent);
                    }

                }
                
                Radium.Remove<CreateComponentPopup>();
            }
        }
    }
}