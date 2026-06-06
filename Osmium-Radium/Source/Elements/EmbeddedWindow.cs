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
            Box().Pos(Rect.min).Size(Rect.size).Color(background).Depth(IDepthElementExtensions.BackgroundDepth);

            //todo: bounds.width and bounds.height

            //todo: a depth class that organizes stuff
            //vertical lines
            Box().Pos(Rect.min).Size(LineSize, Rect.size.y).Color(Palette.Border).Depth(IDepthElementExtensions.Header);
            Box().Pos(Rect.max.x, Rect.min.y).Size(LineSize, Rect.size.y).Color(Palette.Border)
                .Depth(IDepthElementExtensions.Header);
            //horizontal
            Box().Pos(Rect.min).Size(Rect.size.x, LineSize).Color(Palette.Border).Depth(IDepthElementExtensions.Header);
            Box().Pos(Rect.min.x, Rect.max.y).Size(Rect.size.x, LineSize).Color(Palette.Border)
                .Depth(IDepthElementExtensions.Header);

            Box().Pos(Rect.min).Size(Rect.size.x, HeaderSize).Color(Palette.Secondary)
                .Depth(IDepthElementExtensions.Header);
            TextBox().Pos(Rect.min).Size(Rect.size.x, HeaderSize).TextSize(HeaderSize).Text(name)
                .Depth(IDepthElementExtensions.Header);

        DrawEmbeddedWindow();
    }

    protected abstract void DrawEmbeddedWindow();
}