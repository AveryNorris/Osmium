namespace OsmiumNucleus;


#nullable enable
public abstract partial class Component
{
    
    
    
    /// <summary> Scene the Component resides in. To save ram it's not stored but calculated on the fly. </summary>
    [MarkerAttributes.CalculatedProperty, MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.Medium), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public Scene? Scene => __QueryScene();
    private Scene? __QueryScene() {
        if (Parent is Scene scene) return scene;
        if (Parent is Component Component) return Component.__QueryScene();
        
        Debug.LogError("Failed To Find Scene? Osmium does not know what is going on but you beefed up.");
        
        return null;
    }
    
    
    
    /// <summary> Returns the Parent Component. Will be null if the Component's parent is not a Component. </summary>
    [MarkerAttributes.CalculatedProperty, MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.O1)]
    public Component? ComponentParent => __QueryComponentParent();
    private Component? __QueryComponentParent() {
        if (Parent is Component _component)
            return _component;
        return null;
    }
    
    
    
    /// <summary> Returns the Parent Scene. Will be null if the Component's parent is not a Scene. </summary>
    [MarkerAttributes.CalculatedProperty, MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.O1)]
    public Scene? SceneParent => __QuerySceneParent();
    private Scene? __QuerySceneParent() {
        if (Parent is Scene _scene)
            return _scene;
        return null;
    }
    
    
    
    /// <summary> All parent Components and the parents of the parents up until the Scene. Will only list parents of parents, not uncle Components. And will not list the Scene </summary>
    [MarkerAttributes.CalculatedProperty, MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.Medium), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public IReadOnlyList<Component> AllParents => __QueryComponents();
    private IReadOnlyList<Component> __QueryComponents() {
        List<Component> returnValue = [];
        ComponentDocker currentComponentDocker = Parent;

        while (true) {
            if (currentComponentDocker is Component Component) {
                returnValue.Add(Component);
                currentComponentDocker = Component.Parent;
            } else {
                break;
            }
        }

        return returnValue;
    }

    
}