using System.Drawing;
using System.Numerics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OsmiumRadium;

public class Button : GUIElement
{
    
    
    
    public Palette backgroundColor;
    public static Palette DefaultBackgroundColor = Palette.Unknown;
    
    public Palette backgroundHoverColor;
    public static Palette DefaultBackgroundHoverColor = Palette.Unknown;

    public Palette backgroundHeldColor;
    public static Palette DefaultBackgroundHeldColor = Palette.Unknown;
    
    public Box box;
    
    public Text text;
    
    
    
    public Button(Transform? transform = null, Text? text = null, Palette? backgroundColor = null, Palette? backgroundHoverColor = null, Palette? backgroundHeldColor = null, int z = 0) {

        Transform buttonTransform = transform ?? new Transform();

        this.text = text ?? new Text();
        this.text.center = buttonTransform.center;
        
        this.box = new Box(buttonTransform);
        
        //puts the text on top of the Z stack, text is instantiated in the constructor so it will always be behind the box unless you reset it.
        this.text.z = z;
        
        this.backgroundColor = backgroundColor ?? DefaultBackgroundColor;
        this.backgroundHoverColor = backgroundHoverColor ?? DefaultBackgroundHoverColor;
        this.backgroundHeldColor = backgroundHeldColor ?? DefaultBackgroundHeldColor;
        
    }
    
    
    
    public bool Pressed() => Backend.MouseUp(box.transform.min, box.transform.max, MouseButton.Left);
    
    public bool Down() => Backend.MouseUp(box.transform.min, box.transform.max, MouseButton.Left);
    
    public bool Held() => Backend.MouseHeld(box.transform.min, box.transform.max, MouseButton.Left);
    
    public bool Hovered() => Backend.MouseInBounds(box.transform.min, box.transform.max);
    
    
    
    protected internal override void Draw()
    {
        if (Held()) box.color = backgroundHeldColor;
        else if (Hovered()) box.color = backgroundHoverColor;
        else box.color = backgroundColor;
    }
}