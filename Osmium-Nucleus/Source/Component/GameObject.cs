namespace OsmiumNucleus;


/// <summary> The backing class for all Components in Osmium. Inherit this to become one. </summary>
/// <author> Avery Norris </author>
#nullable enable
public abstract partial class GameObject : Entity   
{
    


    /// <summary> Current parent of the GameObject. Can be either a Scene or another GameObject.</summary>
    public Entity Docker { get; internal set; }
    
    #region GameObject Information
    
    /// <summary> The GameObject's name </summary>
    public string Name {
        get => _name;
        set { 
            if (value == null) { Debug.Error("A GameObject's name cannot be null!"); return; } 
            
            _name = value; 
        }
    } private string _name = String.Empty;
    

    #endregion Tags
    
    #region Tags
    
    /// <summary> A list of all tags belonging to the component. Use AddTag() to modify it. Easy to calculate, but setting this requires that the Docker resorts Components; </summary>
    public IReadOnlySet<string> Tags {
        get => _tags;
        set {
            HashSet<string> previousTags = _tags;
            
            foreach (string tag in _tags.ToArray()) if(!value.Contains(tag)) RemoveTag(tag);
            foreach (string tag in value) if(!previousTags.Contains(tag)) AddTag(tag);
        }
    } internal HashSet<string> _tags = [];
    
    

    /// <summary> Adds a new tag to the component</summary>
    public void AddTag(string __tag) {
        if(__tag == null) { Debug.Error("A GameObject's tag cannot be null!"); return; }
        if(Tags.Contains(__tag)) { Debug.Error("GameObject already has the given tag!", ["ComponentName", "Tag"], [_name, __tag]); return; }

        _tags.Add(__tag);
        Docker.HashTaggedComponent(__tag, this);
    }
    
    
    
    /// <summary> Removes a tag from the component.</summary>
    public void RemoveTag(string __tag) {
        if (__tag == null) { Debug.Error("Cannot remove a null tag! It is impossible for Components to have null tags"); return;};
        if(!Tags.Contains(__tag)) { Debug.Error("GameObject does not have the given tag!", ["ComponentName", "Tag"], [_name, __tag]); return; }

        _tags.Remove(__tag);
        Docker.UnhashTaggedComponent(__tag, this);
    }
    
    #endregion Tags
    
    
    
    /// <summary> Attempts to send an event to the component, and quietly exits if not.</summary>
    internal void TryEvent(Event __timeEvent) {
        EventManager.EventProfile profile = EventManager._TypeAssociatedTimeEvents[GetType()];
        
        if ((!Enabled || !Osmium.IsRunning) && !profile.AlwaysUpdate) return;
        
        profile.Callbacks[(int) __timeEvent]?.Invoke(this);
    }
    

    /// <summary> Just gives the GameObject's name. But makes debugging a little easier :).</summary>
    public override string ToString() => this.Name;
    
}