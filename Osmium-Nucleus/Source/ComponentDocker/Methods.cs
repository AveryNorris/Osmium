using System.Reflection;

namespace OsmiumNucleus;



public abstract partial class ComponentDocker
{
    
    
    public static event Action<ComponentDocker, ComponentDocker, Component>? ComponentMoved;
    
    public static event Action<ComponentDocker, Component>? ComponentAdded;
    
    public static event Action<ComponentDocker, Component>? ComponentDestroyed;

    internal static void ClearCollectibleAssemblies() {
        if(ComponentMoved != null) foreach(Action<ComponentDocker, ComponentDocker, Component> action in ComponentMoved.GetInvocationList()) if(action.GetMethodInfo().IsCollectible) ComponentMoved -= action;
        
        if(ComponentAdded != null) foreach(Action<ComponentDocker, Component> action in ComponentAdded.GetInvocationList()) if(action.GetMethodInfo().IsCollectible) ComponentAdded -= action;
        
        if(ComponentDestroyed != null) foreach(Action<ComponentDocker, Component> action in ComponentDestroyed.GetInvocationList()) if(action.GetMethodInfo().IsCollectible) ComponentDestroyed -= action;
    }

    
    
    /// <summary> Tests to see if every single <see cref="Component"/> is a direct child of the <see cref="ComponentDocker"/>. </summary>
    /// <param name="Components">The components that are being tested</param>
    /// <returns> True if every <see cref="Component"/> in <see cref="Components"/> belongs to the Docker, otherwise false. </returns>
    public bool Contains(params IEnumerable<Component> Components) => Components.All(x => _components.Contains(x));

    /// <summary> Tests if the <see cref="ComponentDocker"/> contains a components of type <see cref="TComponent"/>. </summary>
    /// <typeparam name="TComponent"> The type of the <see cref="Component"/> you want to check for </typeparam>
    /// <returns> True if the Docker has at least one child of type <see cref="TComponent"/>, otherwise false. </returns>
    public bool Contains<TComponent>() where TComponent : Component, new() => _componentTypeDictionary.ContainsKey(typeof(TComponent));

    /// <summary> Tests to see if the <see cref="ComponentDocker"/> has a child with all the given tags at the very least.</summary>
    /// <param name="Tags"> The list of tags to find on a <see cref="Component"/> </param>
    /// <returns> True if there is at last one <see cref="Component"/> with at least all the given tags, otherwise false. </returns>
    public bool Contains(params IEnumerable<string> Tags) => GetAll(Tags).Any();

    /// <summary> Tests to see if the <see cref="ComponentDocker"/> has a child with at least all the given tags, and of type <see cref="TComponent"/>. </summary>
    /// <param name="Tags"> The list of tags to find on a Component </param>
    /// <typeparam name="TComponent"> The type of <see cref="Component"/> to search for </typeparam>
    /// <returns> True if there is at least one <see cref="Component"/> of type <see cref="TComponent"/> and with all tags in <see cref="Tags"/> </returns>
    public bool Contains<TComponent>(params IEnumerable<string> Tags) where TComponent : Component, new() => GetAll<TComponent>(Tags).Any();
    


    /// <summary> Finds the very first <see cref="Component"/> owned by the <see cref="ComponentDocker"/> that belongs to the <see cref="ComponentDocker"/>. </summary>
    /// <returns> The very first <see cref="Component"/> owned by the <see cref="ComponentDocker"/> found in the <see cref="ComponentDocker"/>, null if there is none. </returns>
    public Component? Get() => _components.FirstOrDefault();

    /// <summary> Finds the very first <see cref="Component"/> owned by the <see cref="ComponentDocker"/> that is of type <see cref="TComponent"/>. </summary>
    /// <typeparam name="TComponent"> The true of <see cref="Component"/> to search for </typeparam>
    /// <returns> The very first <see cref="Component"/> owned by the <see cref="ComponentDocker"/> of type <see cref="TComponent"/>, null if there is none. </returns>
    public TComponent? Get<TComponent>() where TComponent : Component, new() => GetAll<TComponent>().FirstOrDefault();

    /// <summary> Finds the very first <see cref="Component"/> owned by the <see cref="ComponentDocker"/> that has all the tags in <see cref="Tags"/>. </summary>
    /// <param name="Tags"> The tags to search for </param>
    /// <returns> The first <see cref="Component"/> owned by the <see cref="ComponentDocker"/> that has all the tags present in <see cref="Tags"/>, null if there is none. </returns>
    public Component? Get(params IEnumerable<string> Tags) => GetAll(Tags).FirstOrDefault();
    
    /// <summary> Finds the first <see cref="Component"/> owned by the <see cref="ComponentDocker"/> that is of type <see cref="TComponent"/> and has all the tags in <see cref="Tags"/>. </summary>
    /// <param name="Tags"> The tags to search for </param>
    /// <typeparam name="TComponent"> The type of <see cref="Component"/> to search for </typeparam>
    /// <returns> The first <see cref="Component"/> owned by the <see cref="ComponentDocker"/> of type <see cref="TComponent"/> and has all the tags present in <see cref="Tags"/>, null if there is none. </returns>
    public TComponent? Get<TComponent>(params IEnumerable<string> Tags) where TComponent : Component, new() => GetAll<TComponent>(Tags).FirstOrDefault();
    
    
    
    /// <summary> Finds all <see cref="Component">Components</see> that belong to the <see cref="ComponentDocker"/>. </summary>
    /// <returns> Returns a <see cref="List{T}"/> of all <see cref="Component">Components</see> that belongs to the <see cref="ComponentDocker"/>. </returns>
    public List<Component> GetAll() => _components;

    /// <summary> Finds all <see cref="Component">Components</see> owned by the <see cref="ComponentDocker"/> of type <see cref="TComponent"/>. </summary>
    /// <typeparam name="TComponent">The type of <see cref="Component"/> to search for. </typeparam>
    /// <returns> A <see cref="List{T}"/> of all <see cref="Component">Components</see> that belong to the <see cref="ComponentDocker"/> and are of type <see cref="TComponent"/>. </returns>
    public List<TComponent> GetAll<TComponent>() where TComponent : Component, new() {
        if (_componentTypeDictionary.TryGetValue(typeof(TComponent), out HashSet<Component>? components))
        {
            //todo: BAD TO LIST CALL! MAYBE TYPEDICTIONARY is a list?
            return components.OfType<TComponent>().ToList();
        }
        
        return [];
    }
    
    /// <summary> Finds all <see cref="Component">Components</see> owned by the <see cref="ComponentDocker"/> that have all the tags in <see cref="Tags"/>. </summary>
    /// <param name="Tags"> All the tags to search for. </param>
    /// <returns> A <see cref="List{T}"/> of all <see cref="Component">Components</see> that belong to the <see cref="ComponentDocker"/> and have all the tags in <see cref="Tags"/>. </returns>
    public List<Component> GetAll(params IEnumerable<string> Tags) {
        if (Tags == null) { Debug.Error("The given Tags cannot be null!"); return []; }
        if (Tags.Contains(null)) { Debug.Error("A given Tag cannot be null!"); return []; }
        if(!Tags.Any()) return GetAll(); 
        
        HashSet<Component> components;
        if (_componentTagDictionary.TryGetValue(Tags.First(), out HashSet<Component>? firstComponents)) components = firstComponents.ToHashSet(); else return [];
        
        foreach(string tag in Tags)
            if (_componentTagDictionary.TryGetValue(tag, out HashSet<Component>? taggedComponents))
                components.RemoveWhere(x => !taggedComponents.Contains(x));

        return components.ToList();
    }

    /// <summary> Finds all the <see cref="Component">Components</see> owned by the <see cref="ComponentDocker"/> of type <see cref="TComponent"/> that have all the tags in <see cref="Tags"/>. </summary>
    /// <param name="Tags"> All the tags to search for. </param>
    /// <typeparam name="TComponent"> The type of <see cref="Component"/> to search for .</typeparam>
    /// <returns> A <see cref="List{T}"/> of all <see cref="Component">Components</see> that belong ot the <see cref="ComponentDocker"/> of type <see cref="TComponent"/> and have all the tags in <see cref="Tags"/>. </returns>    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public List<TComponent> GetAll<TComponent>(IEnumerable<string> Tags) where TComponent : Component, new() {
        if (Tags == null) { Debug.Error("The given Tags cannot be null!"); return []; }
        if (Tags.Contains(null)) { Debug.Error("A given Tag cannot be null!"); return []; }
        if (!Tags.Any()) return GetAll<TComponent>();
        
        HashSet<TComponent> components = [];

        if (_componentTagDictionary.TryGetValue(Tags.First(), out HashSet<Component>? firstComponents)) 
            foreach (Component component in firstComponents) if (component is TComponent typedComponent) components.Add(typedComponent);
        
        foreach(string tag in Tags)
            if (_componentTagDictionary.TryGetValue(tag, out HashSet<Component>? taggedComponents)) 
                components.RemoveWhere(x => !taggedComponents.Contains(x));

        return components.ToList();
    }



    /// <summary> Adds every <see cref="Component"/> in <see cref="Components"/> as children to the <see cref="ComponentDocker"/>. </summary>
    /// <param name="Components"> All the <see cref="Component">Components</see> to add. </param>
    public void Add(params IEnumerable<Component> Components) {
        foreach (Component component in Components) {
            component.Parent = this;
        
            //add to Component Docker's lists
            AddComponentToLists(component);

            component.Parent = this;
        
            //create event
            component.TryEvent(Event.Create);
            component.ChainEvent(Event.Create);
        
            ComponentAdded?.Invoke(this, component);
        }
    }

    /// <summary> Adds a new <see cref="Component"/> of type <see cref="TComponent"/>. </summary>
    /// <typeparam name="TComponent"> The type of Component to add. </typeparam>
    /// <returns> The new <see cref="Component"/> that was created. </returns>
    public TComponent Add<TComponent>() where TComponent : Component, new() { TComponent component = new TComponent(); Add(component); return component; }

    /// <summary> Adds a new <see cref="Component"/> of type <see cref="TComponent"/> with the name <see cref="Name"/>. </summary>
    /// <param name="Name"> The name of the <see cref="Component"/>. </param>
    /// <typeparam name="TComponent"> The type of <see cref="Component"/> to add. </typeparam>
    /// <returns> The new <see cref="Component"/> that was created. </returns>
    public TComponent Add<TComponent>(string Name) where TComponent : Component, new() { TComponent component = new TComponent() { Name = Name }; Add(component); return component; }
    
    /// <summary> Adds a new <see cref="Component"/> of type <see cref="TComponent"/> with a priority of <see cref="Priority"/>. </summary>
    /// <param name="Priority"> The priority of the new <see cref="Component"/>. </param>
    /// <typeparam name="TComponent"> The type of the <see cref="Component"/>. </typeparam>
    /// <returns> The new <see cref="Component"/> that was created. </returns>
    public TComponent Add<TComponent>(int Priority) where TComponent : Component, new() { TComponent component = new TComponent() { Priority = Priority }; Add(component); return component; }
    
    /// <summary> Adds a new <see cref="Component"/> of type <see cref="TComponent"/> with a priority of <see cref="Priority"/> and a name of <see cref="Name"/>. </summary>
    /// <param name="Name"> The name of the <see cref="Component"/>. </param>
    /// <param name="Priority"> The priority of the new <see cref="Component"/> </param>
    /// <typeparam name="TComponent"> The type of <see cref="Component"/> to create. </typeparam>
    /// <returns> The new <see cref="Component"/> that was created. </returns>
    public TComponent Add<TComponent>(string Name, int Priority) where TComponent : Component, new() { TComponent component = new TComponent() {  Name = Name, Priority = Priority }; Add(component); return component; }



    /// <summary> Destroys all <see cref="Component">Components</see> in <see cref="Components"/>. </summary>
    /// <param name="Components"> The list of <see cref="Component">Components</see> to destroy. </param>
    public void Destroy(params IEnumerable<Component> Components) {
        foreach (Component component in Components) {
            component.TryEvent(Event.Destroy);
            component.ChainEvent(Event.Destroy);

            RemoveComponentFromLists(component);
            component.Parent = null;

            ComponentDestroyed?.Invoke(this, component);
        }
    }

    /// <summary> Destroys the first found <see cref="Component">Components</see> of type <see cref="TComponent"/>. </summary>
    /// <typeparam name="TComponent"> The type of <see cref="Component"/> to destroy. </typeparam>
    public void Destroy<TComponent>() where TComponent : Component, new() {
        TComponent? component = Get<TComponent>();

        if (component != null) {
            Destroy(component);
        }
    }

    /// <summary> Destroys the first found <see cref="Component">Components</see> that has all the given tags. </summary>
    /// <param name="Tags"> The tags to search for. </param>
    public void Destroy(params IEnumerable<string> Tags) { Component? component = Get(Tags); if (component != null) { Destroy(component); } }

    /// <summary> Destroys the first found <see cref="Component"/> of type <see cref="TComponent"/> with all the given tags. </summary>
    /// <param name="Tags"> The tags to search for. </param>
    /// <typeparam name="TComponent"> The type of component to search for. </typeparam>
    public void Destroy<TComponent>(params IEnumerable<string> Tags) where TComponent : Component, new() { Component? component = Get<TComponent>(Tags); if (component != null) Destroy(component); }


    /// <summary> Destroys all <see cref="Component">Components</see> owned by the <see cref="ComponentDocker"/>. </summary>
    public void DestroyAll() => Destroy(GetAll());
    
    /// <summary> Destroys all <see cref="Component">Components</see> of type <see cref="TComponent"/>. </summary>
    /// <typeparam name="TComponent"> The type of <see cref="Component"/> to destroy. </typeparam>
    public void DestroyAll<TComponent>() where TComponent : Component, new() => Destroy(GetAll<TComponent>());

    /// <summary> Destroys all <see cref="Component">Components</see> with all the given tags. </summary>
    /// <param name="Tags"> The tags to search for. </param>
    public void DestroyAll(params IEnumerable<string> Tags) => Destroy(GetAll(Tags));

    /// <summary> Destroys all <see cref="Component">Components</see> of type <see cref="TComponent"/> with the given tags. </summary>
    /// <param name="Tags"> The tags to search for. </param>
    /// <typeparam name="TComponent"> The type of <see cref="Component"/> to destroy. </typeparam>
    public void DestroyAll<TComponent>(params IEnumerable<string> Tags) where TComponent : Component, new() => Destroy(GetAll<TComponent>(Tags));



    /// <summary> Moves all the <see cref="Component">Components</see> in <see cref="Components"/> to the given <see cref="ComponentDocker"/>. </summary>
    /// <param name="ComponentDocker"> The docker to move the <see cref="Component">Components</see> to. </param>
    /// <param name="Components"> the <see cref="Component">Components</see> to move to the other <see cref="ComponentDocker"/> </param>
    public void Move(ComponentDocker ComponentDocker, params IEnumerable<Component> Components) {
        foreach (Component component in Components) {
            RemoveComponentFromLists(component);
            ComponentDocker.AddComponentToLists(component);

            component.Parent = ComponentDocker;

            ComponentMoved?.Invoke(this, ComponentDocker, component);
        }
    }

    /// <summary> Moves the first <see cref="Component"/> of type <see cref="TComponent"/>. </summary>
    /// <param name="ComponentDocker"> The <see cref="ComponentDocker"/> to move the <see cref="Component"/> to. </param>
    /// <typeparam name="TComponent"> The type of <see cref="Component"/> to search for. </typeparam>
    public void Move<TComponent>(ComponentDocker ComponentDocker) where TComponent : Component, new() { TComponent? component = Get<TComponent>(); if (component != null) Move(ComponentDocker, component); }

    /// <summary> Moves the first found <see cref="Component"/> with all the given tags, to the given <see cref="ComponentDocker"/>. </summary>
    /// <param name="ComponentDocker"> The <see cref="ComponentDocker"/> to move the <see cref="Component"/> to. </param>
    /// <param name="__tags"> The tags to search for. </param>
    public void Move(ComponentDocker ComponentDocker, params IEnumerable<string> __tags) { Component? component = Get(__tags); if(component != null) Move(ComponentDocker, component); }

    /// <summary> Moves the first found <see cref="Component"/> of type <see cref="TComponent"/> to with all the given tags, to the given <see cref="ComponentDocker"/>. </summary>
    /// <param name="ComponentDocker"> The <see cref="ComponentDocker"/> to move to the <see cref="Component"/> to. </param>
    /// <param name="__tags"> The tags to search for. </param>
    /// <typeparam name="TComponent"> The type of <see cref="Component"/>. </typeparam>
    public void Move<TComponent>(ComponentDocker ComponentDocker, params IEnumerable<string> __tags) where TComponent : Component, new() => Move(ComponentDocker, Get<TComponent>(__tags));
    
    
    
    /// <summary> Moves all <see cref="Component">Components</see> to the given <see cref="ComponentDocker"/>. </summary>
    /// <param name="ComponentDocker"> The <see cref="ComponentDocker"/> to move all <see cref="Component">Components</see> to. </param>
    public void MoveAll(ComponentDocker ComponentDocker) => Move(ComponentDocker, GetAll());
    
    /// <summary> Moves all <see cref="Component">Components</see> of type <see cref="TComponent"/> to the given <see cref="ComponentDocker"/>. </summary>
    /// <param name="ComponentDocker"> The <see cref="ComponentDocker"/> to move the <see cref="Component">Components</see> to. </param>
    /// <typeparam name="TComponent"> The type of <see cref="Component">Components</see> to move. </typeparam>
    public void MoveAll<TComponent>(ComponentDocker ComponentDocker) where TComponent : Component, new() => Move(ComponentDocker, GetAll<TComponent>());
    
    /// <summary> Moves all <see cref="Component">Components</see> with the given tags, to the <see cref="ComponentDocker"/>. </summary>
    /// <param name="ComponentDocker"> The <see cref="ComponentDocker"/> to move the <see cref="Component">Components</see> to. </param>
    /// <param name="Tags"> The tags to search for. </param>
    public void MoveAll(ComponentDocker ComponentDocker, params IEnumerable<string> Tags) => Move(ComponentDocker, GetAll(Tags));
    
    /// <summary> Moves all <see cref="Component">Components</see> of type <see cref="TComponent"/> to the <see cref="ComponentDocker"/>. </summary>
    /// <param name="ComponentDocker"> The <see cref="ComponentDocker"/> to move the <see cref="Component">Components</see> to. </param>
    /// <param name="Tags"> The tags to search for. </param>
    /// <typeparam name="TComponent"> The type of <see cref="Component"/> to search for. </typeparam>
    public void MoveAll<TComponent>(ComponentDocker ComponentDocker, params IEnumerable<string> Tags) where TComponent : Component, new() => Move(ComponentDocker, GetAll<TComponent>(Tags));
    
    
}