using System.Numerics;
using OsmiumRadium;


namespace OsmiumEditor;

public class Inspector : RadiumElement
{

    public const int Size = 20;
    
    protected override void Draw() {
        ConfigureWindow();

        
    }

    private void ConfigureWindow() {
        
        //todo: window element?
        var BackgroundBox = new Box(new Transform(min: new Vector2(100 - Size, 0), max: new Vector2(100, 100)), color: Palette.BackgroundLow);

        var DividerLine = new Box(new Transform(pos: new Vector2(100 - Size, 0), size: new Vector2(.125f, 100)), color: Palette.BackgroundHigh);
        
        var Header = new Box(new Transform(pos: new Vector2(100 - Size, 0), size: new Vector2(Size, 3.125f)), color: Palette.Secondary);
        var HeaderText = new Text("Inspector", pos: new Vector2((100 - Size) + .5f, .9f), spacing: new Vector2(.285f, 1), size: 1.6f);
    }
}