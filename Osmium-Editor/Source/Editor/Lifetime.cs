using System.Reflection;
using System.Runtime.Loader;
using OpenTK.Windowing.Common;
using OsmiumNucleus;

namespace OsmiumEditor;

public static partial class Editor
{
    
    public static void OpenProject(string __path) {
            
        Debug.Action("Opening project! ", ["Path"], [__path]);
        
        ProjectMemory.RefreshProjectTime(__path);
        
        string parentDirectory = Path.GetDirectoryName(__path);
        Project.ProjectPath = parentDirectory;

        Bedrock.window.WindowBorder = WindowBorder.Resizable;
        
        Reload();
        
        _EditorModules.Resolving += (context, assemblyName) =>
        {
            return _RuntimeModules.Assemblies
                .FirstOrDefault(a =>
                    AssemblyName.ReferenceMatchesDefinition(
                        a.GetName(),
                        assemblyName));
        };

        foreach (string editorModule in Directory.GetFiles(Project.GetProjectSubdirectory(true, "Modules", "Editor"), "*.dll", SearchOption.AllDirectories))
            _EditorModules.LoadFromAssemblyPath(editorModule);

        List<MethodInfo> OnEditorOpenEvents = [];
        
        foreach (Assembly assembly in _EditorModules.Assemblies) foreach (Type type in assembly.GetTypes()) foreach (MethodInfo eventMethod in type.GetMethods()) {
            if (eventMethod.IsStatic && eventMethod.GetCustomAttributes(typeof(OnEditorOpen), true).Length > 0 && eventMethod.GetParameters().Length == 0) {
                OnEditorOpenEvents.Add(eventMethod);
            }
        }

        foreach (MethodInfo method in OnEditorOpenEvents) {
            method.Invoke(null, null);
        }
    }
}