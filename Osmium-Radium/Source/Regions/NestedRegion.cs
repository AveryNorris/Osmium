



using OsmiumNucleus;

namespace OsmiumRadium;



/// <summary> Represents a region of elements that is nested within the stable region </summary>
public class NestedRegion(Region parent) : Region, IBoundedElement
{
    

    /// <inheritdoc cref="IBoundedElement.Rect"/>
    public Rect Rect { get; set; } = Rect.FullScreen;



    /// <summary> The value of the scroll, equal to -1 if there is no scroll </summary>
    private float scroll = -1;
    
    
    /// <summary> Lets the user scroll through everything in the region </summary>
    /// <param name="__scroll"> The value of the scroll, to use scrolling regions you must save this and insert it back in whenever you use scrollable</param>
    /// <param name="__scrollCap"> The maximum size the scroll can go down </param>
    public void Scrollable(ref float __scroll, float __scrollCap) {
        __scroll -= Input.Scroll.y;
        
        float trueScrollCap = __scrollCap - Rect.size.y;
        if (__scroll > trueScrollCap) __scroll = trueScrollCap;
        if (__scroll < 0) __scroll = 0;
        
        scroll = __scroll;
    }
    
    
    
    protected internal override void Draw() {

        foreach (ImmediateElement element in _children) {

            if (!float.IsNaN(_depth) && element._overrideDepth)
                element._depth = _depth;

            if (scroll > 0 && element is IBoundedElement bounded) {
                if (element is NestedRegion region) region.Scrollable(ref scroll, float.PositiveInfinity);
                else {
                    bounded.Rect = bounded.Rect with {
                        pos = new Vector2(bounded.Rect.pos.x, bounded.Rect.pos.y - scroll)
                    };
                }
            }
        }
        
        base.Draw();
    }

    
    //todo: make sure these are immutable at immediate time, and make that documented
    //todo: ALWAYS think about Immediate vs Retained time when debugging, 9 times / 10 it is the issue 

    
    
    protected internal override void SetClipping() {
        parent.SetClipping();
        Backend.UploadSubclippingUniform( Rect);
    }
}