using System.Drawing;
using System.Numerics;
using OsmiumNucleus;
using OsmiumRadium;
using RadiumTest2;

namespace OsmiumEditor;

public class PlayMenu : RadiumElement
{

    public const int Size = 65;
    
    public bool Running = false;
    
    protected override void Draw() {
        ConfigureWindow();

        
    }

    private void ConfigureWindow() {
        
        //todo: window element?
        var BackgroundBox = new Box(new Transform(pos: Vector2.Zero, size: new Vector2(Size, 3.125f)), color: Palette.BackgroundLow);

        var DividerLine = new Box(new Transform(pos: new Vector2(0, 3.125f - .125f), size: new Vector2(Size, .125f)), color: Palette.BackgroundHigh);

        Button PlayButton;
        if (!Running) {
            PlayButton =
                    new Button(new Transform(center: new Vector2(Size / 2f, 3.125f / 2), size: new Vector2(7, 3.125f)),
                        new Text("Play"));
        } else {
            PlayButton =
                    new Button(new Transform(center: new Vector2(Size / 2f, 3.125f / 2), size: new Vector2(7, 3.125f)),
                        new Text("Stop"));
        }

        var ReloadButton = new Button(new Transform(center: Vector2.One * 5, size: Vector2.One * 5), new Text("r"));

        //todo: make active a bool and have it a setter
        
        if (ReloadButton.Active())
        {
            Context.Reload();
        }

        if (PlayButton.Active()) {
            if (!Running) {
                Running = true;
                Context.Reload();
                Osmium.VirtualRun();
            } else {
                Running = false;
                Osmium.VirtualClose();
            }
        }

        //todo: handle negative size
    }
}