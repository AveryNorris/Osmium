namespace OsmiumNucleus;


//You'll notice some methods do NOT check validity. To prevent redundancy, the rule is anything that just uses another base Osmium method with the SAME parameters
//doesn't need to check since it is assumed that one will.
#nullable enable
public abstract partial class ComponentDocker
{


    /// <summary> Tells you whether the docker contains a component with the given tag </summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.O1)]
    public bool Contains(string __tag) {
        if(__tag == null) { Debug.LogError("A given tag cannot be null!"); return false; }
        return _componentTagDictionary.ContainsKey(__tag);
    }
    
    /// <summary> Tells you whether the docker contains a component with the given type</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.O1)]
    public bool Contains<__Type>() where __Type : Component => _componentTypeDictionary.ContainsKey(typeof(__Type));

    /// <summary> Tells you whether the docker contains a component with a given type and tag</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public bool Contains<__Type>(string __tag) where __Type : Component => Get<__Type>(__tag) != null;

    /// <summary> Tells you whether the docker contains a component with all the given tags </summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow),
     MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public bool Contains(ICollection<string> __tags) => Get(__tags) != null;

    /// <summary> Tells you whether the docker contains a component with all the given tags and the type</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public bool Contains<__Type>(ICollection<string> __tags) where __Type : Component => Get<__Type>(__tags) != null;

    /// <summary> Tells you whether the docker contains the given component.</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.O1)]
    public bool Contains(Component __component) {
        if(__component == null) { Debug.LogError("A given Component cannot be null!"); return false; }
        
        return _componentTypeDictionary.TryGetValue(__component.GetType(), out HashSet<Component>? components) && components.Contains(__component);
    }

    /// <summary> Tells you whether the docker contains the all the given components.</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.Medium), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public bool ContainsAll(IEnumerable<Component> __components) => __components.All(x => _components.Contains(x));
    


    /// <summary> Gets all components of a given type</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.O1)] 
    public ICollection<Component> GetAll() => [.._components];

    /// <summary> Finds the first component with a given Type</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.O1)]
    public __Type? Get<__Type>() where __Type : Component => _componentTypeDictionary.TryGetValue(typeof(__Type), out HashSet<Component>? Components) ? Components.FirstOrDefault() as __Type : null;

    /// <summary> Tries to find the first component with a given tag, and returns false if there is none</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.O1)]
    public bool TryGet<__Type>(out __Type? __component) where __Type : Component { __component = Get<__Type>(); return __component != null; }



    /// <summary> Gets all components of a given type</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.O1)]
    public ICollection<__Type> GetAll<__Type>() where __Type : Component => _componentTypeDictionary.TryGetValue(typeof(__Type), out HashSet<Component>? components) ? components.OfType<__Type>().ToList() : [];
    /// <summary> Tries to get all components of a given type and returns false if there are none</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.O1)]
    public bool TryGetAll<__Type>(out ICollection<__Type> __components) where __Type : Component { __components = GetAll<__Type>(); return __components.Any(); }
    
    
    
    /// <summary> Finds all components that have all the given tags</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public ICollection<Component> GetAll(ICollection<string> __tags) {
        if (__tags == null) { Debug.LogError("A given collection cannot be null!"); return []; }
        if(__tags.Contains(null)) { Debug.LogError("A given collection cannot contain null!"); return []; }
        if(__tags.Count == 0) return GetAll(); 
        
        HashSet<Component> components;
        if (_componentTagDictionary.TryGetValue(__tags.First(), out HashSet<Component>? firstComponents)) components = firstComponents.ToHashSet(); else return [];
        
        foreach(string tag in __tags)
            if (_componentTagDictionary.TryGetValue(tag, out HashSet<Component>? taggedComponents))
                components.RemoveWhere(x => !taggedComponents.Contains(x));

        return components.ToList();
    }

    /// <summary> Tries to find all components that have all the given tags, returns false if there are none</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow),
     MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public bool TryGetAll(ICollection<string> __tags, out ICollection<Component> __components) {
        __components = GetAll(__tags); return __components.Any();
    }
    
    
    
    
    
    /// <summary> Finds all Components that have the given type, and all the given tags</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public ICollection<__Type> GetAll<__Type>(ICollection<string> __tags) where __Type : Component {
        if (__tags == null) { Debug.LogError("A given collection cannot be null!"); return []; }
        if(__tags.Contains(null)) { Debug.LogError("A given collection cannot contain null!"); return []; }
        if (__tags.Count == 0) return GetAll<__Type>();
        
        HashSet<__Type> components = [];

        if (_componentTagDictionary.TryGetValue(__tags.First(), out HashSet<Component>? firstComponents)) 
            foreach (Component component in firstComponents) if (component is __Type typedComponent) components.Add(typedComponent);
        
        foreach(string tag in __tags)
            if (_componentTagDictionary.TryGetValue(tag, out HashSet<Component>? taggedComponents)) 
                components.RemoveWhere(x => !taggedComponents.Contains(x));

        return components.ToList();
    }

    /// <summary> Tries to find all the components that have the given tags and type, returns false if there are none</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow),
     MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public bool TryGetAll<__Type>(ICollection<string> __tags, out ICollection<__Type> __components) where __Type : Component {
        __components = GetAll<__Type>(__tags); return __components.Any();
    }





    /// <summary> Gets all the components with the given tag</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow),
     MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.O1)]
    public ICollection<Component> GetAll(string __tag) {
        if(__tag == null) { Debug.LogError("A given tag cannot be null!"); return []; }
        
        return _componentTagDictionary.TryGetValue(__tag, out HashSet<Component>? components) ? components.ToList() : Array.Empty<Component>();
    }

    /// <summary> Tries to get all the components with the given tag, returns false if there are none</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.O1)]
    public bool TryGetAll(string __tag, out ICollection<Component> __components) {
        __components = GetAll(__tag); return __components.Any();
    }
    
    
    
    /// <summary> Gets all the components that have a certain type, and a certain tag</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public ICollection<__Type> GetAll<__Type>(string __tag) where __Type : Component => GetAll<__Type>([__tag]);

    /// <summary> Tries to get all the components with a certain tag, and a type. Returns false if there are none</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public bool TryGetAll<__Type>(string __tag, out ICollection<__Type> __components) where __Type : Component {
        __components = GetAll<__Type>(__tag); return __components.Any();
    }

    
    
    /// <summary> Gets the first component with all the given tags</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public Component? Get(ICollection<string> __tags) => GetAll(__tags).FirstOrDefault();

    /// <summary> Tries to get the first component with all the given tags. Returns false if there are none</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public bool TryGet(ICollection<string> __tags, out Component? __component) {
        __component = Get(__tags); return __component != null;
    }
    
    

    /// <summary> Finds the first component with the given tag</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.O1)]
    public Component? Get(string __tag) => GetAll([__tag]).FirstOrDefault();

    /// <summary> Tries to find the first component with the given tag, returns false if there is none</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.O1)]
    public bool TryGet(string __tag, out Component? __component) {
        __component = Get(__tag); return __component != null;
    }


    
    /// <summary> Gets the first component with the given type and tag</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public __Type? Get<__Type>(string __tag) where __Type : Component => GetAll<__Type>(__tag).FirstOrDefault();

    /// <summary> Tries to get the first component with the given type and tag, returns false if there is none.</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public bool TryGet<__Type>(string __tag, out __Type? __component) where __Type : Component {
        __component = Get<__Type>(__tag); return __component != null;
    }
    
    
    
    /// <summary> Gets the first component with the given type and tag</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public __Type? Get<__Type>(ICollection<string> __tags) where __Type : Component => GetAll<__Type>(__tags).FirstOrDefault();

    /// <summary> Tries to get the first component with the given type and tag, returns false if there is none.</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public bool TryGet<__Type>(ICollection<string> __tags, out __Type? __component) where __Type : Component {
        __component = Get<__Type>(__tags); return __component != null;
    }
    
    
    
}