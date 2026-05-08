using System.Numerics;
using OsmiumNucleus;


namespace OsmiumRadium;


public interface IRegion : IList<IElement>;


public class StableRegion : List<IElement>, IRegion
{
    public void Draw() {
        Backend.UploadClippingUniform(new Vector4(0,0,100,100));
        
        for (int i = 0; i < Count; i++) {
            this[i].Draw();
        }
    }
}

public class NestedRegion : List<IElement>, IElement, IBoundedElement, IBoundedElement<NestedRegion>
{
    
    
    
    public static Dictionary<int, Vector4> ClippingRects = [];
    
    
    
    public void Clip(Bounds __bounds) => Clip(__bounds.min, __bounds.max);
    
    public void Clip(Vector2 __min, Vector2 __max) {
        if(Backend.DrawingImmediate) { Debug.Error("You cannot set clipping while immediate elements are being drawn. Use Backend.UploadClippingUniform for temporary values!"); return; }
        
        Vector4 value = new Vector4(__min.X, __min.Y, __max.X, __max.Y);
        int index = Backend.elementCount;

        if (!ClippingRects.TryAdd(index, value)) {
            ClippingRects[index] = value;
        }
    }
    
    //todo: be careful with many names, add a dispose or something maybe
    
    //todo: ADD a custom identifier for each region or something; so scrolling values are saved.
    
    //todo: make all element types have a backing type! like ibounded element and Iboundedelement<> 
    public Bounds _bounds { get; set; }
    
    public bool clipping = true;
    public bool scrolling = false;

    //todo: make a permanent solution
    public static float scroll;

    public static float scrollCap = float.PositiveInfinity;

    private string name;

    public static Dictionary<string, float> ScrollingValues = [];

    public NestedRegion Scrolling(bool __scrolling) {
        scrolling = __scrolling;
        return this;
    }

    public NestedRegion ScrollCap(int __scrollCap) {
        scrollCap = __scrollCap;

        return this;
    }

    public NestedRegion(string __name) {
        name = __name;
        if (ScrollingValues.TryGetValue(name, out float value)) {
            scroll = value;
        } else {
            ScrollingValues.Add(name, scroll);
        }
    }

    public void Draw() {
        Backend.UploadSubclippingUniform(new Vector4(_bounds.min.X, _bounds.min.Y, _bounds.max.X, _bounds.max.Y));

        if (scrolling) {
            scroll -= Backend.ScrollDeltaY * 3;
            if (scroll < 0) {
                scroll = 0;
            }else if (scroll >= scrollCap) {
                scroll = scrollCap;
            }
            
            ScrollingValues[name] = scroll;
        }
        
        //todo: error for setting clipping inside of a region, nested regions? virtual regions

        for(int i = 0; i < Count; i++) {
            IElement element = this[i];
            
            if (scrolling) {
                if (element is IBoundedElement boundedElement) {
                    Bounds bounds = boundedElement._bounds;

                    bounds.pos = bounds.pos with { Y = bounds.pos.Y - scroll };

                    boundedElement._bounds = bounds;
                }
            }

            if (ClippingRects.TryGetValue(i, out Vector4 value)) {
                Backend.UploadClippingUniform(value);
            }
            
            element.Draw();
        }
        
        Backend.RevertSubclippingBounds();
    }
}