using System.Numerics;
using OpenTK.Windowing.Common.Input;
using OsmiumNucleus;
using OsmiumRadium;
using RadiumTest2;
using StbImageSharp;
using Image = OpenTK.Windowing.Common.Input.Image;

namespace OsmiumEditor;

public static class Editor
{
    public static int Main(string[] __args) {
        Osmium.EditorInitialize();
        
        ImageResult image;
        using (FileStream stream = File.OpenRead("/Users/averynorris/Osmium/Osmium-Nucleus/Osmium.png"))
        {
            image = ImageResult.FromStream(stream);
        }
            
        //Osmium.Context.Icon = new WindowIcon(new Image(image.Width, image.Height, image.Data));
            
        Text.DefaultFont = new Font("/Users/averynorris/Programming/Radium-Test2/RadiumFonts/ProggyClean.radfont");
        Text.DefaultColor = Palette.TextHigh;
        Text.DefaultSpacingFactor = new Vector2(.285f, 1);
        Text.DefaultTextSize = 1.6f;
            
        //viewport must be 36.56 high, if it is 65 wide todo:

        Button.DefaultBackgroundColor = Palette.Secondary;
        Button.DefaultBackgroundHoverColor = Palette.Secondary;
        Button.DefaultBackgroundHeldColor = Palette.SecondaryActive;
        
        Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.Personal));


        Radium.Add<ProjectSelectMenu>();

        Osmium.Run();
        
        return 0;
    }
}