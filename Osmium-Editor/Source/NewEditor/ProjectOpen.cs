using System.Diagnostics;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using OpenTK.Windowing.Common;
using OsmiumBedrock;
using OsmiumNucleus;
using Debug = OsmiumNucleus.Debug;

namespace OsmiumEditor;

public static partial class Editor
{

    public static readonly List<Assembly> EditorModules = [];

    public static readonly List<Assembly> RuntimeModules = [];

    public static List<Assembly> Modules => [..EditorModules, ..RuntimeModules];

    public static List<Assembly> Assemblies => [..Modules, ..EditorLoadContext.Assemblies];
    
    public const string IDETrickingCSProjFront = $"<Project Sdk=\"Microsoft.NET.Sdk\">\n\n    <PropertyGroup>\n        <TargetFramework>{NetVersion}</TargetFramework>\n        <ImplicitUsings>enable</ImplicitUsings>\n        <Nullable>enable</Nullable>\n\n    </PropertyGroup> \n\n   <ItemGroup>\n        <PackageReference Include=\"OpenTK\" Version=\"4.9.4\" />\n    </ItemGroup> \n   \n";
    public const string IDETrickingCSProjEnd = "\n\n</Project>\n";
    
    public const string GlobalUsage = "//-------------------------- READ ME --------------------------\n// THIS IS AN AUTO-GENERATED FILE BY OSMIUM! EDITING THIS IS OK, BUT\n// BE SURE YOU KNOW WHAT YOU ARE DOING, THE AVERAGE USER HAS NO REASON TO BE HERE\n//-------------------------------------------------------------\n\n//Automatically references OsmiumNucleus to make life easier!\nglobal using OsmiumNucleus;\nglobal using OsmiumBedrock;\n\n//Most IDES will automatically add this file to avoid redundant usage statements for obvious libraries\n//this is added to match the user experience of most IDES\nglobal using System;\nglobal using System.Collections.Generic;\nglobal using System.IO;\nglobal using System.Linq;\nglobal using System.Net.Http;\nglobal using System.Threading;\nglobal using System.Threading.Tasks;\n\n//The word IEnumerator can be confusing for beginners! Coroutine is cleaner and more readable\nglobal using Coroutine = System.Collections.Generic.IEnumerator<OsmiumNucleus.ICoroutineAction>;";
    
    internal static void OpenProject(string __projectPath) {
        
        //UI AND FACE FORWARD STUFF EXCEPT CSPROJ
        
        Project.ProjectPath = Path.GetDirectoryName(__projectPath);
        ProjectMemory.RefreshProjectTime(__projectPath);
        File.WriteAllText(Project.GetProjectSubpath(true, "Editor", "GlobalUsage.cs"), GlobalUsage);
        Bedrock.window.WindowBorder = WindowBorder.Resizable;
        Bedrock.Unload += SaveProject;
        
        //STATE AND LOADING MODULES

        foreach (string module in Directory.GetFiles(Project.GetProjectSubdirectory(true, "Modules", "Editor"), "*.dll", SearchOption.AllDirectories)) {
            EditorModules.Add(Assembly.LoadFile(module));
        }

        foreach (string module in Directory.GetFiles(Project.GetProjectSubdirectory(true, "Modules", "Runtime"), "*.dll", SearchOption.AllDirectories)) {
            RuntimeModules.Add(Assembly.LoadFile(module));
        }
        
        foreach (Assembly assembly in EditorModules) foreach (Type type in assembly.GetTypes()) foreach (MethodInfo eventMethod in type.GetMethods()) {
            if (eventMethod.IsStatic && eventMethod.GetCustomAttributes(typeof(OnEditorOpen), true).Length > 0 && eventMethod.GetParameters().Length == 0) {
                //todo: originally it saves the events to a list then calls it in a seperate for loop, should it be that way? or is this fine too
                eventMethod.Invoke(null, null);
            }
        }
        
        foreach (Assembly assembly in Modules) {
            foreach (Type type in assembly.GetTypes()) {
                foreach (MethodInfo method in type.GetMethods())
                {
                    //todo: prevent double attributes
                    OnInitialize? attribute = method.GetCustomAttributes(typeof(OnInitialize)).OfType<OnInitialize>().FirstOrDefault();

                    if (attribute != null) {
                        if (attribute.IgnoreVirtualization)
                        {
                            if(method.IsStatic && method.GetParameters().Length == 0 && method.ReturnType == typeof(void))
                                method.Invoke(null,null);
                        }
                    }
                }
            }
        }
        
        //COMPILATION AND LYING TO IDES
        
        StringBuilder sbr = new StringBuilder(IDETrickingCSProjFront);

        foreach (string file in Directory.GetFiles(Project.GetProjectSubdirectory(true, "Modules", "Runtime"), "*.dll", SearchOption.AllDirectories))
        {
            sbr.Append(CSProjModuleReferenceStart + Assembly.LoadFile(file).GetName().Name + CSProjModuleReferenceMiddle + file + CSProjModuleReferenceEnd);
        }
        
        sbr.Append(CSProjModuleReferenceStart + typeof(Osmium).Assembly.GetName().Name + CSProjModuleReferenceMiddle + typeof(Osmium).Assembly.Location + CSProjModuleReferenceEnd);
        sbr.Append(CSProjModuleReferenceStart + typeof(Bedrock).Assembly.GetName().Name + CSProjModuleReferenceMiddle + typeof(Bedrock).Assembly.Location + CSProjModuleReferenceEnd);

        sbr.Append(IDETrickingCSProjEnd);
        
        File.WriteAllText(Project.GetProjectSubpath(true, "Project.csproj"), sbr.ToString());
        
        RefreshEditor();
    }

    public static event Action? OnSave;

    public static void SaveProject() {
        OnSave?.Invoke();
    }
    
    
}