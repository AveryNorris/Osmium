using System.Reflection;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OsmiumNucleus;
using OsmiumRadium;
using Window = OsmiumRadium.Window;

namespace OsmiumEditor;

public class DebugOverlay : RetainedElement
{
    
    
    
    private float frameratesum = 0;
    private int frameratecount = 0;
    
    private List<float> FrameRates = [];
    
    private string CommandText = string.Empty;
    
    private int offset;

    private int menuState = -1;

    private Scene targetScene = null;
    
    protected override void Update() {
        if (Input.GetKeyDown(Keys.RightShift))
        {
            menuState = menuState == -1 ? 0 : -1;
        }
    }

    protected override void Draw() {
        Region().Depth(-1);
        
        if (menuState == 0) {
            FrameRates.Add(1f / Osmium.DeltaTime);
            if (FrameRates.Count > 1000) {
                FrameRates.RemoveAt(0);
            }
            
            frameratesum += Osmium.DeltaTime;
            frameratecount++;
            
            TextBox().Text("AFPS : " + (int)(1f / (frameratesum / frameratecount))).TextSize(5).Spacing(.45f, 1).Size(100).TextColor(Color.FromRgb(255,255,0));
            
            TextBox().Text("IFPS : " + (1f / Osmium.DeltaTime)).Pos(26,0).Size(100).TextSize(5).Spacing(.45f, 1).TextColor(Color.FromRgb(0,255,255));
            TextBox().Text(" > " + FrameRates.Min()).Pos(26,5).TextSize(5).Size(100).Spacing(.45f, 1).TextColor(Color.FromRgb(0,255,255));
            TextBox().Text(" < " + FrameRates.Max()).Pos(26,10).TextSize(5).Spacing(.45f, 1).Size(100).TextColor(Color.FromRgb(0,255,255));
            
            TextBox().Text("RElements : " + OsmiumRadium.Backend.RetainedElements.Count).TextSize(5).Pos(62,0).Size(100).Spacing(.45f, 1).TextColor(Color.FromRgb(255,0,255));
            TextBox().Text("IElements : " + OsmiumRadium.Backend.immediateElementCount).TextSize(5).Pos(62,6).Size(100).Spacing(.45f, 1).TextColor(Color.FromRgb(255,0,255));
            
            TextBox().Text("Screen Size : (" + OsmiumRadium.Backend.WindowWidth + ',' + OsmiumRadium.Backend.WindowHeight + ") : " + OsmiumRadium.Backend.WindowWidthHeightRatio).TextSize(5).Pos(0,95).Size(100).Spacing(.45f, 1).TextAnchor(TextAnchor.TopLeft).TextColor(Color.FromRgb(0,255,0));
            
            TextBox().Pos(0).TextAnchor(TextAnchor.Center).TextSize(5).Size(100).Spacing(.45f, 1).TextColor(Color.FromRgb(255,255,0)).Text("_> " + CommandText);

            CommandText += Input.TextInput;
            
            if (Input.GetKeyDown(Keys.Enter)) {
                EnactCommand();
            }


        }else if (menuState == 1)
        {
            DrawWindowOpener();
        }else if (menuState == 2)
        {
            DrawCreateMenu();
        }else
        {
            CommandText = "";
            frameratesum = 0;
            frameratecount = 0;
            offset = 0;
            targetScene = null;
            FrameRates.Clear();
        }
        
        Exit();
    }

    public void EnactCommand() {
        CommandText = CommandText.ToLower();

        string[] commandArgs = CommandText.Split(' ');
        
        if (CommandText is "opw" or "openwindow") {
            menuState = 1;
        }
        
        //todo: make the number keys cycle through the debug menu 
        if (CommandText is "run") {
            Osmium.VirtualRun();
        }
        
        if (CommandText is "crt" or "create") {
            
            
            menuState = 2;


        }

        if (commandArgs.Length > 1)
        {
            if (commandArgs[0] is "scn" or "scene")
            {
                for (int i = 1; i < commandArgs.Length; i++)
                {
                    Debug.Log("Created Scene -> "  + commandArgs[i]);
                    targetScene = Osmium.AddScene(commandArgs[i])!;
                }
            }
        }

        if (CommandText is "dw" or "debugwindows")
        {
            DebugFlags.DebugWindows = !DebugFlags.DebugWindows;
        }
        
        CommandText = "";
    }

    public void DrawWindowOpener() {
        //todo: make it easier to configure window regions?
        
        if (Context.LoadedProgram == null)
        {
            //todo: make vector have implicit conversion from int so i dont have so many bounds overloads?
            TextBox().Size(100).Center(50).TextAnchor(TextAnchor.Center).TextSize(5).Spacing(.45f, 1).TextColor(Color.FromRgb(255,255,0)).Text("Loaded program does not exist \n press rightshift to return").Depth(-1);
            return;
        }

        //todo: region default size is 100
        Region();

        int count = 0;
        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        {//todo: change osmium editor window name in the os to the project opened
            foreach (Type type in assembly.GetTypes())
            {
                if (type.IsSubclassOf(typeof(Window)))
                {
                    //todo: error for adding things during update
                    if (offset == count)
                    {
                        Box().Size(new Vector2(100, 5)).Center(50, 20 + 5 * count).Color(Palette.Primary).Depth(-1);

                        if (Input.GetKeyDown(Keys.Enter))
                        {
                            Window window = Activator.CreateInstance(type) as Window;
                            window.Rect = Rect.FromCenterSize(50, 50);
                            Backend.Add(window);
                            menuState = 0;
                            offset = 0;
                            Exit();
                            return;
                        }
                        
                        //todo sick debug menu that can mess with windows and expand the open window command and set object values with commands
                    }
                    //todo: make sorting layers or Z or something? for retained elements
                    TextBox().Size(100).Center(50, 20 + 5 * count).TextAnchor(TextAnchor.Center).TextSize(5).Spacing(.45f, 1).TextColor(Color.FromRgb(0,255,0)).Text(type.FullName).Depth(-1);
                    
                    count++;
                }
            }
            //todo: commands called exe that executes code, and commands that make adding stuff easy and commands that start and stop the game
        }
        
        //todo: change input and other radium classes to include radium, so the distinction between input collecting modules is clear, or make it a subclass of the editor
        if (Input.GetKeyDown(Keys.Down))
        {
            offset++;
        }else if (Input.GetKeyDown(Keys.Up))
        {
            offset--;
        }
        
        Exit();
    }
    
    public void DrawCreateMenu() {
        //todo: make it easier to configure window regions?
        
        if (Context.LoadedProgram == null)
        {
            //todo: make vector have implicit conversion from int so i dont have so many bounds overloads?
            TextBox().Size(100).Center(50).TextAnchor(TextAnchor.Center).TextSize(5).Spacing(.45f, 1).TextColor(Color.FromRgb(255,0,255)).Text("Loaded program does not exist \n press rightshift to return").Depth(-1);
            return;
        }

        //todo: region default size is 100
        Region();
        
        //todo: make the math library not dumb, and maybe make am attribute for a global usage appenditure
        //todo: add GL vec3.xy and whatnot

        int count = 0;
        foreach (Assembly assembly in Context.GetAssemblies())
        {//todo: change osmium editor window name in the os to the project opened
            foreach (Type type in assembly.GetTypes())
            {
                if (type.IsSubclassOf(typeof(Component)))
                {
                    //todo: error for adding things during update
                    if (offset == count)
                    {
                        Box().Size(new Vector2(100, 5)).Center(50, 20 + 5 * count).Color(Palette.Primary).Depth(-1);

                        if (Input.GetKeyDown(Keys.Enter))
                        {
                            Component component = Activator.CreateInstance(type) as Component;
                            
                            if(component != null)
                                targetScene?.Add(component);
                            
                            menuState = 0;
                            offset = 0;
                            Exit();
                            return;
                        }
                        
                        //todo sick debug menu that can mess with windows and expand the open window command and set object values with commands
                    }
                    //todo: make sorting layers or Z or something? for retained elements
                    TextBox().Size(100).Center(50, 20 + 5 * count).TextAnchor(TextAnchor.Center).TextSize(5).Spacing(.45f, 1).TextColor(Color.FromRgb(0,255,0)).Text(type.FullName).Depth(-1);
                    
                    count++;
                }
            }
        }
        
        //todo: change input and other radium classes to include radium, so the distinction between input collecting modules is clear, or make it a subclass of the editor
        if (Input.GetKeyDown(Keys.Down))
        {
            offset++;
        }else if (Input.GetKeyDown(Keys.Up))
        {
            offset--;
        }
        
        Exit();
    }
}