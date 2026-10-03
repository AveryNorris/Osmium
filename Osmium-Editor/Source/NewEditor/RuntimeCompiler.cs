using System.Diagnostics;
using System.Reflection;
using OsmiumBedrock;
using OsmiumNucleus;

namespace OsmiumEditor;

public static partial class Editor
{
    
    public const string RuntimeCompileCSProjFront = $"<Project Sdk=\"Microsoft.NET.Sdk\">\n\n    <PropertyGroup>\n      <OutputType>Exe</OutputType>\n        <TargetFramework>{NetVersion}</TargetFramework>\n        <ImplicitUsings>enable</ImplicitUsings>\n        <Nullable>enable</Nullable>\n        <OutputPath>{RuntimeCompileOutputPath}</OutputPath>\n    </PropertyGroup> \n\n   <ItemGroup>\n        <PackageReference Include=\"OpenTK\" Version=\"4.9.4\" />\n    </ItemGroup> \n   <PropertyGroup>\n    <BaseIntermediateOutputPath>{RuntimeCompileOutputPath}/</BaseIntermediateOutputPath>\n</PropertyGroup>";

    
    public const string RuntimeCompileCSProjEnd = "\n\n</Project>\n";
    
    public const string RuntimeCompileOutputPath = "Build/obj";
    public const string RuntimeCompileCSProjPath = ".runtimeCompileCSProj";
    
    public const string TopLevelNugget = "using System.Reflection;\n\nOsmium.Initialize();\n\nforeach (AssemblyName assembly in Assembly.GetExecutingAssembly().GetReferencedAssemblies())\n{\n    Assembly.Load(assembly);\n    Console.WriteLine(assembly.FullName);\n}\n\nBedrock.Run(); \nOsmium.Run();";
    public const string TopLevelNuggetPath = ".topLevelNugget";

    
    public static void CompileProject() {
        string CSProj = RuntimeCompileCSProjFront;
        
        foreach (string file in Directory.GetFiles(Project.GetProjectSubdirectory(true, "Modules", "Runtime"), "*.dll", SearchOption.AllDirectories)) {
            CSProj += CSProjModuleReferenceStart + Assembly.LoadFile(file).GetName().Name + CSProjModuleReferenceMiddle + file + CSProjModuleReferenceEnd;
        }
        
        CSProj += CSProjModuleReferenceStart + typeof(Osmium).Assembly.GetName().Name + CSProjModuleReferenceMiddle + typeof(Osmium).Assembly.Location + CSProjModuleReferenceEnd;
        CSProj += CSProjModuleReferenceStart + typeof(Bedrock).Assembly.GetName().Name + CSProjModuleReferenceMiddle + typeof(Bedrock).Assembly.Location + CSProjModuleReferenceEnd;

        CSProj += RuntimeCompileCSProjEnd;
        
        File.WriteAllText(Project.GetProjectSubpath(true, $"{RuntimeCompileCSProjPath}.csproj"), CSProj);
        File.WriteAllText(Project.GetProjectSubpath(true, $"{TopLevelNuggetPath}.cs"), TopLevelNugget);
        
        Process? process = Process.Start(new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = $"build {RuntimeCompileCSProjEnd}.csproj",
            WorkingDirectory = Project.ProjectPath,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = false
        });
        
        process?.WaitForExit();
        
        string dllPath = Project.GetProjectSubpath(false, EditorCompileOutputPath, NetVersion, EditorCompileCSProjPath) + ".dll";
        
        EditorLoadContext.LoadFromAssemblyPath(dllPath);
        
        Osmium.VirtualInitialize([..RuntimeModules, ..EditorLoadContext.Assemblies]);
        
        File.Delete(Path.Combine(Project.ProjectPath, $"{RuntimeCompileCSProjEnd}.csproj"));
    }
}