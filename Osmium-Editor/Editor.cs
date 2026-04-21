using System.Drawing;
using OsmiumRadium;

namespace RadiumTest2;

public static class Editor
{
    public static Palette Palette => new Palette(
        Color.FromArgb(48, 122, 255), //primary
        Color.FromArgb(35, 68, 108), //secondary
        Color.FromArgb(66, 150, 250), //secondary hover
        Color.FromArgb(15, 135, 250), //secondary active
        Color.FromArgb(24, 24, 24), //background highest
        Color.FromArgb(20, 20, 20), //background high
        Color.FromArgb(16, 16, 16), //background medium
        Color.FromArgb(14, 14, 14), //background low
        Color.FromArgb(255, 255, 255), //text high
        Color.FromArgb(50, 50, 50) //text low
    );

    public static Color Get(OsmiumPalette palette)
    {
        return Palette[(uint) palette];
    }
}

public enum OsmiumPalette : uint
{
    Primary,
    Secondary,
    SecondaryHover,
    SecondaryActive,
    BackgroundHighest,
    BackgroundHigh,
    BackgroundMedium,
    BackgroundLow,
    TextHigh,
    TextLow
}