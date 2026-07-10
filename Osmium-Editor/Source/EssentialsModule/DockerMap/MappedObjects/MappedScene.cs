using OsmiumEditor.Source.NewEditor.Serialization;
using OsmiumNucleus;

namespace OsmiumEditor;

public class SerializedScene(Scene scene) : SerializedDocker(scene) {
    public Scene? scene => docker as Scene;
    public SolidType? reloadMemoryType = new SolidType(scene);

    public static implicit operator Scene(SerializedScene node) => node.scene;

    public void Unload() {
        docker = null;
        foreach (ComponentMapNode component in Children) {
            component.Unload();
        }
    }

    public void Construct() {
        Osmium.AddScene(scene);
        
        docker = scene;

        foreach (SerializedVariable variable in SerializedFields) variable.Load(docker);
        

        foreach (ComponentMapNode child in Children) child.Construct(this);
    }
}