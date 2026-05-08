
using System.Numerics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OsmiumNucleus;
using OsmiumRadium;
//using OsmiumRadium.Source.Interfaces;


namespace OsmiumRadium;

public abstract partial class RadiumElement
{

    protected internal virtual void Update() {}
    
    protected internal virtual void Draw() {}

    
    
    protected internal const float r169 = 16f / 9f;
    
    internal List<NestedRegion> Regions = [];
    
    
    
    protected Box Box() {
        //todo: change to official palette system rework
        Box returnValue = new Box();
        IntroduceElement(returnValue);  
        return returnValue;
    }
    
    public TextBox TextBox() {
        TextBox returnValue = new TextBox();
        IntroduceElement(returnValue);
        return returnValue;
    }

    public Button Button() {
        Button returnValue = new Button();
        IntroduceElement(returnValue);
        return returnValue;
    }

    public NestedRegion Region(string name) {
        NestedRegion returnValue = new NestedRegion(name);
        IntroduceElement(returnValue);
        Regions.Add(returnValue);
        return returnValue;
    }

    public void Exit() {
        if (Regions.Count == 0) {
            Debug.Error("No region to exit! You are already in the base region.");
        } else {
            Regions.RemoveAt(Regions.Count - 1);
        }
    }

    public void IntroduceElement(IElement iElement) {
        if (Regions.Count > 0) {
            Regions[^1].Add(iElement);
        } else {
            Backend.BaseRegion.Add(iElement);
        }

        Backend.elementCount++;
    }
    
    public static Vector2 BoundsMin { get; private set; }
    public static Vector2 BoundsMax { get; private set; }
    
        
    /// <summary> Sets the clipping bounds of follow elements when they are drawn. Do not use this while immediate elements are being drawn. </summary>
    public void Clip(Vector2 __min, Vector2 __max) {
        BoundsMin = __min;
        BoundsMax = __max;
        
        //todo: current region property
        if (Regions.Count > 0) {
            Regions[^1].Clip(__min, __max);
        } else {
            Debug.Error("Cannot modify clipping in an unnested region!");
        }
    }
    
    public void ResetClippingRect() {
        Clip(Vector2.Zero, new Vector2(100));
    }

    public static RadiumElement Add<T>() where T : RadiumElement, new() => Radium.Add<T>();

    public static void RemoveElement(RadiumElement __element) => Radium.RemoveElement(__element);
    
    public static void Remove<T>() where T : RadiumElement, new() => Radium.Remove<T>();
}