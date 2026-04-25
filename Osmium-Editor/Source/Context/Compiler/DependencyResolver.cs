using Microsoft.CodeAnalysis;
using OsmiumNucleus;


namespace OsmiumEditor;


public static class DependencyResolver
{

    //todo: make change to match currently opened project
    
    
    public static MetadataReference[] GetDependencies()
    {
        var paths = new HashSet<string>(
            GetTrustedPlatformAssemblyPaths()
                .Concat(GetExternalModulePaths())
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



    public static string[] GetMicrosoftReferenceAssembliesPaths() {
        return Directory.GetFiles("/usr/lib/dotnet/packs/Microsoft.NETCore.App.Ref/9.0.14/ref/net9.0/", "*.dll");
    }
    /// todo: add error documentation like C# exceptions? or attributes for that
    /// <summary> Finds all the trusted platform assembly paths, that give essential C# types</summary>
    //public static string[] GetTrustedPlatformLibraryPaths() => AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!.ToString()!.Split(Path.PathSeparator);
    /// <summary> Finds all the external modules that are present in the current plugin directory</summary>
    public static string[] GetExternalModulePaths() => Directory.GetFiles(Project.ModulesPath, "*.dll", SearchOption.AllDirectories);

    //todo: add extension checks to prevent compiling txt lol
    


    public static MetadataReference? ResolveReferenceFromPath(string path) {
        try {
            return MetadataReference.CreateFromFile(path);
        } catch (ArgumentException) {
            Debug.LogError("The system cannot resolve a given trusted library's path! ", ["Path"], [path]);
        } catch (IOException) {
            Debug.LogError("An error occured while reading a trusted library!", ["Path"], [path]);
        }

        return null;
    }
    
    public static string[] GetTrustedPlatformAssemblyPaths()
    {
        var tpa = AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES") as string;

        if (string.IsNullOrEmpty(tpa))
            throw new InvalidOperationException("TPA list not available");

        return tpa.Split(Path.PathSeparator);
    }
    
    
    
}