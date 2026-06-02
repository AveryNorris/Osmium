



namespace OsmiumRadium;


public class NestedRegion : Region, IBoundedElement, IBoundedElement<NestedRegion>, IColoredElement<NestedRegion>, IColoredElement
{
    
    //todo: be careful with many names, add a dispose or something maybe
    
    //todo: ADD a custom identifier for each region or something; so scrolling values are saved.
    
    //todo: make all element types have a backing type! like ibounded element and Iboundedelement<> 
    public Bounds _bounds { get; set; }
    
    public bool clipping = true;
    public bool scrolling = false;

    public bool coloring = false;

    //todo: make a permanent solution
    public static float scroll;

    public static float scrollCap = float.PositiveInfinity;

    private string name;

    public static Dictionary<string, float> ScrollingValues = [];

    public NestedRegion Coloring() {
        coloring = true;
        return this;
    }

    public NestedRegion Scrolling(bool __scrolling) {
        scrolling = __scrolling;
        return this;
    }

    public NestedRegion ScrollCap(int __scrollCap) {
        scrollCap = __scrollCap;

        return this;
    }

    public NestedRegion(string __name) {

        this.Size(100);
        
        name = __name;
        if (ScrollingValues.TryGetValue(name, out float value)) {
            scroll = value;
        } else {
            ScrollingValues.Add(name, scroll);
        }
    }

    protected internal override void Draw() {
        if (scrolling) {
            scroll -= Input.Scroll.y * 3;
            if (scroll < 0) {
                scroll = 0;
            }else if (scroll >= scrollCap) {
                scroll = scrollCap;
            }
            
            ScrollingValues[name] = scroll;
        }
        
        //todo: error for setting clipping inside of a region, nested regions? virtual regions

        Bounds ParentClipping = Backend.Clipping;

        for(int i = 0; i < _children.Count; i++) {
            Backend.UploadSubclippingUniform(ParentClipping, _bounds);
            
            ImmediateElement immediateElement = _children[i];
            
            if (scrolling) {
                if (immediateElement is IBoundedElement boundedElement) {
                    Bounds bounds = boundedElement._bounds;

                    bounds.pos = bounds.pos with { y = bounds.pos.y - scroll };

                    boundedElement._bounds = bounds;
                }
            }

            if (coloring)
            {
                if (immediateElement is IColoredElement coloredElement)
                {
                    float er = coloredElement._color.r / 255f;
                    float eg = coloredElement._color.g / 255f;
                    float eb = coloredElement._color.b / 255f;
                    float ea = coloredElement._color.a / 255f;

                    float mr = _color.r / 255f;
                    float mg = _color.g / 255f;
                    float mb = _color.b / 255f;
                    float ma = _color.a / 255f;
                    
                    coloredElement._color = Color.FromArgb(er * mr, eg * mg, eb * mb, ea * ma);
                }
                
                //todo: trash???
            }
            
            immediateElement.Draw();
        }
        
    }

    public Color _color { get; set; }
    public List<NestedRegion> Regions { get; set; } = [];
}