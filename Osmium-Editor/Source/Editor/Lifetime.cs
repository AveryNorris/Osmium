using System.Reflection;
using System.Runtime.Loader;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OsmiumBedrock;
using OsmiumNucleus;

namespace OsmiumEditor;

public static partial class Editor
{

    public const string GlobalUsage =
        "//-------------------------- READ ME --------------------------\n// THIS IS AN AUTO-GENERATED FILE! EDITING THIS IS OK, BUT\n// BE SURE YOU KNOW WHAT YOU ARE DOING, THE AVERAGE USER HAS NO REASON TO BE HERE\n//-------------------------------------------------------------\n\n//Automatically references OsmiumNucleus to make life easier!\nglobal using OsmiumNucleus;\nglobal using OsmiumBedrock;\n\n//Most IDES will automatically add this file to avoid redundant usage statements for obvious libraries\n//this is added to match the user experience of most IDES\nglobal using System;\nglobal using System.Collections.Generic;\nglobal using System.IO;\nglobal using System.Linq;\nglobal using System.Net.Http;\nglobal using System.Threading;\nglobal using System.Threading.Tasks;\n\n//The word IEnumerator can be confusing for beginners! Coroutine is cleaner and more readable\nglobal using Coroutine = System.Collections.Generic.IEnumerator<OsmiumNucleus.ICoroutineAction>;";

    public const string TopLevelNugget = "using System.Reflection;\n\nOsmium.Initialize();\n\nforeach (AssemblyName assembly in Assembly.GetExecutingAssembly().GetReferencedAssemblies())\n{\n    Assembly.Load(assembly);\n    Console.WriteLine(assembly.FullName);\n}\n\nBedrock.Run(); \nOsmium.Run();";

    public const string CSProjFront = "<Project Sdk=\"Microsoft.NET.Sdk\">\n\n    <PropertyGroup>\n        <OutputType>Exe</OutputType>\n        <TargetFramework>net10.0</TargetFramework>\n        <ImplicitUsings>enable</ImplicitUsings>\n        <Nullable>enable</Nullable>\n        <OutputPath>Build</OutputPath>\n\n        <SelfContained>false</SelfContained>\n        <PublishSingleFile>false</PublishSingleFile>\n        <PublishTrimmed>false</PublishTrimmed>\n        <PublishReadyToRun>false</PublishReadyToRun>\n    </PropertyGroup>";
    public const string CSProjEnd = "</Project>\n";

    public const string CSProjModuleReferenceStart =
        "<ItemGroup>\n      <Reference Include=\"";

    public const string CSProjModuleReferenceMiddle = "\">\n        <HintPath>";

    public const string CSProjModuleReferenceEnd = "</HintPath>\n      </Reference>\n    </ItemGroup>";

    public static void OpenProject(string __path) {
        Debug.Action("Opening project! ", ["Path"], [__path]);
        
        ProjectMemory.RefreshProjectTime(__path);
        Project.ProjectPath = Path.GetDirectoryName(__path);
        File.WriteAllText(Path.Combine(Project.GetProjectSubdirectory(true, "Editor"), "GlobalUsage.cs"), GlobalUsage);
        
        //todo: use strign builder, and use relative paths so the csproj is valid across git repositories
        string csProj = CSProjFront;

        foreach (string file in Directory.GetFiles(Project.GetProjectSubdirectory(true, "Modules"), "*.dll", SearchOption.AllDirectories))
        {
            //todo: gross and somewhat temporary,
            //todo: once I start supporting module development, there should be a toggle setting to add editor modules as well so that your IDE doesnt flip the math out
            csProj += CSProjModuleReferenceStart + Assembly.LoadFile(file).GetName().Name + CSProjModuleReferenceMiddle + file + CSProjModuleReferenceEnd;
        }
        
        csProj += CSProjModuleReferenceStart + typeof(Osmium).Assembly.GetName().Name + CSProjModuleReferenceMiddle + typeof(Osmium).Assembly.Location + CSProjModuleReferenceEnd;
        csProj += CSProjModuleReferenceStart + typeof(Bedrock).Assembly.GetName().Name + CSProjModuleReferenceMiddle + typeof(Bedrock).Assembly.Location + CSProjModuleReferenceEnd;
        csProj += "<ItemGroup>\n      <PackageReference Include=\"ImGui.NET\" Version=\"1.91.6.1\" />\n      <PackageReference Include=\"Microsoft.CodeAnalysis.CSharp\" Version=\"5.9.0\" />\n      <PackageReference Include=\"NativeFileDialogNET\" Version=\"2.0.2\" />\n      <PackageReference Include=\"OpenTK\" Version=\"4.9.4\" />\n      <PackageReference Include=\"StbImageSharp\" Version=\"2.30.15\" />\n      <PackageReference Include=\"System.IO.Compression\" Version=\"4.3.0\" />\n    </ItemGroup>";

        csProj += CSProjEnd;
        
        File.WriteAllText(Path.Combine(Project.ProjectPath, "Project.csproj"), csProj);
        
        Bedrock.window.WindowBorder = WindowBorder.Resizable;

        foreach (string module in Directory.GetFiles(Project.GetProjectSubdirectory(true, "Modules"), "*.dll", SearchOption.AllDirectories)) _Modules.LoadFromAssemblyPath(module);

        //todo: reinitialize after compiling the program
        Osmium.VirtualInitialize(_Modules.Assemblies);

        List<MethodInfo> OnEditorOpenEvents = [];
        
        foreach (Assembly assembly in _Modules.Assemblies) foreach (Type type in assembly.GetTypes()) foreach (MethodInfo eventMethod in type.GetMethods()) {
            if (eventMethod.IsStatic && eventMethod.GetCustomAttributes(typeof(OnEditorOpen), true).Length > 0 && eventMethod.GetParameters().Length == 0) {
                OnEditorOpenEvents.Add(eventMethod);
            }
        }

        foreach (MethodInfo method in OnEditorOpenEvents) {
            method.Invoke(null, null);
        }
        
        Compile();
    }
}