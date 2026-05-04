
using System.Numerics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OsmiumNucleus;
using OsmiumRadium;
using OsmiumRadium.Source.Interfaces;


namespace OsmiumRadium;

public abstract partial class RadiumElement
{

    protected internal virtual void Update() {}
    
    protected internal virtual void Draw() {}

    
    
    protected internal const float r169 = 16f / 9f;
    
    
    
    protected Box Box() {
        //todo: change to official palette system rework
        Box returnValue = new Box();
        returnValue.Introduce();
        return returnValue;
    }
    
    public Text TextBox() {
        Text returnValue = new Text();
        returnValue.Introduce();
        return returnValue;
    }

    public Button Button() {
        Button returnValue = new Button();
        returnValue.Introduce();
        return returnValue;
    }
    
    public static void SetClippingRect(Vector2 __min, Vector2 __max) {
        Vector4 value = new Vector4(__min.X, __min.Y, __max.X, __max.Y);
        int index = Backend.IMGUIElements.Count - 1;
        
        if (!Backend.ClippingRects.TryAdd(index, value)) {
            Backend.ClippingRects[index] = value;
        }
    }
    
    public static void ResetClippingRect() {
        SetClippingRect(Vector2.Zero, new Vector2(100));
    }
}