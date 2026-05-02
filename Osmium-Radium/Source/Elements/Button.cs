using System.Drawing;
using System.Numerics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OsmiumRadium;

public class Button : ImGUI
{
    
    
    
    public Color backgroundColor;
    public static Color DefaultBackgroundColor = Palette.Secondary;
    
    public Color backgroundHoverColor;
    public static Color DefaultBackgroundHoverColor = Palette.SecondaryHover;

    public Color backgroundHeldColor;
    public static Color DefaultBackgroundHeldColor = Palette.SecondaryActive;
    
    public Box box;
    
    public Text text;
    
    
    
    public Button(Bounds? transform = null, Text? text = null, Color? backgroundColor = null, Color? backgroundHoverColor = null, Color? backgroundHeldColor = null, int z = 0) {

        Bounds buttonBounds = transform ?? new Bounds();

        this.text = text ?? new Text();
        this.text.center = buttonBounds.center;
        
        this.box = new Box(buttonBounds);
        this.box.z = z;
        
        //puts the _text on top of the Z stack, _text is instantiated in the constructor so it will always be behind the box unless you reset it.
        this.text.z = z;
        
        this.backgroundColor = backgroundColor ?? DefaultBackgroundColor;
        this.backgroundHoverColor = backgroundHoverColor ?? DefaultBackgroundHoverColor;
        this.backgroundHeldColor = backgroundHeldColor ?? DefaultBackgroundHeldColor;
    }
    
    
    
    public bool Active() => Backend.MouseUp(box.Bounds.min, box.Bounds.max, MouseButton.Left);
    
    public bool Held() => Backend.MouseHeld(box.Bounds.min, box.Bounds.max, MouseButton.Left);
    
    public bool Hovered() => Backend.MouseInBounds(box.Bounds.min, box.Bounds.max);
    
    
    
    protected internal override void Draw()
    {
        if (Held()) box.color = backgroundHeldColor;
        else if (Hovered()) box.color = backgroundHoverColor;
        else box.color = backgroundColor;
    }
}