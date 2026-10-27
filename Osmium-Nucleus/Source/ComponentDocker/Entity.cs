using System.Collections;
using System.Reflection;


namespace OsmiumNucleus;



/// <summary> An object that transfers updates, and has GameObject children. </summary>
public abstract partial class Entity
{
    
    
    
    /// <summary> Blocks external inheritance </summary>
    internal Entity() {}



    /// <summary> Whether the <see cref="Entity"/> is receiving events or not. </summary>
    public bool Enabled { get; internal set; } = true;

    /// <summary> If the <see cref="Entity"/> has been destroyed. </summary>
    public bool Destroyed { get; internal set; } = false;
    
    
    
    public static event Action<Entity, Entity, GameObject>? ComponentMoved;
    
    public static event Action<Entity, GameObject>? ComponentAdded;
    
    public static event Action<Entity, GameObject>? ComponentDestroyed;
    
    
    
    /// <summary> Core of the Parent, holds all the Components.</summary>
    internal readonly List<GameObject> _components = [];
    
        
    //Treats a destroyed Entity as null
    public static bool operator ==(Entity __left, Entity __right) {
        if (__left is null) {
            if (__right is null) return true;
            return __right.Destroyed;
        }

        if (__right == null) { 
            return __left.Destroyed;
        }
        
        return ReferenceEquals(__left, __right);
    }

    public static bool operator !=(Entity __left, Entity __right) {
        return !(__left == __right);
    }
    
    
    public override bool Equals(object? obj) { return Destroyed ? obj == null : ReferenceEquals(this, obj); }
    
    
    internal void ChainEvent(Event __timeEvent) {
        if (!Enabled) return;

        foreach (GameObject gameObject in _components) {
            gameObject.TryEvent(__timeEvent);
            gameObject.ChainEvent(__timeEvent);
        }
    }
    
    /// <summary> Add a GameObject to the Parent. </summary>
    private void AddComponentToLists(GameObject gameObject) {
        _components.Add(gameObject);
    }

    
    
    /// <summary> Removes a GameObject from the Parent. </summary>
    private void RemoveComponentFromLists(GameObject gameObject) {
        _components.Remove(gameObject);
    }
    
    internal static void ClearCollectibleAssemblies() {
        if(ComponentMoved != null) foreach(Action<Entity, Entity, GameObject> action in ComponentMoved.GetInvocationList()) if(action.GetMethodInfo().IsCollectible) ComponentMoved -= action;
        
        if(ComponentAdded != null) foreach(Action<Entity, GameObject> action in ComponentAdded.GetInvocationList()) if(action.GetMethodInfo().IsCollectible) ComponentAdded -= action;
        
        if(ComponentDestroyed != null) foreach(Action<Entity, GameObject> action in ComponentDestroyed.GetInvocationList()) if(action.GetMethodInfo().IsCollectible) ComponentDestroyed -= action;
    }
    

}