using System.Drawing;
using System.Numerics;
using OsmiumRadium;
using RadiumTest2;

namespace OsmiumEditor;

public class PlayMenu : RadiumElement
{

    public const int Size = 65;
    
    protected override void Draw() {
        ConfigureWindow();

        
    }

    private void ConfigureWindow() {
        
        //todo: window element?
        var BackgroundBox = new Box(new Transform(pos: Vector2.Zero, size: new Vector2(Size, 3.125f)), color: Palette.BackgroundLow);

        var DividerLine = new Box(new Transform(pos: new Vector2(0, 3.125f - .125f), size: new Vector2(Size, .125f)), color: Palette.BackgroundHigh);

        var PlayButton = new Button(new Transform(center: new Vector2(Size / 2f, 3.125f / 2), size: new Vector2(7, 3.125f)), new Text("Play"));
        
        //todo: handle negative size
    }
}