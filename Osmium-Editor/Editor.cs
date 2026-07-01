
using System.Reflection;
using Dear_ImGui_Sample.Backends;
using ImGuiNET;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Common.Input;
using OsmiumEditor.Source.DearImGUINET.Structure;
using OsmiumEditor.Source.NewEditor;
using OsmiumNucleus;
using OsmiumRadium;
using StbImageSharp;
using Image = OpenTK.Windowing.Common.Input.Image;

namespace OsmiumEditor;

public static class Editor
{
    public static int Main(string[] __args) {
        BedrockImGUICompatability.Incorporate();
        Osmium.EditorInitialize();

        Bedrock.Load += Load;
        
        Bedrock.Run();
        

        /*
        Console.WriteLine("Manifest Names " + string.Join('\n', Assembly.GetAssembly(typeof(Editor))!.GetManifestResourceNames()));
        
        ImageResult image;
        using (Stream stream = Assembly.GetAssembly(typeof(Editor)).GetManifestResourceStream("OsmiumEditor.Assets.Osmium.png"))
        {
            image = ImageResult.FromStream(stream);
        }
        
        if(!OperatingSystem.IsMacOS())
            Osmium.Window.Icon = new WindowIcon(new Image(image.Width, image.Height, image.Data));
            
        TextBox.DefaultFont = Font.FromBitmapStream(Assembly.GetAssembly(typeof(Editor)).GetManifestResourceStream("OsmiumEditor.Assets.proggyBitmapASCII.png"), 75, 19, [32,136]);
        TextBox.DefaultColor = Palette.Text;
        //Text.DefaultSpacingFactor = new Vector2(.285f, 1);
        //Text.DefaultTextSize = 1.6f;
            //todo: dont use manifest resource stream
        //viewport must be 36.56 high, if it is 65 wide todo:
        
        Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.Personal));


        //Backend.Add<ProjectSelectMenu>();
        Backend.Add<DebugOverlay>();
        
        //todo: make osmium mapping and delete scenes and components and collect garbage before then

        Context.OnUnload += ComponentMap.ComponentMap.Unload;
        Context.OnReload += ComponentMap.ComponentMap.Reload;

        Osmium.Window.UpdateFrame += (FrameEventArgs e) => Context.Update();
        
        */
        
        Osmium.EditorRun();
        
        return 0;
    }

    public static void Load() {
        EditorWindowHierarchy.Add<ProjectMenu>();
    }
}