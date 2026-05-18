using OsmiumNucleus;



namespace OsmiumRadium;



/// <summary> Represents a retained element which exists for many frames, and is meant to draw immediate elements </summary>
public abstract class RetainedElement : Element
{


    
    /// <summary> Resets the retained element to work for the next frame </summary>
    protected internal void ResetState() {
        if (RegionState.RegionScope.Count > 0) {
            Debug.Error("Regions are unbalanced! Did you forget an Exit statement?");
        }
        
        RegionState.RegionScope = [];
    }

    
    
    /// <summary> Radium calls this whenever the game updates </summary>
    protected internal virtual void Update() {}
    
    
    
    /// <summary> Radium calls this whenever the game begins to draw </summary>
    protected internal virtual void Draw() {}
    
    
    
    protected Box Box() {
        Box returnValue = new Box();
        IntroduceElement(returnValue);  
        return returnValue;
    }
    
    protected TextBox TextBox() {
        TextBox returnValue = new TextBox();
        IntroduceElement(returnValue);
        return returnValue;
    }

    protected Button Button() {
        Button returnValue = new Button();
        IntroduceElement(returnValue);
        return returnValue;
    }

    protected NestedRegion Region(string name) {
        NestedRegion returnValue = new NestedRegion(name);
        IntroduceElement(returnValue);
        
        RegionState.Focus(returnValue);
        return returnValue;
    }

    protected void Exit() => RegionState.Exit();

    private static void IntroduceElement(ImmediateElement __element) {
        RegionState.Current.Add(__element);

        Backend.elementCount++;
    }
}