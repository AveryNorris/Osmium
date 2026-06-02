using System.Reflection;
using OsmiumNucleus;
using OsmiumRadium;

namespace OsmiumEditor;

public class CompileTesting : RetainedElement
{


    public bool added = false;
    
    protected override void Draw() {
        if (added) return;

        Backend.Add<TestWindow>().bounds = new Bounds(size: new Vector2(50), center: new Vector2(50));
        added = true;

        
        foreach (Assembly assembly in Context.LoadedProgram.Assemblies)
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (type.FullName == "Osmium_RenderLib_Editor.RendererWindow")
                {
                    Debug.Log("added window");
                    Window window = Activator.CreateInstance(type) as Window;
                    window.bounds = new Bounds(size: new Vector2(100,100));
                    //todo: move extension methods to bounds and make current methods extend bounds extensions?
                    
                    Backend.Add(window);
                }
            }
        }
        
        //todo: prevent anything other than editor from accessing nucleus internals?
    }
}