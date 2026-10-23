using System.Collections;
using System.Reflection;


namespace OsmiumNucleus;



/// <summary> Any object that transfers updates, and has GameObject children. </summary>
public abstract partial class Entity : IEnumerable<GameObject>
{
    
    
    
    /// <summary> Blocks external inheritance </summary>
    internal Entity() {}



    /// <summary> Whether the <see cref="Entity"/> is receiving events or not. </summary>
    public bool Enabled { get; internal set; } = true;

    /// <summary> If the <see cref="Entity"/> has been destroyed. </summary>
    public bool Destroyed { get; internal set; } = false;
    
    
    
    
    IEnumerator IEnumerable.GetEnumerator() { return  GetEnumerator(); }

    public IEnumerator<GameObject> GetEnumerator() { return _components.GetEnumerator(); }
    
    public static event Action<Entity, Entity, GameObject>? ComponentMoved;
    
    public static event Action<Entity, GameObject>? ComponentAdded;
    
    public static event Action<Entity, GameObject>? ComponentDestroyed;
    
    
    
    /// <summary> Core of the Docker, holds all the Components.</summary>
    [UnsafeInternal] private readonly List<GameObject> _components = [];
    /// <summary> Holds a list of Components at each of their types. This optimizes <see cref="Get()"/> to O(1) </summary>
    [UnsafeInternal] private readonly Dictionary<Type, HashSet<GameObject>> _componentTypeDictionary = new();
    /// <summary> Stores a GameObject in a list at each of their tags. This optimizes Get(string tag) to O(1)</summary>
    [UnsafeInternal] private readonly Dictionary<string, HashSet<GameObject>> _componentTagDictionary = new();
    
        
    //Treats a destroyed Entity as null
    public static bool operator ==(Entity? __left, Entity? __right) {
        if (__left is null) {
            if (__right is null) return true;
            return __right.Destroyed;
        }

        if (__right == null) { 
            return __left.Destroyed;
        }
        
        return ReferenceEquals(__left, __right);
    }

    public static bool operator !=(Entity? __left, Entity? __right) {
        return !(__left == __right);
    }
    
    
    public override bool Equals(object? obj) { return Destroyed ? obj == null : ReferenceEquals(this, obj); }
    
    
    [UnsafeInternal]
    internal void ChainEvent(Event __timeEvent) {
        if (!Enabled) return;

        foreach (GameObject gameObject in _components) {
            gameObject.TryEvent(__timeEvent);
            gameObject.ChainEvent(__timeEvent);
        }
    }
    
    /// <summary> Add a GameObject to the Docker. </summary>
    [UnsafeInternal]
    private void AddComponentToLists(GameObject gameObject) {
        Type Type = gameObject.GetType();
        _components.Add(gameObject); 
        
        if (!_componentTypeDictionary.TryAdd(Type, [gameObject])) _componentTypeDictionary[Type].Add(gameObject);

        foreach (string tag in gameObject._tags) HashTaggedComponent(tag, gameObject);
    }

    
    
    /// <summary> Removes a GameObject from the Docker. </summary>
    [UnsafeInternal]
    private void RemoveComponentFromLists(GameObject gameObject) {
        Type Type = gameObject.GetType();
        _components.Remove(gameObject); 
        
        if (_componentTypeDictionary.TryGetValue(Type, out HashSet<GameObject>? componentsOfType)) {
            //if there is only one item or less in the list, just remove it from the dictionary as a whole since the priority of this has now changed.
            if (componentsOfType.Count > 1) componentsOfType.Remove(gameObject);
            else _componentTypeDictionary.Remove(Type);
        }
        
        foreach (string tag in gameObject._tags) UnhashTaggedComponent(tag, gameObject);
    }

    /// <summary> Hashes a component in the tag dictionary </summary>
    [UnsafeInternal]
    internal void HashTaggedComponent(string __tag, GameObject gameObject) {
        if (!_componentTagDictionary.TryAdd(__tag, [gameObject])) 
            _componentTagDictionary[__tag].Add(gameObject);
    }

    /// <summary> Removes a component's hash from the tag dictionary </summary>
    [UnsafeInternal]
    internal void UnhashTaggedComponent(string __tag, GameObject gameObject) {
        if (_componentTagDictionary.TryGetValue(__tag, out HashSet<GameObject>? value)) {
            value.Remove(gameObject); if(value.Count == 0) _componentTagDictionary.Remove(__tag);
        }
    }
    
    internal static void ClearCollectibleAssemblies() {
        if(ComponentMoved != null) foreach(Action<Entity, Entity, GameObject> action in ComponentMoved.GetInvocationList()) if(action.GetMethodInfo().IsCollectible) ComponentMoved -= action;
        
        if(ComponentAdded != null) foreach(Action<Entity, GameObject> action in ComponentAdded.GetInvocationList()) if(action.GetMethodInfo().IsCollectible) ComponentAdded -= action;
        
        if(ComponentDestroyed != null) foreach(Action<Entity, GameObject> action in ComponentDestroyed.GetInvocationList()) if(action.GetMethodInfo().IsCollectible) ComponentDestroyed -= action;
    }
    

}