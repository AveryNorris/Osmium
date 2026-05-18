
using OsmiumRadium;


namespace OsmiumEditor;

public class Inspector : RetainedElement
{

    public const int Size = 20;
    
    protected override void Draw() {
        ConfigureWindow();

        
    }

    private void ConfigureWindow() {
        
        //todo: window element?
        Box().Max(100).Min(100 - Size, 0).Color(Palette.BackgroundLow);

        Box().Pos(100 - Size, 0).Size(.125f, 100).Color(Palette.BackgroundHigh);
        Box().Pos(100 - Size, 0).Size(Size, 3.125f).Color(Palette.Secondary);
        TextBox().Text("Inspector").Pos(100 - Size + .5f, .9f).Size(100).Spacing(.45f, 1).TextSize(1.6f).TextColor(Palette.TextHigh);
    }
}
