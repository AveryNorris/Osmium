
using OpenTK.Windowing.GraphicsLibraryFramework;
using OsmiumNucleus;


namespace OsmiumRadium;


/// <summary> Represents a 2D Rectangle on the screen </summary>
public struct Rect
{
    
    internal Rect(Vector2 __pos, Vector2 __size) {
        pos = __pos;
        size = __size;
    }
    

    
    public static Rect FromCorners(Vector2 a, Vector2 b) => new Rect { pos = Vector2.Min(a, b), size = Vector2.Max(a, b) - Vector2.Min(a, b) };
    
    public static Rect FromCorners(float ax, float ay, Vector2 b) => FromCorners(ax, ay, b.x, b.y);

    public static Rect FromCorners(Vector2 a, float bx, float by) => FromCorners(a.x, a.y, bx, by);

    public static Rect FromCorners(float ax, float ay, float bx, float by) => FromPosSize(float.Min(ax, bx), float.Min(ay, by), float.Abs(ax - bx), float.Abs(ay - by));


    public static Rect FromPosSize(Vector2 pos, Vector2 size) => new Rect { pos = pos, size = size };
    
    public static Rect FromPosSize(float px, float py, Vector2 size) => FromPosSize(new Vector2(px, py), size);
    
    public static Rect FromPosSize(Vector2 pos, float sx, float sy) => FromPosSize(pos, new Vector2(sx ,sy));

    public static Rect FromPosSize(float px, float py, float sx, float sy) => FromPosSize(new Vector2(px, py), new Vector2(sx, sy));


    public static Rect FromCenterSize(Vector2 center, Vector2 size) => new Rect { size = size, center = center };
    
    public static Rect FromCenterSize(float cx, float cy, Vector2 size) => FromCenterSize(new Vector2(cx, cy), size);
    
    public static Rect FromCenterSize(Vector2 center, float sx, float sy) => FromCenterSize(center, new Vector2(sx, sy));
    
    public static Rect FromCenterSize(float cx, float cy, float sx, float sy) => FromCenterSize(new Vector2(cx, cy), new Vector2(sx, sy));
    
    
    public static Rect FromSize(Vector2 size) => new Rect {size = size};

    public static Rect FromSize(float sx, float sy) => FromSize(new Vector2(sx, sy));



    public static Rect FullScreen => Rect.FromSize(100);



    public Vector2 pos;

    public Vector2 size
    {
        get => field;
        set
        {
            if (value.x < 0)
            {
                field.x = -value.x;
                pos.x -= field.x;
            }else field.x = value.x;

            if (value.y < 0)
            {   
                //todo: change how Rects are created, min and max cannot be individually set, theres no way to guarantee identical behavior with two way setting
                field.y = -value.y;
                pos.y -= field.y;
            }else field.y = value.y;
        }
    }

    public Vector2 min
    {
        get => pos;
        set {
            Vector2 oldMax = max;
            pos = value;
            max = oldMax;
        }
    }

    public Vector2 max
    {
        get => pos + size;
        set => size = value - pos;
    }

    public Vector2 center
    {
        get => (min + max) / 2;
        set => pos = value - size / 2;
    }

    public override string ToString() {
        return "<" + min.x + "," + min.y + "," + max.x + "," + max.y + ">";
    }

    public Rect Sub(Rect parent, Rect mask) {
        return FromCorners(Vector2.Max(parent.min, mask.min), Vector2.Min(parent.max, mask.max));
    }


    public bool MouseDown(MouseButton button) => 
        MouseInBounds() && Osmium.Context.MouseState.IsButtonPressed(button);
    
    public bool MouseUp(MouseButton button) => 
        MouseInBounds() && Osmium.Context.MouseState.IsButtonReleased(button);

    public bool MouseHeld(MouseButton button) => 
        MouseInBounds() && Osmium.Context.MouseState.IsButtonDown(button);

    public bool MouseInBounds() =>
        Input.MousePos.x >= min.x && Input.MousePos.y >= min.y &&
        Input.MousePos.x <= max.x && Input.MousePos.y <= max.y
        && ((Input.MousePos.x >= Backend.Clipping.min.x && Input.MousePos.y >= Backend.Clipping.min.y &&
             Input.MousePos.x <= Backend.Clipping.max.x && Input.MousePos.y <= Backend.Clipping.max.y) || 
            (Input.MousePos.x >= RegionState.CurrentRegionRect.min.x && Input.MousePos.y >= RegionState.CurrentRegionRect.min.y &&
             Input.MousePos.x <= RegionState.CurrentRegionRect.max.x && Input.MousePos.y <= RegionState.CurrentRegionRect.max.y));






}