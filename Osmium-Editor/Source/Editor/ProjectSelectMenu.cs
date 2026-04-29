using System.Drawing;
using System.Numerics;
using System.Reflection;
using System.Text;
using NativeFileDialogNET;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OsmiumNucleus;
using OsmiumRadium;
using RadiumTest2;
using Vector2 = System.Numerics.Vector2;


namespace OsmiumEditor;


public class ProjectSelectMenu : RadiumElement
{
    private static Font jetbrains = new Font(Assembly.GetAssembly(typeof(Editor)).GetManifestResourceStream("OsmiumEditor.Assets.jetbrainsMonoRegular.png"), 100, 19, [32,136]);
    
    //todo: add back texture caching
    private static Texture osmiumLogo;

    static ProjectSelectMenu() {
        using (Stream stream = Assembly.GetAssembly(typeof(Editor)).GetManifestResourceStream("OsmiumEditor.Assets.Osmium.png")) {
            osmiumLogo = new Texture(stream);
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
        
        var Background = new Box(new Transform(
                size: new Vector2(100, 100)), 
            color: Palette.BackgroundLow);
        
        var HorizontalDividerLine = new Box(color: Palette.BackgroundHighest, transform: new Transform(min: new Vector2(0, 16.75f), max: new Vector2(100, 17)));
        var VerticalDividerLine = new Box(color: Palette.BackgroundHighest, transform: new Transform(min: new Vector2(2, 16.75f), max: new Vector2(2.25f, 100)));
    }
    
    //todo: change component map method from reload and unload to save?

    public void DefineHeader() {
        var OsmiumLogo = new Image(
            osmiumLogo,
            transform: new Transform(size: Vector2.One * 13, pos: new Vector2(2, 2)),
            color: Palette.White);
        
        var Text = new Text("Osmium", 
            font: jetbrains, 
            pos: new Vector2(19.5f, 2f), 
            color: Palette.Primary, size: 13, 
            spacing: new Vector2(.5f,1));
        
        var ExitButton = new Button(text: new Text("X", color: Palette.TextLow, spacing: new Vector2(.33f, 1)),
            transform: new Transform(pos: new Vector2(95.5f, 0), size: new Vector2(4.5f)),
            backgroundColor: Palette.BackgroundHighest, backgroundHoverColor: Palette.BackgroundHigh,
            backgroundHeldColor: Palette.BackgroundLow);
        
        var CreateButton = new Button(
            transform: new Transform(pos: new Vector2(2.25f, 16.75f), size: new Vector2(48.5f, 5)), 
            new Text("Create", size: 3f, spacing: new Vector2(.6f,1)));
        
        //todo: make open and create button in front and fix Z
        var OpenButton = new Button(
            transform: new Transform(pos: new Vector2(51.4f, 16.75f), size: new Vector2(48.875f, 5)), 
            new Text("Open", size: 3f, spacing: new Vector2(.6f,1)));
        
        var VersionText = new Text('V' + Osmium.Version, color: Palette.TextHigh, size: 3, spacing: new Vector2(.5f,1));
        VersionText.pos = new Vector2(100, 16) - VersionText.bounds;
        
        //todo: radium scrolling abstractions debug color options
        
        
        if (ExitButton.Active()) Osmium.Close();
        
        if (CreateButton.Active()) {
            CreateProjectPrompt();
        }

        if (OpenButton.Active()) {
            OpenProjectPrompt();
        }
        
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

    public float scroll = 0;
    
    public void ProjectList() {

        float listSize = 0;
        
        Radium.SetClippingBounds(new Vector2(2.5f, 22), new Vector2(100, 100));
        
        int futureOffset = 0;
        for(int i = 0; i < ProjectMemory.Projects.Count; i++) {
            
            string path = ProjectMemory.Projects[i];
            string projectName = Path.GetFileNameWithoutExtension(path);

            Vector2 pos = new Vector2(5, 26 + futureOffset * 2.1f + i * 11 + scroll);
            
            StringBuilder indentedPath = new StringBuilder();
            for (int c = 0; c < path.Length; c++) {
                
                //wrap text if it is too many characters
                if (c % 65 == 0) {
                    indentedPath.Append('\n');
                    futureOffset++;
                }

                indentedPath.Append(path[c]);
            }
            
            Vector2 size = new Vector2(5, 26 + futureOffset * 2.1f + (i + 1) * 11 + scroll) - pos;
            var BackgroundButton = new Button(transform: new Transform(pos: new Vector2(2.5f, pos.Y - 1.5f), size: new Vector2(97.5f, size.Y)), backgroundColor: Palette.BackgroundLow, backgroundHoverColor: Palette.Primary, backgroundHeldColor: Palette.SecondaryActive);

            var ProjectText = new Text(projectName, pos: pos, spacing: new Vector2(.55f, 1), size: 3.5f);

            Vector2 linkOffset = new Vector2(.5f, 3.5f);
            
            var ProjectLink = new Text(indentedPath.ToString(), pos: pos + linkOffset, spacing: new Vector2(.55f, 1), size: 2.5f, color: Palette.TextLow);
            
            var ForgetButton =
                new Button(transform: new Transform(center: pos + new Vector2(90, 1), size: Vector2.One * 5),
                    new Text("x", color: Palette.TextLow), backgroundColor: Palette.Transparent, backgroundHoverColor: Palette.Transparent, backgroundHeldColor: Palette.Transparent);

            listSize += size.Y;

            if (ForgetButton.Active())
            {
                ProjectMemory.ForgetProject(path);
            }else if (BackgroundButton.Active())
            {        
                Radium.Remove<ProjectSelectMenu>();
                Context.OpenProject(path);
                Radium.Add<EditorOverhead>();
                return;
            }

        }
        
        scroll += Backend.ScrollDeltaY;
        
        //88 is the screen size for the project list, only allow scrolling past it
        float listOverflowSize = listSize - 72.5f;
        
        if(listOverflowSize < 0) listOverflowSize = 0;
        
        
        scroll = Math.Clamp(scroll, -listOverflowSize, 0);
        
        Radium.SetClippingBounds(new Vector2(0), new Vector2(100));
    }
}