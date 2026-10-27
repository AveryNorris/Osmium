using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Linq.Expressions;
using System.Reflection;


namespace OsmiumNucleus;


/// <summary> Registers <see cref="GameObject"/> events for Osmium! Builds them into a hyper fast dictionary for quick access. </summary>
/// <author> Avery Norris </author>
internal static class EventManager
{
    
    
    
    /// <summary> Holds an index of each GameObject Type and their respective Event Profile,
    /// which contains a callback to all the Components events and information and rules on how to update
    /// the GameObject </summary>
    internal static FrozenDictionary<Type, EventProfile> _TypeAssociatedTimeEvents = FrozenDictionary<Type, EventProfile>.Empty;
    
    
    /// <summary> Represents a GameObject's event callbacks and flags/capabilities </summary>
    internal record EventProfile(Action<GameObject>[] callbacks, bool __alwaysUpdate) {
        public readonly Action<GameObject>?[] Callbacks = callbacks;
        public readonly bool AlwaysUpdate = __alwaysUpdate;
    }
    
    
    
    /// <summary> All types of time based events in Osmium. Stored at their respective ID's by index.</summary>
    internal static readonly ImmutableArray<string> Events = [
        "Load", "Unload", "Update", "Draw", "Create", "Remove"
    ];



    /// <summary> All Runtime module methods that are using OnInitialize to track startup </summary>
    internal static List<MethodInfo> OnInitializeEvents = [];
    
    
    
    /// <summary> Searches all assemblies in the current App Domain; and automatically resolves all Components with a given type! This is necessary for Osmium's function
    /// and part of the reason why Initialize() must be called so early.</summary>
    internal static void ResolveAllModules() {
        HashSet<Assembly> FoundAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToHashSet();
        foreach (AssemblyName assembly in Assembly.GetEntryAssembly().GetReferencedAssemblies()) FoundAssemblies.Add(Assembly.Load(assembly));
        
        string UserCodePath = Path.Combine(
                AppContext.BaseDirectory,
                ".compilationUserCode.dll"
            );
        
        Debug.Log(UserCodePath);
        
        if (File.Exists(UserCodePath)) {
            FoundAssemblies.Add(Assembly.LoadFile(UserCodePath));
        }

        foreach (Assembly assembly in FoundAssemblies)
        {
            if (assembly.GetName().Name == ".compilationCSProj")
            {
                Debug.Log(string.Join(',' , assembly.GetTypes()));
                foreach (Type type in assembly.GetTypes())
                {
                    Debug.Log(string.Join(',' , type.GetNestedTypes()));
                    Debug.Log("NAME " + type.FullName);
                }
            }
        }
        
        ResolveAllModules(FoundAssemblies);
        Debug.Log("Finished Resolving!");
    }
    
    
    
    /// <summary> Resolves all the components from only the given assemblies</summary>
    internal static void ResolveAllModules(IEnumerable<Assembly> __sources) {
        
        Dictionary<Type, EventProfile> _newAssociatedTimeEvents = [];
        
        foreach (Assembly assembly in __sources) {
            foreach (Type type in assembly.GetTypes()) {

                foreach (MethodInfo eventMethod in type.GetMethods()) {
                    if (eventMethod.IsStatic && eventMethod.GetCustomAttributes(typeof(OnInitialize), true).Length > 0 && eventMethod.GetParameters().Length == 0) {
                        OnInitializeEvents.Add(eventMethod);
                    }
                }
                
                if (!type.IsSubclassOf(typeof(GameObject))) continue;

                List<Action<GameObject>> timeEvents = [];

                foreach (MethodInfo eventMethod in Events.Select(type.GetMethod)) {
                    if (eventMethod == null) {
                        //is this bad?
                        timeEvents.Add(null);
                        continue;
                    }
                    
                    Debug.Log("Found event " + eventMethod.Name + " on " + type.Name);

                    //create a new delegate expression that calls the Components associated method.
                    ParameterExpression ComponentInstanceParameter = Expression.Parameter(typeof(GameObject), "__component");
                    UnaryExpression Casting = Expression.Convert(ComponentInstanceParameter, type);
                    MethodCallExpression Call = Expression.Call(Casting, eventMethod);
                    Expression<Action<GameObject>> Lambda = Expression.Lambda<Action<GameObject>>(Call, ComponentInstanceParameter);
                    timeEvents.Add(Lambda.Compile());
                }
                
                bool alwaysUpdate = type.IsDefined(typeof(AlwaysUpdate), true);
                
                Debug.Log("Found and Resolved events attached to : " + type.Name + " In " + type.Namespace);

                _newAssociatedTimeEvents.Add(type, new EventProfile(timeEvents.ToArray(), alwaysUpdate));
            }
        }
        
        _TypeAssociatedTimeEvents = _newAssociatedTimeEvents.ToFrozenDictionary();
    }



    /// <summary> Resolves all the components from only the given assemblies</summary>
    internal static void InvokeModulesInitializeEvent()
    {
        foreach (MethodInfo method in OnInitializeEvents) {
            method.Invoke(null, null);
        }
    }
}