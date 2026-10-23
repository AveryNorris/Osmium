namespace OsmiumNucleus;


#nullable enable
public abstract partial class GameObject
{
    
    
    
    /// <summary> Scene the GameObject resides in. To save ram it's not stored but calculated on the fly. </summary>
    public Scene Scene => __QueryScene();
    private Scene __QueryScene() {
        if (Docker is Scene scene) return scene;
        if (Docker is GameObject Component) return Component.__QueryScene();
        
        Debug.Error("Failed To Find Scene? Osmium does not know what is going on but you beefed up.");
        
        return null;
    }
    
    
    
    /// <summary> Returns the Docker GameObject. Will be null if the GameObject's parent is not a GameObject. </summary>
    public GameObject? Parent => __QueryComponentParent();
    private GameObject? __QueryComponentParent() {
        if (Docker is GameObject _component)
            return _component;
        return null;
    }
    
    
    
    /// <summary> Returns the Docker Scene. Will be null if the GameObject's parent is not a Scene. </summary>
    public Scene? SceneParent => __QuerySceneParent();
    private Scene? __QuerySceneParent() {
        if (Docker is Scene _scene)
            return _scene;
        return null;
    }
    
    
    
    /// <summary> All parent Components and the parents of the parents up until the Scene. Will only list parents of parents, not uncle Components. And will not list the Scene </summary>
    public IReadOnlyList<GameObject> AllParents => __QueryComponents();
    private IReadOnlyList<GameObject> __QueryComponents() {
        List<GameObject> returnValue = [];
        Entity currentEntity = Docker;

        while (true) {
            if (currentEntity is GameObject Component) {
                returnValue.Add(Component);
                currentEntity = Component.Docker;
            } else {
                break;
            }
        }

        return returnValue;
    }

    
}