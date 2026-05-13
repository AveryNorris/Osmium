using System.Drawing;
using System.Numerics;
using System.Reflection;
using System.Text;
using NativeFileDialogNET;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OsmiumNucleus;
using OsmiumRadium;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Color = OsmiumRadium.Color;
using Vector2 = System.Numerics.Vector2;


namespace OsmiumEditor;


public class ProjectSelectMenu : RadiumElement
{
    private static Font jetbrains = new Font(Assembly.GetAssembly(typeof(Editor)).GetManifestResourceStream("OsmiumEditor.Assets.jetbrainsMonoRegular.png"), 100, 19, [32,136]);
    
    //todo: add back _texture caching
    private static Texture osmiumLogo;
    
    private static string text = "";

    static ProjectSelectMenu() {
        using (Stream stream = Assembly.GetAssembly(typeof(Editor)).GetManifestResourceStream("OsmiumEditor.Assets.Osmium.png")) {
            osmiumLogo = Texture.LoadFromStream(stream);
        }

        for (int i = 0; i < 100_000; i++) {
            text += "C";
        }
    }

    public ProjectSelectMenu() {
        ProjectMemory.RefreshProjectList();
    }

    protected override void Draw() {
        ConfigureWindow();
        DefineHeader();
        ProjectList();
    }

    public void ConfigureWindow() {
        Osmium.Context.WindowBorder = WindowBorder.Hidden;

        Vector2i clientSize = new Vector2i((int) (Osmium.Context.CurrentMonitor.ClientArea.Height * .4f));
        
        Osmium.Context.ClientSize = clientSize;
        Osmium.Context.ClientLocation = Osmium.Context.CurrentMonitor.ClientArea.HalfSize - clientSize / 2;
        
        Box().Size(100, 100).Color(Palette.BackgroundLow);

        Box().Color(Palette.BackgroundHighest).Pos(0, 16.75f).Max(100, 17);
        Box().Color(Palette.BackgroundHighest).Pos(2, 16.75f).Max(2.25f, 100);
    }
    
    //todo: change component map method from reload and unload to save?

    //todo: _bounds setter interface makes it easier to set stuff individually
    public void DefineHeader() {
        Box().Texture(osmiumLogo).Color(Color.White).Pos(2,2).Size(13);
        
        TextBox().Text("Osmium").Font(jetbrains).Pos(14.5f, 2f).Size(100).TextSize(13).Spacing(.5f, 1).TextColor(Palette.Primary);
        
        if(Button().
            Text("X ").
            TextColor(Palette.TextLow).
            TextSize(3).
            Spacing(.33f,1).Pos(95.5f, 0).
            Size(4.5f).NormalColor(Palette.BackgroundHighest).
            HoverColor(Palette.BackgroundHigh).
            ActiveColor(Palette.BackgroundLow).
            TextAnchor(TextAnchor.Center)
        .Up()) Osmium.Close();
        
        if(Button().
            Text("Create").
            TextSize(3).
            Spacing(.6f, 1).
            Pos(2.25f, 16.75f).
            Size(48.5f, 5).
            TextAnchor(TextAnchor.Center).
        Up()) CreateProjectPrompt();
                
        
        //todo: open create buttons dif sizes?
        
        if(Button().
           Text("Open").
           TextSize(3).
           Spacing(.6f, 1).
           Pos(51.4f, 16.75f).
           Size(48.875f, 5).
           TextAnchor(TextAnchor.Center).
           Up()) OpenProjectPrompt();
        
        
        //todo: make open and create button in front and fix Z
        
        //toDO OPISJGOIGIO)SJGIOSG
        
        //todo: text.text
        TextBox().Text('V' + Osmium.Version).Size(100, 16.75f).TextColor(Palette.TextHigh).TextSize(3).Spacing(.5f, 1).TextAnchor(TextAnchor.BottomRight);

        //todo: radium scrolling abstractions debug _color options

    }

    public void CreateProjectPrompt() {
        using NativeFileDialog Dialog = new NativeFileDialog();
        Dialog.SaveFile().AddFilter("Osmium Projects", "osproj");
        
        DialogResult result = Dialog.Open(out string? output);
        if (result == DialogResult.Okay && output != null) {
            string projectName = Path.GetFileNameWithoutExtension(output);
            string projectFolderPath = Path.ChangeExtension(output, null);

            if (Directory.Exists(projectFolderPath)) {
                Debug.Error("A folder with the given name already exists!", ["Path", "Name"], [projectFolderPath, projectName]);
                return;
            }
                
            Directory.CreateDirectory(projectFolderPath);

            //todo: path.combine
            string projectFilePath = projectFolderPath + '/' + projectName + ".osproj";
            File.WriteAllText(projectFilePath, "Created using Osmium: V" + Osmium.Version);
                
            ProjectMemory.AppendProject(projectFilePath);
        }
        
        ProjectMemory.RefreshProjectList();
    }

    public void OpenProjectPrompt() {
        using NativeFileDialog Dialog = new NativeFileDialog();
        Dialog.SelectFile().AddFilter("Osmium Projects", "osproj");
        
        DialogResult result = Dialog.Open(out string? output);
        if (result == DialogResult.Okay && output != null) {
                
            ProjectMemory.AppendProject(output);
        }
        
        ProjectMemory.RefreshProjectList();
    }

    
    public void ProjectList() {
        
        //todo : region inheritance pattern
        Region("ProjectList").Min(2.5f, 22).Max(100).Scrolling(true);
        
        int futureOffset = 0;
        for(int i = 0; i < ProjectMemory.Projects.Count; i++) {
            
            string path = ProjectMemory.Projects[i];
            string projectName = Path.GetFileNameWithoutExtension(path);

            Vector2 pos = new Vector2(5, 26 + futureOffset * 2.1f + i * 11);
            
            StringBuilder indentedPath = new StringBuilder();
            for (int c = 0; c < path.Length; c++) {
                
                //wrap _text if it is too many characters
                if (c % 65 == 0) {
                    indentedPath.Append('\n');
                    futureOffset++;
                }

                indentedPath.Append(path[c]);
            }
            
            Vector2 size = new Vector2(5, 26 + futureOffset * 2.1f + (i + 1) * 11) - pos;

            if (Button().
                Pos(2.5f, pos.Y - 1.5f).
                Size(97.5f, size.Y).
                NormalColor(Palette.BackgroundLow).
                HoverColor(Palette.Primary).
                ActiveColor(Palette.SecondaryActive).
            Up()) {
                Remove<ProjectSelectMenu>();
                Context.OpenProject(path);
                return;
            }

            TextBox().Text(projectName).Pos(pos).Size(100).Spacing(.55f, 1).TextSize(3.5f).TextColor(Palette.TextHigh);

            Vector2 linkOffset = new Vector2(.5f, 3.5f);
            
            TextBox().Text(indentedPath.ToString()).Pos(pos + linkOffset).Size(100).Spacing(.55f, 1).TextSize(2.5f).TextColor(Palette.TextLow);
            
            //todo: turn off file replace when making projects maybe?
            
            if(Button().Center(pos.X + 88.5f, pos.Y).Size(5).TextSize(3).Text('x').TextColor(Palette.TextLow).AllColors(Palette.Transparent).Up())
                ProjectMemory.ForgetProject(path);

        }
        
        Exit();

    }
}