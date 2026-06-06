using System.Reflection;
using System.Text;
using NativeFileDialogNET;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OsmiumNucleus;
using OsmiumRadium;
using Vector2 = OsmiumRadium.Vector2;



namespace OsmiumEditor;


public class ProjectSelectMenu : RetainedElement
{
    
    
    
    private static Font jetbrains = Font.FromBitmapStream(Assembly.GetAssembly(typeof(Editor)).GetManifestResourceStream("OsmiumEditor.Assets.jetbrainsMonoRegular.png"), 100, 19, [32,136]);
    

    
    private static Texture osmiumLogo;

    
    
    static ProjectSelectMenu() {
        using (Stream stream = Assembly.GetAssembly(typeof(Editor)).GetManifestResourceStream("OsmiumEditor.Assets.Osmium.png")) {
            osmiumLogo = Texture.LoadFromStream(stream);
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

        Vector2i targetSize = new Vector2i((int) (Osmium.Context.CurrentMonitor.ClientArea.Height * .4f));
        
        Osmium.Context.ClientSize = targetSize;
        Osmium.Context.ClientLocation = Osmium.Context.CurrentMonitor.ClientArea.HalfSize - targetSize / 2;
        
        Box().Size(100, 100).Color(Palette.Background);

        Box().Color(Palette.Border).Corners(0, 16.75f, 100, 17);
        Box().Color(Palette.Border).Corners(2, 16.75f, 2.25f, 100);

        const float lineSize = .125f;

        //todo: use a window with draggable set to false
        
        Region(() =>
        {
            Box().Corners(0, 0, 100, lineSize);
            Box().Corners(0, 0, lineSize, 100);
            Box().Corners(0, 100 - lineSize, 100, 100);
            Box().Corners(0, 100 - lineSize, 100, 100);
        }).Depth(IDepthElementExtensions.Header);
        
        
    }   
    
    public void DefineHeader() {
        
        Box().Texture(osmiumLogo).Color(Palette.White).Pos(2,2).Size(13);
        
        TextBox().Text("Osmium").Font(jetbrains).Pos(14.5f, 2f).Size(100).TextSize(13).Spacing(.5f, 1).TextColor(Palette.Primary);
        
        if(Button().
            Text("X ").
            TextColor(Palette.Text | Palette.Low).
            TextSize(3).
            Spacing(.33f,1).Pos(95.5f, 0).
            Size(4.5f).NormalColor(Palette.Background | Palette.High).
            HoverColor(Palette.Background | Palette.Medium).
            ActiveColor(Palette.Background).
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
        
        TextBox().Text('V' + Osmium.Version).Size(100, 16.75f).TextColor(Palette.Text).TextSize(3).Spacing(.5f, 1).TextAnchor(TextAnchor.BottomRight);
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

    private float scroll = 0;
    
    public void ProjectList() {
        
        //todo : region inheritance pattern

        const float horizontalStart = 5;
        const float verticalStart = 26;

        const float lineBreakHeight = 2.1f;
        const float projectHeight = 11;

        const int pathLineBreakCharacters = 65;
        
        int lineBreaks = 0;
        Group(ProjectMemory.Projects, (count, element) => {

            bool forgetting = false;

            string path = element;
            string projectName = Path.GetFileNameWithoutExtension(path);

            Vector2 pos = new Vector2(horizontalStart, verticalStart + lineBreaks * lineBreakHeight + count * projectHeight);
            Vector2 size = new Vector2(0, projectHeight);
            
            StringBuilder indentedPath = new StringBuilder();
            for (int c = 0; c < path.Length; c++) {
                if (c % pathLineBreakCharacters == 0) {
                    indentedPath.Append('\n');
                    lineBreaks++;
                }

                indentedPath.Append(path[c]);
            }
            

            Button projectButton = Button().Pos(2.5f, pos.y - 1.5f).Size(97.5f, size.y).NormalColor(Palette.Background)
                .HoverColor(Palette.Primary).ActiveColor(Palette.Secondary | Palette.Active);

            TextBox().Text(projectName).Pos(pos).Size(100).Spacing(.55f, 1).TextSize(3.5f).TextColor(Palette.Text);

            Vector2 linkOffset = new Vector2(.5f, 3.5f);
            
            TextBox().Text(indentedPath.ToString()).Pos(pos + linkOffset).Size(100).Spacing(.55f, 1).TextSize(2.5f).TextColor(Palette.Text | Palette.Low);
            
            //todo: turn off file replace when making projects maybe?
            
            if(Button().Center(pos.x + 88.5f, pos.y).Size(5).TextSize(3).Text('x').TextColor(Palette.Text | Palette.Low).AllColors(Palette.Transparent).Up())
            { ProjectMemory.ForgetProject(path); forgetting = true; }
            
            if(projectButton.Up() && !forgetting)
            {
                Backend.RemoveElement(this);
                Backend.Add<EditorOverhead>();
                Context.OpenProject(path);
            }

        }).Corners(2.5f, 22, 100, 100).Depth(0).Scrollable(ref scroll, verticalStart + lineBreaks * lineBreakHeight + ProjectMemory.Projects.Count * projectHeight);
    }
}