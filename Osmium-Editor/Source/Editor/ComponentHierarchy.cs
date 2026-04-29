using System.Numerics;
using OsmiumEditor.Source.Components;
using OsmiumNucleus;
using OsmiumRadium;
using RadiumTest2;

namespace OsmiumEditor;

public class ComponentHierarchy : RadiumElement
{

    public const int Size = 15;

    public const int Offset = 20;

    public const float Height = 100 - (DebugConsole.Height);

    public static Component? SelectedComponent;

    //todo: make sure subscription leaks dont happen
    static ComponentHierarchy() {
        Context.OnUnload += () => SelectedComponent = null;
    }
    
    protected override void Draw() {
        ConfigureWindow();

        DisplayComponents();
    }

    private void ConfigureWindow() {
        
        //todo: window element?
        var BackgroundBox = new Box(new Transform(pos: new Vector2((100 - Size) - Offset, 0), size: new Vector2(Size, Height)), color: Palette.BackgroundLow);

        var DividerLine = new Box(new Transform(pos: new Vector2((100 - Size) - Offset, 0), size: new Vector2(.125f, Height)), color: Palette.BackgroundHigh);
        
        var Header = new Box(new Transform(pos: new Vector2((100 - Size) - Offset, 0), size: new Vector2(Size, 3.125f)), color: Palette.Secondary);
        var HeaderText = new Text("Hierarchy", pos: new Vector2(((100 - Size) + .5f) - Offset, .9f), spacing: new Vector2(.285f, 1), size: 1.6f);
        
        //todo: set bounds to prevent text overlap
    }

    private void DisplayComponents() {

        Vector2 pos = new Vector2((100 - Size) - Offset, 5.5f);
        
        //todo: reset clipping bounds in catch statements in backend and use transform to represent the bounds

        Vector2 min = new Vector2((100 - Size) - Offset, 0);
        Vector2 size = new Vector2(Size, Height);
        //Radium.SetClippingBounds(min, min + size);
        //todo: clipping bounds

        if (SceneHierarchy.SelectedScene == null) {
            //Radium.SetClippingBounds(Vector2.Zero, Vector2.One * 100);
            return;
        }

        count = 0;
        foreach (Component component in SceneHierarchy.SelectedScene) {

            DisplayComponent(0, component);
        }
        
        var AddComponentButton = new Button(new Transform(pos: new Vector2((100 - Size) - Offset, Height - 3.75f), size: new Vector2(Size, 3.75f)), new Text("Add"));
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

        Color boxColor = Color.White;
        //todo: console.clear button!
        //todo: watch int/float overflow for stuff
        if (__component.GetType() == Context.LoadedProgram.Assemblies.First(x => x.GetType(typeof(Package).FullName) != null)?.GetType(typeof(Package).FullName))
        {
            boxColor = Color.FromArgb(255, 255, 0, 0);
        }
        
        //todo: clear selected component when scenes switch
        //todo: add unselecting components by clicking anywhere
        
            
        var SceneDisplay = new Button(new Transform(size: new Vector2(Size, 3), pos: pos), new Text(name, size: 1.25f), backgroundColor: Palette.Transparent, backgroundHoverColor: Palette.BackgroundHigh, backgroundHeldColor: Palette.SecondaryHover);
        SceneDisplay.text.pos = pos + new Vector2(2, 1) + new Vector2(2,0) * __depth;    
            
        //todo: one fram disparity between selection nerd
        if (__component == SelectedComponent) {
            SceneDisplay.backgroundColor = Palette.SecondaryHover;
            SceneDisplay.backgroundHeldColor = Palette.SecondaryHover;
            SceneDisplay.backgroundHoverColor = Palette.SecondaryHover;
        }
            
        //todo: make them alternate colors so light gray dark gray light gray dark gray so it looks better and make hover color offset too

        if (SceneDisplay.Active() || SceneDisplay.Held()) {
            SelectedComponent = __component;
        }
        
        var TabColor = new Box(new Transform(pos: pos, size: new Vector2(1, 3)), boxColor);


        count++;

        foreach (Component component in __component)
        {
            DisplayComponent(__depth + 1, component);
        }
    }
}