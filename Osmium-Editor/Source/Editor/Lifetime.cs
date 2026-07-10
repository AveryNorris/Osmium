using System.Reflection;
using System.Runtime.Loader;
using OsmiumNucleus;

namespace OsmiumEditor;

public static partial class Editor
{
    
    private static readonly AssemblyLoadContext _EditorModules = new AssemblyLoadContext("EditorModules");
    
    private static readonly List<IEditorModule> _LeadingEditorModules = [];
    
    public static void OpenProject(string __path) {
            
        Debug.Action("Opening project! ", ["Path"], [__path]);
        
        ProjectMemory.RefreshProjectTime(__path);
        
        string parentDirectory = Path.GetDirectoryName(__path);
        Project.ProjectPath = parentDirectory;
        
        

        foreach (string editorModule in Directory.GetFiles(Project.GetProjectSubdirectory(true, "Modules", "Editor"), "*.dll", SearchOption.AllDirectories))
            _EditorModules.LoadFromAssemblyPath(editorModule);

        foreach (Assembly assembly in _LeadingEditorModules) foreach (Type type in assembly.GetTypes())
            if (typeof(IEditorModule).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract) 
                _LeadingEditorModules.Add((IEditorModule)Activator.CreateInstance(type));

        foreach (IEditorModule editorModule in _LeadingEditorModules) 
            editorModule.EditorOpen();
        
        
        
        UpdateTracker.Initialize();
        
        string ComponentMapPath = Project.GetProjectSubPath(Path.Combine("Editor", "Component.osmap"), regenerate: true);
        UpdateTracker.BlacklistedPaths.Add(ComponentMapPath);
        
        Reload();
        
        DockerMap.Load();
        
        //post reload so the assemblies exist

    }
}