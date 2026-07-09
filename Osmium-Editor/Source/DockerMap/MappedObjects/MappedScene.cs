using OsmiumEditor.Source.NewEditor.Serialization;
using OsmiumNucleus;

namespace OsmiumEditor;

public class SerializedScene(Scene scene) : SerializedDocker(scene) {
    public Scene scene = scene;
    public SolidType? reloadMemoryType = new SolidType(scene);

    public static implicit operator Scene(SerializedScene node) => node.scene;

    public void Unload() {
        scene = null;
        docker = null;
        foreach (ComponentMapNode component in Children) {
            component.Unload();
        }
    }

    public void Construct() {
        scene = (Scene)Activator.CreateInstance(reloadMemoryType.FindType(), "MAP_GENERATED_SCENE");
        Osmium.AddScene(scene);
        
        docker = scene;

        foreach (SerializedVariable variable in SerializedFields) variable.Load(docker);
        

        foreach (ComponentMapNode child in Children) child.Construct(this);
    }
}