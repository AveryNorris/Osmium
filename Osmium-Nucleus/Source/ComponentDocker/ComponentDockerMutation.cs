namespace OsmiumNucleus;


public abstract partial class ComponentDocker
{
    
    
    
    /// <summary> Is called when a new <see cref="Component"/> is added!
    /// The first parameter is the parent, and the second is the component that was added </summary>
    public static event Action<ComponentDocker, Component>? ComponentAdded;
    /// <summary> Is called when a new <see cref="Component"/> is removed!
    /// The first parameter is the parent, and the second is the component that was removed </summary>
    public static event Action<ComponentDocker, Component>? ComponentRemoved;

    
    
    /// <summary> Creates a new instance of that type of component and attaches it to the docker, then returns a reference to it.</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.Medium), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.O1)]
    public __Type Add<__Type>(string? name = null, ICollection<string>? tags = null, int priority = 0, bool enabled = true) where __Type : Component, new() {
        Component newComponent = new __Type();

        name ??= typeof(__Type).Name;
        tags ??= [];

        InitiateComponent(newComponent, name, tags.ToHashSet(), priority, enabled);
        
        return (__Type)newComponent;
    }
    
    
    
    /// <summary> Adds a custom component type and does not call any create events. This is for the editor!</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.Medium), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.O1), MarkerAttributes.UnsafePipeline]
    public void Add(Component __component) {
        __component.Parent = this; 
        AddComponentToLists(__component);
        
        __component.TryEvent(Event.Create);
        __component.ChainEvent(Event.Create);
        
        ComponentAdded?.Invoke(this, __component);
    }

    

    /// <summary> Initiates a component into the docker. </summary>
    [MarkerAttributes.UnsafeInternal]
    private void InitiateComponent(Component __component, string __name, HashSet<string> __tags, int __priority, bool __enabled) {
        //add to Component Docker's lists
        AddComponentToLists(__component);

        __component.Parent = this;
        
        __component.Name = __name;
        __component.Priority = __priority;
        __component._tags = __tags;
        
        //create event
        __component.TryEvent(Event.Create);
        __component.ChainEvent(Event.Create);
        
        ComponentAdded?.Invoke(this, __component);
    }





    /// <summary> Destroys a component attached to the Docker </summary>
    /// <param name="__component"></param>
    public void Destroy(Component __component) {
        if(__component == null) { Debug.Error("A given Component cannot be null!"); return; }
        if(!Contains(__component)) { Debug.Error("This Docker does not own the given Component"); return; }
        
        __component.TryEvent(Event.Destroy);
        __component.ChainEvent(Event.Destroy);

        RemoveComponentFromLists(__component);
        __component.Parent = null;
        
        ComponentRemoved?.Invoke(this, __component);
    }



    /// <summary> Destroys all the components in a given list </summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.Medium), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)] 
    public void DestroyAll() => DestroyAll(GetAll());



    /// <summary> Destroys all the components in a given list </summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.Medium), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public void DestroyAll(ICollection<Component> __Components) { foreach (Component component in __Components.ToArray()) Destroy(component); }



    /// <summary> Destroys the first component with a given Type</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.Low), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.O1)]
    public void Destroy<__Type>() where __Type : Component => Destroy(Get<__Type>());



    /// <summary> Destroys all components of a given type</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.Medium), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public void DestroyAll<__Type>() where __Type : Component => DestroyAll(new List<Component>(GetAll<__Type>()));



    /// <summary> Destroys all components that have all the given tags</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.Medium), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public void DestroyAll(ICollection<string> __tags) => DestroyAll(GetAll(__tags));




    /// <summary> Destroys all Components that have the given type, and all the given tags</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.Medium), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public void DestroyAll<__Type>(ICollection<string> __tags) where __Type : Component => DestroyAll((ICollection<Component>) GetAll<__Type>(__tags));




    /// <summary> Destroys all the components with the given tag</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.Medium), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public void DestroyAll(string __tag) => DestroyAll(GetAll([__tag]));



    /// <summary> Destroys all the components that have a certain type, and a certain tag</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.Medium), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public void DestroyAll<__Type>(string __tag) where __Type : Component => DestroyAll((ICollection<Component>) GetAll<__Type>([__tag]));



    /// <summary> Destroys the first component with the given tag</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.Low), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.O1)]
    public void Destroy(string __tag) => Destroy(Get(__tag));



    /// <summary> Destroys the Destroys component with the given type and tag</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.Low), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public void Destroy<__Type>(string __tag) where __Type : Component => Destroy(Get<__Type>(__tag));

    
    
}