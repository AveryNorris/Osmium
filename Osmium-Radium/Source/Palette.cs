using System.Drawing;

public static class Radium
{
    public static List<Color> ColorPalette = [
        
        Color.Magenta, //unkown
        
        Color.FromArgb(48, 122, 255), //primary
        Color.FromArgb(35, 68, 108), //secondary
        Color.FromArgb(66, 150, 250), //secondary hover
        Color.FromArgb(15, 135, 250), //secondary active
        
        Color.FromArgb(24, 24, 24), //background highest
        Color.FromArgb(20, 20, 20), //background high
        Color.FromArgb(16, 16, 16), //background medium
        Color.FromArgb(14, 14, 14), //background low
        
        Color.FromArgb(255, 255, 255), //text high
        Color.FromArgb(50, 50, 50), //text low
        
        Color.White
    ];
    
    //todo: file customization for palettes?
}

public enum Palette : uint
{
    
    Unknown,
    
    Primary,
    Secondary,
    SecondaryHover,
    SecondaryActive,
    
    BackgroundHighest,
    BackgroundHigh,
    BackgroundMedium,
    BackgroundLow,
    
    TextHigh,
    TextLow,
    
    White,
}