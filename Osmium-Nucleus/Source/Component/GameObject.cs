namespace OsmiumNucleus;


/// <summary> The backing class for all Components in Osmium. Inherit this to become one. </summary>
/// <author> Avery Norris </author>
public abstract class GameObject : Entity
{
    


    /// <summary> Current parent of the GameObject. Can be either a Scene or another GameObject.</summary>
    public Entity Parent { get; internal set; }
    
    
    /// <summary> The GameObject's name </summary>
    public string Name {
        get => field;
        set { 
            if (value == null) { Debug.Error("A GameObject's name cannot be null!"); return; } 
            
            field = value; 
        }
    }
    

    
    
    /// <summary> A list of all tags belonging to the component. Use AddTag() to modify it. Easy to calculate, but setting this requires that the Parent resorts Components; </summary>
    public HashSet<string> Tags = [];
    
    
    
    /// <summary> Attempts to send an event to the component, and quietly exits if not.</summary>
    internal void TryEvent(Event __timeEvent) {
        EventManager.EventProfile profile = EventManager._TypeAssociatedTimeEvents[GetType()];
        
        if ((!Enabled || !Osmium.IsRunning) && !profile.AlwaysUpdate) return;
        
        profile.Callbacks[(int) __timeEvent]?.Invoke(this);
    }
    

    /// <summary> Just gives the GameObject's name. But makes debugging a little easier :).</summary>
    public override string ToString() => this.Name;
    
    /// <summary> Scene the GameObject resides in. To save ram it's not stored but calculated on the fly. </summary>
    public Scene Scene => __QueryScene();
    private Scene __QueryScene() {
        if (Parent is Scene scene) return scene;
        if (Parent is GameObject Component) return Component.__QueryScene();
        
        Debug.Error("Failed To Find Scene? Osmium does not know what is going on but you beefed up.");
        
        return null;
    }
    

    /// <inheritdoc cref="Entity.Move(CoEntityentDocker)"/>
    public void Move(Entity __newDocker) => Parent.Move(__newDocker, this);



    /// <summary> Makes the GameObject destroy itself </summary>
    public void Destroy() => Parent.Destroy(this);
    
}