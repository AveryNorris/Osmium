using System.Reflection;
using System.Runtime.Loader;
using OsmiumBedrock;
using OsmiumNucleus;

namespace OsmiumEditor;

public static partial class Editor
{
    public static readonly AssemblyLoadContext _Modules = new AssemblyLoadContext(null, false);
    
    public static Assembly[] Assemblies => [.._Modules.Assemblies, typeof(Osmium).Assembly, typeof(Editor).Assembly, typeof(Bedrock).Assembly, ..EditorLoadContext.Assemblies];

}