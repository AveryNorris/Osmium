using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Linq.Expressions;
using System.Reflection;


namespace OsmiumNucleus;


/// <summary> Registers <see cref="Component"/> events for Osmium! Builds them into a hyper fast dictionary for quick access. </summary>
/// <author> Avery Norris </author>
internal static class EventManager
{
    
    
    
    /// <summary> Holds an index of each Component Type and their respective Event Profile,
    /// which contains a callback to all the Components events and information and rules on how to update
    /// the Component </summary>
    [MarkerAttributes.UnsafeInternal]
    internal static FrozenDictionary<Type, EventProfile> _TypeAssociatedTimeEvents = FrozenDictionary<Type, EventProfile>.Empty;
    
    
    /// <summary> Represents a Component's event callbacks and flags/capabilities </summary>
    internal record EventProfile(Action<Component>[] callbacks, bool __alwaysUpdate) {
        public readonly Action<Component>?[] Callbacks = callbacks;
        public readonly bool AlwaysUpdate = __alwaysUpdate;
    }
    
    
    
    /// <summary> All types of time based events in Osmium. Stored at their respective ID's by index.</summary>
    internal static readonly ImmutableArray<string> Events = [
        "Load", "Unload", "Update", "Draw", "Create", "Remove"
    ];

    
    
    /// <summary> Contains an instance of an IModule for all unique IModules found </summary>
    [MarkerAttributes.UnsafeInternal]
    internal static readonly List<IRuntimeModule> _LeadingModuleReferences = [];
    
    
    
    /// <summary> Searches all assemblies in the current App Domain; and automatically resolves all Components with a given type! This is necessary for Osmium's function
    /// and part of the reason why Initialize() must be called so early.</summary>
    [MarkerAttributes.UnsafeInternal]
    internal static void ResolveAllModules() {
        ResolveAllModules(AppDomain.CurrentDomain.GetAssemblies());
        Debug.Action("Finished Resolving!");
    }
    
    
    
    /// <summary> Resolves all the components from only the given assemblies</summary>
    [MarkerAttributes.UnsafePipeline]
    internal static void ResolveAllModules(IEnumerable<Assembly> __sources) {
        
        Dictionary<Type, EventProfile> _newAssociatedTimeEvents = [];
        
        foreach (Assembly assembly in __sources) {
            foreach (Type type in assembly.GetTypes()) {
                
                if (typeof(IRuntimeModule).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                {
                    IRuntimeModule runtimeModule = (IRuntimeModule) Activator.CreateInstance(type);
                    if(runtimeModule != null) _LeadingModuleReferences.Add(runtimeModule);
                }
                
                if (!type.IsSubclassOf(typeof(Component))) continue;

                List<Action<Component>> timeEvents = [];

                foreach (MethodInfo eventMethod in Events.Select(type.GetMethod)) {
                    if (eventMethod == null) {
                        timeEvents.Add(null);
                        continue;
                    }

                    //create a new delegate expression that calls the Components associated method.
                    ParameterExpression ComponentInstanceParameter = Expression.Parameter(typeof(Component), "__component");
                    UnaryExpression Casting = Expression.Convert(ComponentInstanceParameter, type);
                    MethodCallExpression Call = Expression.Call(Casting, eventMethod);
                    Expression<Action<Component>> Lambda = Expression.Lambda<Action<Component>>(Call, ComponentInstanceParameter);
                    timeEvents.Add(Lambda.Compile());
                }
                
                bool alwaysUpdate = type.IsDefined(typeof(AlwaysUpdate), true);
                
                Debug.Action("Found and Resolved events attached to : " + type.Name + " In " + type.Namespace);

                _newAssociatedTimeEvents.Add(type, new EventProfile(timeEvents.ToArray(), alwaysUpdate));
            }
        }
        
        _TypeAssociatedTimeEvents = _newAssociatedTimeEvents.ToFrozenDictionary();
    }
    
    
    
}