using System.Diagnostics;
using System.Reflection;
using System.Runtime.Loader;
using OsmiumBedrock;
using OsmiumNucleus;
using Debug = OsmiumNucleus.Debug;

namespace OsmiumEditor;

public static partial class Editor
{
    public static AssemblyLoadContext EditorLoadContext = new AssemblyLoadContext(null, true);

    public const string EditorCompileCSProjFront = $"<Project Sdk=\"Microsoft.NET.Sdk\">\n\n    <PropertyGroup>\n        <TargetFramework>{NetVersion}</TargetFramework>\n        <ImplicitUsings>enable</ImplicitUsings>\n        <Nullable>enable</Nullable>\n        <OutputPath>{EditorCompileOutputPath}</OutputPath>\n    </PropertyGroup> \n\n   <ItemGroup>\n        <PackageReference Include=\"OpenTK\" Version=\"4.9.4\" />\n    </ItemGroup> \n   <PropertyGroup>\n\n</PropertyGroup>";
    public const string EditorCompileCSProjEnd = "\n\n</Project>\n";

    public const string EditorCompileOutputPath = ".editorCompileOutputPath";
    
    public const string EditorCompileCSProjPath = ".editorCompileCSProj";
    
    public const string CSProjModuleReferenceStart =
        "<ItemGroup>\n      <Reference Include=\"";

    public const string CSProjModuleReferenceMiddle = "\">\n        <HintPath>";

    public const string CSProjModuleReferenceEnd = "</HintPath>\n      </Reference>\n    </ItemGroup>";

    public const string NetVersion = "net10.0";
    

    public static void RefreshEditor() {
        if (Osmium.IsInitialized)
        {
            Osmium.VirtualClose();
        }

        string EditorCompileCSProj = EditorCompileCSProjFront;
        
        foreach (string file in Directory.GetFiles(Project.GetProjectSubdirectory(true, "Modules", "Runtime"), "*.dll", SearchOption.AllDirectories)) {
            EditorCompileCSProj += CSProjModuleReferenceStart + Assembly.LoadFile(file).GetName().Name + CSProjModuleReferenceMiddle + file + CSProjModuleReferenceEnd;
        }
        
        EditorCompileCSProj += CSProjModuleReferenceStart + typeof(Osmium).Assembly.GetName().Name + CSProjModuleReferenceMiddle + typeof(Osmium).Assembly.Location + CSProjModuleReferenceEnd;
        EditorCompileCSProj += CSProjModuleReferenceStart + typeof(Bedrock).Assembly.GetName().Name + CSProjModuleReferenceMiddle + typeof(Bedrock).Assembly.Location + CSProjModuleReferenceEnd;

        EditorCompileCSProj += EditorCompileCSProjEnd;
        
        File.WriteAllText(Path.Combine(Project.ProjectPath, $"{EditorCompileCSProjPath}.csproj"), EditorCompileCSProj);
        
        Process? process = Process.Start(new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = $"build {EditorCompileCSProjPath}.csproj",
            WorkingDirectory = Project.ProjectPath,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = false
        });
        
        Debug.Log(process.StandardOutput.ReadToEnd());
        
        //todo: add error checking
        
        process?.WaitForExit();
        
        string dllPath = Project.GetProjectSubpath(false, EditorCompileOutputPath, NetVersion, EditorCompileCSProjPath) + ".dll";
        
        EditorLoadContext.LoadFromAssemblyPath(dllPath);
        
        //todo: load module assemblies
        
        Osmium.VirtualInitialize([..RuntimeModules, ..EditorLoadContext.Assemblies]);
        
        File.Delete(Path.Combine(Project.ProjectPath, $"{EditorCompileCSProjPath}.csproj"));
        Directory.Delete(Path.Combine(Project.ProjectPath, EditorCompileOutputPath), true);
    }
}