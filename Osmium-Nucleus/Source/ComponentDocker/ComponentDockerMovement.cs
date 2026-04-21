namespace OsmiumNucleus;


public abstract partial class ComponentDocker
{

    
    
    /// <summary> Moves a component belonging to the docker to another docker</summary>
    public void Move(Component __component, ComponentDocker __componentDocker) {
        if(__component == null) { Debug.LogError("Component cannot be null!"); return; }
        if(__componentDocker == null) { Debug.LogError("Docker cannot be null!"); return; }
        if(__componentDocker == this) { Debug.LogError("A Component cannot move to a Docker it already belongs to!"); return; }
        if(!this.Contains(__component)) { Debug.LogError("The Docker you are calling does not own this Component!"); return; }
        if(__component == __componentDocker)  { Debug.LogError("You cannot move a Component into itself!"); return; }
        if(__component.AllChildren.Contains(__componentDocker)) { Debug.LogError("The Component is a parent of the Docker it is trying to move to!"); return; }
        
        RemoveComponentFromLists(__component);
        __componentDocker.AddComponentToLists(__component);
        
        __component.Parent = __componentDocker;
    }
        

    /// <summary> Moves all components in a list to another docker</summary>
    public void MoveAll(IEnumerable<Component> __Components, ComponentDocker __componentDocker) {
        if(__Components == null) { Debug.LogError("Components cannot be null!"); return; }
        if(__componentDocker == null) { Debug.LogError("Docker cannot be null!"); return; }
        if(__componentDocker == this) { Debug.LogError("A Component cannot move to a Docker it already belongs to!"); return; }
        
        foreach (Component component in __Components) {
            Move(component, __componentDocker);
        }
    }
    
    /// <summary> Moves all components</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.O1)] 
    public void MoveAll(ComponentDocker __componentDocker) => MoveAll(GetAll(), __componentDocker);

    /// <summary> Moves the first component with a given Type</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.O1)]
    public void Move<__Type>(ComponentDocker __componentDocker) where __Type : Component => Move(Get<__Type>(), __componentDocker);



    /// <summary> Moves all components of a given type</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.Low), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public void MoveAll<__Type>(ComponentDocker __componentDocker) where __Type : Component => MoveAll((ICollection<Component>) GetAll<__Type>(), __componentDocker);



    /// <summary> Moves all components that have all the given tags</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public void MoveAll(ICollection<string> __tags, ComponentDocker __componentDocker) => MoveAll(GetAll(__tags), __componentDocker);
    



    /// <summary> Moves all Components that have the given type, and all the given tags</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public void MoveAll<__Type>(ICollection<string> __tags, ComponentDocker __componentDocker) where __Type : Component => MoveAll((ICollection<Component>) GetAll<__Type>(__tags), __componentDocker);
    
    
    
    
    /// <summary> Moves all the components with the given tag</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.O1)]
    public void MoveAll(string __tag, ComponentDocker __componentDocker) => MoveAll(GetAll([__tag]), __componentDocker);
    
    
    
    /// <summary> Moves all the components that have a certain type, and a certain tag</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public void MoveAll<__Type>(string __tag, ComponentDocker __componentDocker) where __Type : Component => MoveAll((ICollection<Component>) GetAll<__Type>([__tag]), __componentDocker);
    
    

    /// <summary> Moves the first component with the given tag</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.O1)]
    public void Move(string __tag, ComponentDocker __componentDocker) => Move(Get(__tag), __componentDocker);


    
    /// <summary> Moves the component with the given type and tag</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public void Move<__Type>(string __tag, ComponentDocker __componentDocker) where __Type : Component => Move(Get<__Type>(__tag), __componentDocker);
    
    
}