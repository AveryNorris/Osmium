using System.Runtime.Loader;

namespace OsmiumEditor;

public static partial class Editor
{
    public static readonly AssemblyLoadContext _EditorModules = new AssemblyLoadContext(null, false);

    public static AssemblyLoadContext _RuntimeModules = new AssemblyLoadContext(null, true);
}