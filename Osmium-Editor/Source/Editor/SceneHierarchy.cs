using System.Numerics;
using OsmiumNucleus;
using OsmiumRadium;
using RadiumTest2;

namespace OsmiumEditor;

public class SceneHierarchy : RadiumElement
{

    public const int Size = 15;

    public const int Offset = 20;
    
    public const float Height = DebugConsole.Height;

    public static Scene? SelectedScene;
    
    protected override void Draw() {
        ConfigureWindow();

        DisplayComponents();
    }

    private void ConfigureWindow() {
        
        //todo: window element?
        var BackgroundBox = new Box(new Transform(pos: new Vector2((100 - Size) - Offset, 100 - Height), size: new Vector2(Size, 100 - Height)), color: Palette.BackgroundLow);

        var HorizontalDividerLine = new Box(new Transform(pos: new Vector2((100 - Size) - Offset, 100 - Height), size: new Vector2(.125f, 100 - Height)), color: Palette.BackgroundHigh);
        
        var Header = new Box(new Transform(pos: new Vector2((100 - Size) - Offset, 100 - Height), size: new Vector2(Size, 3.125f + .125f)), color: Palette.Secondary);
        var HeaderText = new Text("Scenes", pos: new Vector2(((100 - Size) + .5f) - Offset, (100 - Height) + .9f), spacing: new Vector2(.285f, 1), size: 1.6f);
        
        //var AddSceneButton = new Button()
        
        //todo: set bounds to prevent text overlap
    }

    private void DisplayComponents() {
        
    }
}