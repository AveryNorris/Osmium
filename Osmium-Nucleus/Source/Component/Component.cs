namespace OsmiumNucleus;


/// <summary> The backing class for all Components in Osmium. Inherit this to become one. </summary>
/// <author> Avery Norris </author>
#nullable enable
public abstract partial class Component : ComponentDocker
{
    


    /// <summary> Current parent of the Component. Can be either a Scene or another Component.</summary>
    [MarkerAttributes.UnsafeInternal] public ComponentDocker Parent { get; internal set; }
    
    
    #region Component Information
    
    /// <summary> The Component's name </summary>
    public string Name {
        get => _name;
        set { 
            if (value == null) { Debug.LogError("A Component's name cannot be null!"); return; } 
            
            _name = value; 
        }
    } [MarkerAttributes.UnsafeInternal] private string _name = String.Empty;

    
    
    /// <summary> If the component receives time events or not. </summary>
    [MarkerAttributes.CalculatedProperty, MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.O1)]
    public bool Enabled {
        get => OrderProfile >= 0;
        set {
            if (Enabled != value) OrderProfile *= -1;
        }
    }
    
    
    
    /// <summary> Represents the Component's Update priority; higher priorities get updated first. Can be any integer in the range -63 -> 63. Easy to calculate, but when set, it must resort all Components. </summary>
    [MarkerAttributes.CalculatedProperty, MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.High), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public int Priority {
        get => Math.Abs(OrderProfile) - 64;
        set {
            if(value is < -63 or > 63) { Debug.LogError("Priority cannot be set to any integer larger than 63 or less than -64!"); return; }

            Parent.UpdatePriority(this, Priority, value);
            OrderProfile = (sbyte) (Enabled ? value + 64 : (value + 64) * -1);
        }
    }
    
    
    
    /// <summary> Represents the state of this Component, if it is negative then it is a disabled component, and the abs of the value represents the update priority of the Component </summary>
    [MarkerAttributes.UnsafeInternal] private sbyte OrderProfile = 64;
    
    

    #endregion Tags
    
    #region Tags
    
    /// <summary> A list of all tags belonging to the component. Use AddTag() to modify it. Easy to calculate, but setting this requires that the Docker resorts Components; </summary>
    [MarkerAttributes.CalculatedProperty, MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.Medium), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public IReadOnlySet<string> Tags {
        get => _tags;
        set {
            HashSet<string> previousTags = _tags;
            
            foreach (string tag in _tags.ToArray()) if(!value.Contains(tag)) RemoveTag(tag);
            foreach (string tag in value) if(!previousTags.Contains(tag)) AddTag(tag);
        }
    } [MarkerAttributes.UnsafeInternal] internal HashSet<string> _tags = [];
    
    

    /// <summary> Adds a new tag to the component</summary>
    [MarkerAttributes.CalculatedProperty, MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.Low), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public void AddTag(string __tag) {
        if(__tag == null) { Debug.LogError("A Component's tag cannot be null!"); return; }
        if(Tags.Contains(__tag)) { Debug.LogError("Component already has the given tag!", ["ComponentName", "Tag"], [_name, __tag]); return; }

        _tags.Add(__tag);
        Parent.HashTaggedComponent(__tag, this);
    }
    
    
    
    /// <summary> Removes a tag from the component.</summary>
    [MarkerAttributes.CalculatedProperty, MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.Low), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public void RemoveTag(string __tag) {
        if (__tag == null) { Debug.LogError("Cannot remove a null tag! It is impossible for Components to have null tags"); return;};
        if(!Tags.Contains(__tag)) { Debug.LogError("Component does not have the given tag!", ["ComponentName", "Tag"], [_name, __tag]); return; }

        _tags.Remove(__tag);
        Parent.UnhashTaggedComponent(__tag, this);
    }
    
    #endregion Tags
    
    
    
    /// <summary> Attempts to send an event to the component, and quietly exits if not.</summary>
    [MarkerAttributes.UnsafeInternal]
    internal void TryEvent(int __timeEvent) {
        if (!Enabled) return;

        Type type = GetType();
        if (EventManager._TypeAssociatedVirtualEventPrivileges[type] || Osmium.IsRunning) {
            EventManager._TypeAssociatedTimeEvents[type][__timeEvent]?.Invoke(this);
        }
    }
    

    /// <summary> Just gives the Component's name. But makes debugging a little easier :).</summary>
    public override string ToString() => this.Name;
    
}