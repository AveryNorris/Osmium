using System.Drawing;
using System.Numerics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OsmiumRadium;

public class BoxText : ImGUI
{
    
    
    public Box box;
    
    public Text text;

    public Color backgroundColor;
    
    
    
    public BoxText(Bounds bounds = new Bounds(), Text? text = null, Color? backgroundColor = null, Color? backgroundHoverColor = null, Color? backgroundHeldColor = null, int z = 0) {

        Bounds boxBounds = bounds;

        this.text = text ?? new Text();
        this.text.center = boxBounds.center;
        
        this.box = new Box(boxBounds);
        this.box.z = z;
        
        //puts the _text on top of the Z stack, _text is instantiated in the constructor so it will always be behind the box unless you reset it.
        this.text.z = z;
        
        this.backgroundColor = backgroundColor ?? Box.DefaultColor;
    }
    
    
    
    public bool Active() => Backend.MouseUp(box.Bounds.min, box.Bounds.max, MouseButton.Left);
    
    public bool Held() => Backend.MouseHeld(box.Bounds.min, box.Bounds.max, MouseButton.Left);
    
    public bool Hovered() => Backend.MouseInBounds(box.Bounds.min, box.Bounds.max);
    
    
    
    protected internal override void Draw()
    {
        box.color = backgroundColor;
    }
}