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
using OsmiumRadium.Source.Interfaces;
using Anchor = OsmiumRadium.Source.Interfaces.Anchor;
using Vector2 = System.Numerics.Vector2;


namespace OsmiumEditor;


public class ProjectSelectMenu : RadiumElement
{
    private static Font jetbrains = new Font(Assembly.GetAssembly(typeof(Editor)).GetManifestResourceStream("OsmiumEditor.Assets.jetbrainsMonoRegular.png"), 100, 19, [32,136]);

    private static float frameratesum = 0;
    private static int frameratecount = 0;
    
    //todo: add back _texture caching
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
        
        //todo: delta time float?
        
        //STARTING FPS IS 160
        
        //todo: add debug overloads
        
        //todo: 12770
        //todo: 14800
        
        //Debug.Log("Current : " + 1 / Osmium.DeltaTime);
        //todo: optimize _text
        
        //Radium.SetClippingBounds(new Bounds(size: new Vector2(100,100)));
        
        //for(int i = 0; i < 100; i++)
        //    Box()
        //        .Size(i / 200f,i / 200f)
        //        .Pos(i / 200f * i, i / 200f * i)
        //        .Color((100 - i) * 255, 255 * i, 0);

        //todo : text KILLS performance
        //Text().Text("Hello!\n I am Text").Size(100,100).Size(20).Spacing(.3f,1f);
        
        //todo: make text have parameters and make interfaces more explicit? so maybe anchor is text anchor etc, same for text size!

        ButtonData.DefaultNormalColor = Palette.Secondary;
        ButtonData.DefaultActiveColor = Palette.SecondaryActive;
        ButtonData.DefaultHoverColor = Palette.SecondaryHover;
        
        ButtonData.DefaultTextColor = Palette.White;

        for(int i = 0; i < 100; i++)
            Button().Size(10, 7).Center(50, 50).Text("Hello I am a\nbutton").Anchor(Anchor.Center).TextSize(2f).Spacing(.3f,1f);
        
        frameratesum += Osmium.DeltaTime;
        frameratecount++;
        
        Debug.Log("Average : " + 1f / (frameratesum / frameratecount));



        //ConfigureWindow();

        //DefineHeader();

        //ProjectList();
    }

    public void ConfigureWindow() {
        Osmium.Context.WindowBorder = WindowBorder.Hidden;

        Vector2i clientSize = new Vector2i((int) (Osmium.Context.CurrentMonitor.ClientArea.Height * .4f));
        
        Osmium.Context.ClientSize = clientSize;
        Osmium.Context.ClientLocation = Osmium.Context.CurrentMonitor.ClientArea.HalfSize - clientSize / 2;
        
        Box(new Bounds(
                size: new Vector2(100, 100)), 
            color: Palette.BackgroundLow);
        
        Box(color: Palette.BackgroundHighest, bounds: new Bounds(min: new Vector2(0, 16.75f), max: new Vector2(100, 17)));
        Box(color: Palette.BackgroundHighest, bounds: new Bounds(min: new Vector2(2, 16.75f), max: new Vector2(2.25f, 100)));
    }
    
    //todo: change component map method from reload and unload to save?

    //todo: _bounds setter interface makes it easier to set stuff individually
    public void DefineHeader() {
        Image(
            osmiumLogo,
            bounds: new Bounds(size: Vector2.One * 13, pos: new Vector2(2, 2)),
            color: Palette.White);
        
        Text("Osmium", 
            font: jetbrains, 
            bounds: new Bounds(pos: new Vector2(14.5f, 2f), size: new Vector2(100,100)),
            color: Palette.Primary, size: 13, 
            spacing: new Vector2(.5f,1));

        if(Button("X", textColor: Palette.TextLow, spacing: new Vector2(.33f, 1),
            bounds: new Bounds(pos: new Vector2(95.5f, 0), size: new Vector2(4.5f)),
            backgroundColor: Palette.BackgroundHighest, hoverColor: Palette.BackgroundHigh,
            heldColor: Palette.BackgroundLow, anchor: OsmiumRadium.Anchor.Center).MouseUp(MouseButton.Left))
            Osmium.Close();

        if(Button(
            "Create", textSize: 3, spacing: new Vector2(.6f, 1),
            bounds: new Bounds(pos: new Vector2(2.25f, 16.75f), size: new Vector2(48.5f, 5)), anchor: OsmiumRadium.Anchor.Center).MouseUp(MouseButton.Left))
            CreateProjectPrompt();
        
        if(Button(
            "Open", textSize: 3, spacing: new Vector2(.6f, 1),
            bounds: new Bounds(pos: new Vector2(51.4f, 16.75f), size: new Vector2(48.875f, 5)), anchor: OsmiumRadium.Anchor.Center).MouseUp(MouseButton.Left))
            OpenProjectPrompt();
        
        
        //todo: make open and create button in front and fix Z
        
        //toDO OPISJGOIGIO)SJGIOSG
        Text('V' + Osmium.Version, new Bounds(size: new Vector2(100, 16.75f)),  color: Palette.TextHigh, size: 3, spacing: new Vector2(.5f,1), anchor: OsmiumRadium.Anchor.BottomRight);
        
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

    public float scroll = 0;
    
    public void ProjectList() {

        float listSize = 0;
        
        SetClippingBounds(new Bounds(min: new Vector2(2.5f, 22), max: new Vector2(100, 100)));
        
        int futureOffset = 0;
        for(int i = 0; i < ProjectMemory.Projects.Count; i++) {
            
            string path = ProjectMemory.Projects[i];
            string projectName = Path.GetFileNameWithoutExtension(path);

            Vector2 pos = new Vector2(5, 26 + futureOffset * 2.1f + i * 11 + scroll);
            
            StringBuilder indentedPath = new StringBuilder();
            for (int c = 0; c < path.Length; c++) {
                
                //wrap _text if it is too many characters
                if (c % 65 == 0) {
                    indentedPath.Append('\n');
                    futureOffset++;
                }

                indentedPath.Append(path[c]);
            }
            
            Vector2 size = new Vector2(5, 26 + futureOffset * 2.1f + (i + 1) * 11 + scroll) - pos;

            if (Button(string.Empty,
                    bounds: new Bounds(pos: new Vector2(2.5f, pos.Y - 1.5f), size: new Vector2(97.5f, size.Y)),
                    backgroundColor: Palette.BackgroundLow, hoverColor: Palette.Primary,
                    heldColor: Palette.SecondaryActive).MouseUp(MouseButton.Left)) {
                Radium.Remove<ProjectSelectMenu>();
                Context.OpenProject(path);
                Radium.Add<EditorOverhead>();
                return;
            }

            Text(projectName, new Bounds(pos: pos, size: Vector2.One * 100), spacing: new Vector2(.55f, 1), size: 3.5f);

            Vector2 linkOffset = new Vector2(.5f, 3.5f);
            
            Text(indentedPath.ToString(), new Bounds(pos: pos + linkOffset, size: Vector2.One * 100), spacing: new Vector2(.55f, 1), size: 2.5f, color: Palette.TextLow);
            
            if(Button(bounds: new Bounds(center: pos + new Vector2(90, 1), size: Vector2.One * 5),
                    text: "x", textColor: Palette.TextLow, backgroundColor: Palette.Transparent, hoverColor: Palette.Transparent, heldColor: Palette.Transparent).MouseUp(MouseButton.Left))
            ProjectMemory.ForgetProject(path);

            listSize += size.Y;

        }
        
        scroll += Backend.ScrollDeltaY;
        
        //88 is the screen _size for the project list, only allow scrolling past it
        float listOverflowSize = listSize - 72.5f;
        
        if(listOverflowSize < 0) listOverflowSize = 0;
        
        
        scroll = Math.Clamp(scroll, -listOverflowSize, 0);
        
        SetClippingBounds(new Bounds(size: new Vector2(100,100)));
    }
}