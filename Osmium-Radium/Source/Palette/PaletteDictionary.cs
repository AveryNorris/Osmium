namespace OsmiumRadium;

public static class PaletteDictionary
{
    //default colors
    public static Dictionary<Palette, Color> ColorDictionary = new ()
    {
        { Palette.Error, Color.FromRgb(0,255,255) },
        
        { Palette.Primary, Color.FromRgb(48, 122, 255) },
        
        { Palette.Secondary, Color.FromRgb(35, 68, 108) },
        { Palette.Secondary | Palette.Selected, Color.FromRgb(66, 150, 250) },
        { Palette.Secondary | Palette.Active,  Color.FromRgb(15, 135, 250) },
        
        { Palette.Background | Palette.High,  Color.FromRgb(24, 24, 24) },
        { Palette.Background | Palette.Medium,  Color.FromRgb(20, 20, 20) },
        { Palette.Background | Palette.Low,  Color.FromRgb(16, 16, 16) },
        { Palette.Background,  Color.FromRgb(14, 14, 14) },
        
        
        { Palette.Text, Color.FromRgb(255, 255, 255) },
        { Palette.Text | Palette.Low, Color.FromRgb(50, 50, 50) },
        
        { Palette.Border, Color.FromRgb(24, 24, 24) },

        //constant
        
        { Palette.White, Color.FromRgb(255, 255, 255) },
        { Palette.Black, Color.FromRgb(0, 0, 0) },
        { Palette.Red, Color.FromRgb(255, 0, 0) },
        { Palette.Green, Color.FromRgb(0, 255, 0) },
        { Palette.Blue, Color.FromRgb(0, 0, 255) },
        { Palette.Transparent, Color.FromArgb(0, 0, 0, 0) },
    };
    
    
    public static void SetColor(Palette color, Color value) {
        ColorDictionary[color] = value;
    }
}