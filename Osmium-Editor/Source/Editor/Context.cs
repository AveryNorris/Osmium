using System.Reflection;
using System.Runtime.Loader;
using OsmiumNucleus;

namespace OsmiumEditor;

public static partial class Editor
{
    public static readonly AssemblyLoadContext _Modules = new AssemblyLoadContext(null, false);

}