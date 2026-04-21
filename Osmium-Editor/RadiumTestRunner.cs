using System.Drawing;
using System.Numerics;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OsmiumNucleus;
using OsmiumRadium;
using Vector2 = System.Numerics.Vector2;


namespace RadiumTest2;


public class RadiumTestRunner : Component
{
    
    
    public void Update() {
        Osmium.Context.WindowBorder = WindowBorder.Hidden;

        Vector2i clientSize = new Vector2i((int) (Osmium.Context.CurrentMonitor.ClientArea.Height * .4f));
        
        Osmium.Context.ClientSize = clientSize;
        Osmium.Context.ClientLocation = Osmium.Context.CurrentMonitor.ClientArea.HalfSize - clientSize / 2;
        
    }

    private Font jetbrains = new Font("/Users/averynorris/Programming/Radium-Test2/RadiumFonts/JetbrainsMono.radfont");

    public void Draw() {

        new Box(new Transform(size: new Vector2(100, 100)), color: Editor.Get(OsmiumPalette.BackgroundLow));
        
        //new Text("hello", size: 20, spacing: new Vector2(.325f, 1), center: Vector2.One * 50);
        new Image("/Users/averynorris/Programming/Radium-Test2/logo.png", pos: new Vector2(2,2), size: Vector2.One * 13);
        new Text("Osmium", font: jetbrains, pos: new Vector2(19.5f, 2f), color: Editor.Get(OsmiumPalette.Primary), size: 13, spacing: new Vector2(.5f,1));


        if (new Button(text: new Text("X", color: Editor.Get(OsmiumPalette.TextLow), spacing: new Vector2(.33f, 1)),
                transform: new Transform(pos: new Vector2(95.5f, 0), size: new Vector2(4.5f)),
                backgroundColor: Editor.Get(OsmiumPalette.BackgroundHighest), backgroundHoverColor: Editor.Get(OsmiumPalette.BackgroundHigh),
                backgroundHeldColor: Editor.Get(OsmiumPalette.BackgroundLow)).Down()) {
            Osmium.Close();
        }
        
        Text text = new Text('V' + Osmium.Version, color: Color.White, size: 3, spacing: new Vector2(.5f,1));
        text.pos = new Vector2(100, 16) - text.bounds;

        new Box(color: Editor.Get(OsmiumPalette.BackgroundHighest), transform: new Transform(min: new Vector2(0, 16.75f), max: new Vector2(100, 17)));
        new Box(color: Editor.Get(OsmiumPalette.BackgroundHighest), transform: new Transform(min: new Vector2(2, 16.75f), max: new Vector2(2.25f, 100)));

        new Button(new Transform(pos: new Vector2(2.25f, 16.75f), size: new Vector2(48.5f, 5)), new Text("Create", size: 3f, spacing: new Vector2(.6f,1)));
        new Button(new Transform(pos: new Vector2(51.4f, 16.75f), size: new Vector2(48.875f, 5)), new Text("Open", size: 3f, spacing: new Vector2(.6f,1)));
    }
}