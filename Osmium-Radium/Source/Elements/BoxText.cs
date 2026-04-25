using System.Drawing;
using System.Numerics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OsmiumRadium;

public class BoxText : ImGUI
{
    
    
    public Box box;
    
    public Text text;

    public Color backgroundColor;
    
    
    
    public BoxText(Transform transform = new Transform(), Text? text = null, Color? backgroundColor = null, Color? backgroundHoverColor = null, Color? backgroundHeldColor = null, int z = 0) {

        Transform boxTransform = transform;

        this.text = text ?? new Text();
        this.text.center = boxTransform.center;
        
        this.box = new Box(boxTransform);
        this.box.z = z;
        
        //puts the text on top of the Z stack, text is instantiated in the constructor so it will always be behind the box unless you reset it.
        this.text.z = z;
        
        this.backgroundColor = backgroundColor ?? Box.DefaultColor;
    }
    
    
    
    public bool Active() => Backend.MouseUp(box.transform.min, box.transform.max, MouseButton.Left);
    
    public bool Held() => Backend.MouseHeld(box.transform.min, box.transform.max, MouseButton.Left);
    
    public bool Hovered() => Backend.MouseInBounds(box.transform.min, box.transform.max);
    
    
    
    protected internal override void Draw()
    {
        box.color = backgroundColor;
    }
}