using System.Numerics;
using System.Reflection;
using OsmiumNucleus;
using OsmiumRadium;
/*

namespace OsmiumEditor;

public class ComponentHierarchy : RadiumElement
{

    public const int Size = 15;

    public const int Offset = 20;

    public const float Height = 100 - (DebugConsole.Height);

    public static Component? SelectedComponent;

    public static Texture ComponentTexture;
    
    public static Texture PackageTexture;

    //todo: make sure subscription leaks dont happen
    static ComponentHierarchy() {
        Context.OnUnload += () => SelectedComponent = null;
        
        using (Stream stream = Assembly.GetAssembly(typeof(Editor)).GetManifestResourceStream("OsmiumEditor.Assets.Component.png")) { 
            ComponentTexture = new Texture(stream);
        }
        
        //todo: assembly constant?
        using (Stream stream = Assembly.GetAssembly(typeof(Editor)).GetManifestResourceStream("OsmiumEditor.Assets.Package.png")) { 
            PackageTexture = new Texture(stream);
        }
    }
    
    protected override void Draw() {
        ConfigureWindow();

        DisplayComponents();
    }

    private void ConfigureWindow() {
        
        //todo: window element?
        var BackgroundBox = new Box(new Bounds(pos: new Vector2((100 - Size) - Offset, 0), size: new Vector2(Size, Height)), color: Palette.BackgroundLow);

        var DividerLine = new Box(new Bounds(pos: new Vector2((100 - Size) - Offset, 0), size: new Vector2(.125f, Height)), color: Palette.BackgroundHigh);
        
        var Header = new Box(new Bounds(pos: new Vector2((100 - Size) - Offset, 0), size: new Vector2(Size, 3.125f)), color: Palette.Secondary);
        var HeaderText = new Text("Hierarchy", pos: new Vector2(((100 - Size) + .5f) - Offset, .9f), spacing: new Vector2(.285f, 1), size: 1.6f);
        
        //todo: set _bounds to prevent _text overlap
    }

    private void DisplayComponents() {

        Vector2 pos = new Vector2((100 - Size) - Offset, 5.5f);
        
        //todo: reset clipping _bounds in catch statements in backend and use _bounds to represent the _bounds

        Vector2 min = new Vector2((100 - Size) - Offset, 0);
        Vector2 size = new Vector2(Size, Height);
        //Radium.SetClippingBounds(min, min + _size);
        //todo: clipping _bounds

        if (SceneHierarchy.SelectedScene == null) {
            //Radium.SetClippingBounds(Vector2.Zero, Vector2.One * 100);
            return;
        }

        count = 0;
        foreach (Component component in SceneHierarchy.SelectedScene) {

            DisplayComponent(0, component);
        }
        
        var AddComponentButton = new Button(new Bounds(pos: new Vector2((100 - Size) - Offset, Height - 3.75f), size: new Vector2(Size, 3.75f)), new Text("Add"));
        if (AddComponentButton.Active()) {
            //todo: safeguard and change button to have right click options
            Radium.Add<CreateComponentPopup>();
        }
        
        //Radium.SetClippingBounds(Vector2.Zero, Vector2.One * 100);
    }

    private int count = 0;

    private void DisplayComponent(int __depth, Component __component) {
        string name = __component.Name;
            
        //todo default name in nucleus to string.empty not ""
        if (name == string.Empty)
            name = __component.GetType().Name;
        
        Vector2 pos = new Vector2((100 - Size) - Offset, 5.5f + 4 * count);
        
        //todo: clear selected component when scenes switch
        //todo: add unselecting components by clicking anywhere
        
            
        var SceneDisplay = new Button(new Bounds(size: new Vector2(Size, 3), pos: pos), new Text(name, size: 1.25f), backgroundColor: Palette.Transparent, backgroundHoverColor: Palette.BackgroundHigh, backgroundHeldColor: Palette.SecondaryHover);
        SceneDisplay.text.pos = pos + new Vector2(2.5f, 1) + new Vector2(.5f,0) * __depth;    
            
        //todo: one fram disparity between selection nerd
        if (__component == SelectedComponent) {
            SceneDisplay.backgroundColor = Palette.SecondaryHover;
            SceneDisplay.backgroundHeldColor = Palette.SecondaryHover;
            SceneDisplay.backgroundHoverColor = Palette.SecondaryHover;
        }
            
        //todo: make them alternate colors so light gray dark gray light gray dark gray so it looks better and make hover _color offset too

        if (SceneDisplay.Active() || SceneDisplay.Held()) {
            SelectedComponent = __component;
        }
        
        //todo: console.clear button!
        //todo: watch int/float overflow for stuff
        if (__component.GetType() == typeof(Package)) {
            new Image(PackageTexture, new Bounds(pos: (pos + new Vector2(.4f, .6f))+ new Vector2(.5f,0) * __depth, size: new Vector2(2 * r169, 2) * new Vector2(26/20f, 1)));
        } else {
            new Image(ComponentTexture, new Bounds(pos: pos + new Vector2(.4f, .6f)+ new Vector2(.5f,0) * __depth, size: new Vector2(2 * r169, 2)));
        }
        

        count++;

        foreach (Component component in __component)
        {
            DisplayComponent(__depth + 1, component);
        }
    }
}
*/