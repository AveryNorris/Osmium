using OsmiumRadium;


namespace OsmiumEditor;

public abstract class EmbeddedWindow(string name, Color? background = null) : Window
{

    //give optional constructor items or something so you can customize colors and whatnot
    public const float LineSize = .15f;
    public const float HeaderSize = 2.5f;

    public Color background = background ?? Palette.Background;

    protected override void DrawWindow() {
        
        //todo: window element?
        Box().Pos(bounds.min).Size(bounds.size).Color(background).Depth(1);

        //todo: bounds.width and bounds.height
        
        //todo: a depth class that organizes stuff
        //vertical lines
        Box().Pos(bounds.min).Size(LineSize, bounds.size.y).Color(Palette.Border);
        Box().Pos(bounds.max.x,bounds.min.y).Size(LineSize, bounds.size.y).Color(Palette.Border);
        //horizontal
        Box().Pos(bounds.min).Size(bounds.size.x, LineSize).Color(Palette.Border);
        Box().Pos(bounds.min.x, bounds.max.y).Size(bounds.size.x, LineSize).Color(Palette.Border);

        Box().Pos(bounds.min).Size(bounds.size.x, HeaderSize).Color(Palette.Secondary);
        TextBox().Pos(bounds.min).Size(bounds.size.x, HeaderSize).TextSize(HeaderSize).Text(name);
        
        DrawEmbeddedWindow();
    }

    protected abstract void DrawEmbeddedWindow();
}