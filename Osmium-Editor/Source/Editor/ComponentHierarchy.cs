using System.Numerics;
using OsmiumRadium;
using RadiumTest2;

namespace OsmiumEditor;

public class ComponentHierarchy : RadiumElement
{

    public const int Size = 15;

    public const int Offset = 20;

    public const float Height = 100 - (DebugConsole.Height);
    
    protected override void Draw() {
        ConfigureWindow();

        DisplayComponents();
    }

    private void ConfigureWindow() {
        
        //todo: window element?
        var BackgroundBox = new Box(new Transform(pos: new Vector2((100 - Size) - Offset, 0), size: new Vector2(Size, Height)), color: Palette.BackgroundLow);

        var DividerLine = new Box(new Transform(pos: new Vector2((100 - Size) - Offset, 0), size: new Vector2(.125f, Height)), color: Palette.BackgroundHigh);
        
        var Header = new Box(new Transform(pos: new Vector2((100 - Size) - Offset, 0), size: new Vector2(Size, 6.75f)), color: Palette.Secondary);
        var HeaderText = new Text("Hierarchy", pos: new Vector2(((100 - Size) + .5f) - Offset, 4.5f), spacing: new Vector2(.285f, 1), size: 1.6f);
        
        //todo: set bounds to prevent text overlap
    }

    private void DisplayComponents() {
        
    }
}