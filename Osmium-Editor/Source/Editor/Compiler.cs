using System.Diagnostics;
using System.Runtime.Loader;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using OsmiumBedrock;
using OsmiumNucleus;
using Debug = OsmiumNucleus.Debug;

namespace OsmiumEditor;

public static partial class Editor
{
    
    public static AssemblyLoadContext EditorLoadContext = new AssemblyLoadContext(null, true);

    public static string BuildDirectory;

    public static void Save() {
        OnSave?.Invoke();
    }
    
    /// <summary> Builds all dependencies into one program </summary>
    public static void RuntimeCompile() {
        OnCompile?.Invoke();
        
        List<SyntaxTree> Trees = [];
        foreach (string file in Directory.GetFiles(Project.ProjectPath, "*.cs", SearchOption.AllDirectories))
        {
            Debug.Log(file);
            Trees.Add(CSharpSyntaxTree.ParseText(File.ReadAllText(file)));
        }
        

        Compilation comp = CSharpCompilation.Create("Program", Trees, GetDependencies(), options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        FileStream compiledEditorCode = File.Create(Path.Combine(Project.ProjectPath, ".compilationUserCode.dll"));
        EmitResult emit = comp.Emit(compiledEditorCode);
        compiledEditorCode.Close();

        foreach (Diagnostic error in emit.Diagnostics)
        {
            Debug.Error(error.GetMessage());
        }
        
        string csProj = GenerateCSProj([Path.Combine(Project.ProjectPath, ".compilationUserCode.dll")], true);
        File.WriteAllText(Path.Combine(Project.ProjectPath, ".compilationCSProj.csproj"), csProj);
        
        File.WriteAllText(Path.Combine(Project.ProjectPath, ".compilationTopNugget.cs"), TopLevelNugget);
        Process? process = Process.Start(new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = "build .compilationCSProj.csproj",
            WorkingDirectory = Project.ProjectPath,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false
        });
        
        //todo: user code is not being left in the app domain as it gets excluded becuase there is no non reflection reference

        if (process == null)
        {
            Debug.Error("Failed to create build process.");
        }
        
        string output = process!.StandardOutput.ReadToEnd();
        string errors = process.StandardError.ReadToEnd();

        process.WaitForExit();

        Console.WriteLine(output);
        Console.Error.WriteLine(errors);

        Console.WriteLine($"Exit code: {process.ExitCode}");
        
        File.Delete(Path.Combine(Project.ProjectPath, ".compilationCSProj.csproj"));
        File.Delete(Path.Combine(Project.ProjectPath, ".compilationTopNugget.cs"));
        Directory.Delete(Path.Combine(Project.ProjectPath, "obj"), true);
        //File.Copy(Path.Combine(Project.GetProjectSubdirectory(true, "Build", "net10.0"), ".compilationCSProj.csproj"), Path.Combine(Project.ProjectPath, "Build"));
        //File.Delete(Path.Combine(Path.Combine()));
    }
    
    /// <summary> Builds just the scripts in source, and loads them temporarily </summary>
    public static void EditorCompile() {

        if (Osmium.IsInitialized) {
            Osmium.VirtualClose();
        }
        
        List<SyntaxTree> Trees = [];
        foreach (string file in Directory.GetFiles(Project.ProjectPath, "*.cs", SearchOption.AllDirectories)) {
            Trees.Add(CSharpSyntaxTree.ParseText(File.ReadAllText(file)));
        }
        

        Compilation comp = CSharpCompilation.Create("Program", Trees, GetDependencies(), options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        MemoryStream compiledEditorCode = new MemoryStream();
        EmitResult emit = comp.Emit(compiledEditorCode);

        foreach (Diagnostic error in emit.Diagnostics)
        {
            if (error.Severity == DiagnosticSeverity.Error)
            {
                Debug.Error(error.GetMessage());
            }
        }
        
        compiledEditorCode.Position = 0;
        
        EditorLoadContext.LoadFromStream(compiledEditorCode);
        
        Osmium.VirtualInitialize(AppDomain.CurrentDomain.GetAssemblies());
        
        compiledEditorCode.Close();
        
        Debug.Log("Successfully compile and loaded editor code");
    }
    
    public static MetadataReference[] GetDependencies() {
        var paths = new HashSet<string>(
            GetTrustedPlatformAssemblyPaths().Concat(GetExternalModulePaths()).Concat(GetCoreDependenciesPath())
        );

        var references = new List<MetadataReference>();

        foreach (var path in paths)
        {
            if (!File.Exists(path))
                continue;

            try
            {
                references.Add(MetadataReference.CreateFromFile(path));
            }
            catch
            {
                // ignore bad assemblies
            }
        }
        
        return references.ToArray();
    }


    /// todo: add error documentation like C# exceptions? or attributes for that
    /// <summary> Finds all the trusted platform assembly paths, that give essential C# types</summary>
    //public static string[] GetTrustedPlatformLibraryPaths() => AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!.ToString()!.Split(Path.PathSeparator);
    /// <summary> Finds all the external modules that are present in the current plugin directory</summary>
    public static string[] GetExternalModulePaths() =>
        Directory.GetFiles(Project.RuntimeModulesPath, "*.dll", SearchOption.AllDirectories);

    public static string[] GetCoreDependenciesPath() => [typeof(Osmium).Assembly.Location, typeof(Bedrock).Assembly.Location, typeof(OpenTK.Audio.OpenAL.AL).Assembly.Location, typeof(OpenTK.Compute.Native.CLBase).Assembly.Location, typeof(OpenTK.Core.Utils).Assembly.Location, typeof(OpenTK.Graphics.OpenGL4.GL).Assembly.Location, typeof(OpenTK.Input.Hid.HidConsumerUsage).Assembly.Location, typeof(OpenTK.Mathematics.BezierCurve).Assembly.Location, typeof(OpenTK.Platform.Windows.All).Assembly.Location, typeof(OpenTK.Windowing.Common.ContextAPI).Assembly.Location];

    //todo: add extension checks to prevent compiling txt lol

    public static string[] GetTrustedPlatformAssemblyPaths() {
        var tpa = AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES") as string;

        if (string.IsNullOrEmpty(tpa))
            throw new InvalidOperationException("TPA list not available");

        return tpa.Split(Path.PathSeparator);
    }
}