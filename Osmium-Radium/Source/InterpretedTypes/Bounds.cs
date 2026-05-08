using System.Numerics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OsmiumNucleus;


namespace OsmiumRadium;


public struct Bounds
{



    public Bounds(Vector2? size = null, Vector2? pos = null, Vector2? center = null, Vector2? min = null, Vector2? max = null) {

        if (size != null) {
            this.size = (Vector2) size;
            
            if (min != null || max != null) 
                Debug.Error("Setting _size before setting min or max is redundant; both settings override the previously defined _size. Only set one of the two!");
        }
        
        if (pos != null) {
            this.pos = (Vector2) pos; 
            
            if (min != null)
                Debug.Error("Setting pos before setting min is redundant; the setting overrides the previously defined pos. Only set one of the two!");
            
            if (center != null)
                Debug.Error("Setting pos before setting center redundant; setting center overrides the previously defined pos. Only set one of the two!");
        }

        if (max != null) this.max = (Vector2) max;
        
        if (min != null) {
            this.min = (Vector2) min;
            
            if (center != null)
                Debug.Error("Setting min before setting center is redundant; the setting overrides the previously defined min. Only set one of the two!");
        }
        
        if (center != null) this.center = (Vector2) center;
        
    }
    
    
    
    /// <summary> Position of the element from the minimum corner </summary>
    public Vector2 pos { get; set; }
    
    /// <summary> Center of the element </summary>
    public Vector2 center { get => pos + size / 2; set => pos = value - size / 2; }
    
    
    
    /// <summary> Minimum corner of the element, element can resize when you change this. </summary>
    public Vector2 min { get => pos;
        set {
            Vector2 oldMax = max;
            pos = value;
            max = oldMax;
            //ConstrainMinimumsAndMaximums();
        }
    }
    
    /// <summary> Maximum corner of the element, element can resize when you change this. </summary>
    public Vector2 max { 
        get => pos + size;
        set {
            size = value - pos; 
            //ConstrainMinimumsAndMaximums();
        }
    }
    
    //todo: fix min max constraints and guard at draw time
    
    
    
    /// <summary> Size of the element</summary>
    public Vector2 size { get; set; }



    /// <summary> Makes sure that both the minimum and maximum vectors are truly max and min, swaps them accordingly if not. </summary>
    private void ConstrainMinimumsAndMaximums() {

        Vector2 newMin = min;
        Vector2 newMax = max;
        
        bool swapped = false;
        
        if(min.X > max.X) {
            swapped = true;
            newMin.X = max.X;
            newMax.X = min.X;
        }
        
        if(min.Y > max.Y) {
            swapped = true;
            newMin.Y = max.Y;
            newMax.Y = min.Y;
        }

        if (swapped) {
            min = newMin;
            max = newMax;

            Debug.Error("Min and Max were incorrect!");
        }
    }







}