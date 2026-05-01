using System.Numerics;
using OsmiumNucleus;
using OsmiumRadium;


namespace OsmiumEditor;

public class SceneHierarchy : RadiumElement
{

    public const int Size = 15;

    public const int Offset = 20;
    
    public const float Height = DebugConsole.Height;

    public static Scene? SelectedScene;
    
    protected override void Draw() {
        ConfigureWindow();

        DisplayScenes();
    }

    private void ConfigureWindow() {
        
        //todo: scene order messed up  on reserialize? 
        
        //todo: window element?
        var BackgroundBox = new Box(new Transform(pos: new Vector2((100 - Size) - Offset, 100 - Height), size: new Vector2(Size, 100 - Height)), color: Palette.BackgroundLow);

        var HorizontalDividerLine = new Box(new Transform(pos: new Vector2((100 - Size) - Offset, 100 - Height), size: new Vector2(.125f, 100 - Height)), color: Palette.BackgroundHigh);
        
        var Header = new Box(new Transform(pos: new Vector2((100 - Size) - Offset, 100 - Height), size: new Vector2(Size, 3.125f + .125f)), color: Palette.Secondary);
        var HeaderText = new Text("Scenes", pos: new Vector2(((100 - Size) + .5f) - Offset, (100 - Height) + .9f), spacing: new Vector2(.285f, 1), size: 1.6f);

        var AddSceneButton = new Button(new Transform(pos: new Vector2((100 - Size) - Offset, 100 - 3.75f), size: new Vector2(Size, 3.75f)), new Text("Add"));
        if (AddSceneButton.Active()) {
            //todo: safeguard and change button to have right click options
            Radium.Add<CreateScenePopup>();
        }

        //var AddSceneButton = new Button()

        //todo: set bounds to prevent text overlap
    }

    private void DisplayScenes() {

        Vector2 pos = new Vector2((100 - Size) - Offset, (100 - Height) + 5.5f);
        
        //todo: reset clipping bounds in catch statements in backend and use transform to represent the bounds
        
        //todo: make a divider line and make z standardized
        
        //todo: make spacing a ratio of text size and screen proportion

        Vector2 min = new Vector2((100 - Size) - Offset, 100 - Height);
        Vector2 size = new Vector2(Size, 100 - Height);
        Radium.SetClippingBounds(min, min + size);
        
        foreach (Scene scene in Osmium.Scenes) {
            var SceneDisplay = new Button(new Transform(size: new Vector2(Size, 3), pos: pos), new Text(scene.Name, size: 1.25f), backgroundColor: Palette.BackgroundLow, backgroundHoverColor: Palette.BackgroundHigh, backgroundHeldColor: Palette.SecondaryHover);
            SceneDisplay.text.pos = pos + new Vector2(1, 1);
            
            if (scene == SelectedScene) {
                SceneDisplay.backgroundColor = Palette.SecondaryHover;
                SceneDisplay.backgroundHeldColor = Palette.SecondaryHover;
                SceneDisplay.backgroundHoverColor = Palette.SecondaryHover;
            }
            
            //todo: make them alternate colors so light gray dark gray light gray dark gray so it looks better and make hover color offset too

            if (SceneDisplay.Active() || SceneDisplay.Held()) {
                SelectedScene = scene;
                ComponentHierarchy.SelectedComponent = null;
            }

            pos += new Vector2(0, 4);
        }
        
        Radium.SetClippingBounds(Vector2.Zero, Vector2.One * 100);
    }
}