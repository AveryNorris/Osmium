using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Linq.Expressions;
using System.Reflection;


namespace OsmiumNucleus;


/// <summary> Registers <see cref="Component"/> events for Osmium! Builds them into a hyper fast dictionary for quick access. </summary>
/// <author> Avery Norris </author>
internal static class EventManager
{
    
    
    
    /// <summary> Holds an associated action for each component and a time event. Is built with ResolveTypeEvents() during Initialize().
    /// There is little to no error checking; it is assumed that any Component trying to index this dictionary, at least has null at a key/value! There cannot be empty
    /// entries even if a Component receives no events. </summary>
    [MarkerAttributes.UnsafeInternal]
    internal static FrozenDictionary<Type, Action<Component>[]> _TypeAssociatedTimeEvents;
    
    
    
    /// <summary> Holds an associated bool for each Component type, which tells whether they are allowed to receive events, even if Osmium is runnning virtually </summary>
    [MarkerAttributes.UnsafeInternal]
    internal static FrozenDictionary<Type, bool> _TypeAssociatedVirtualEventPrivileges;
    
    
    
    /// <summary> All types of time based events in Osmium. Stored at their respective ID's by index.</summary>
    internal static readonly ImmutableArray<string> Events = [
        "Load", "Unload", "Update", "Draw", "Create", "Remove"
    ];
    
    
    
    /// <summary> Searches all assemblies in the current App Domain; and automatically resolves all Components with a given type! This is necessary for Osmium's function
    /// and part of the reason why Initialize() must be called so early.</summary>
    [MarkerAttributes.UnsafeInternal]
    internal static void ResolveAllModules() {
        ResolveAllModules(AppDomain.CurrentDomain.GetAssemblies());
        Debug.LogAction("Finished Resolving!");
    }
    
    
    
    /// <summary> Resolves all the components from only the given assemblies</summary>
    [MarkerAttributes.EditorPipeline]
    internal static void ResolveAllModules(IEnumerable<Assembly> __sources) {
        
        Dictionary<Type, Action<Component>[]> _newAssociatedTimeEvents = [];
        Dictionary<Type, bool> _newAssociatedVirtualEventPrivileges = [];
        
        foreach (Assembly assembly in __sources) {
            foreach (Type type in assembly.GetTypes()) {
                if (!type.IsSubclassOf(typeof(Component))) continue;

                List<Action<Component>> timeEvents = [];

                foreach (MethodInfo eventMethod in Events.Select(type.GetMethod)) {
                    if (eventMethod == null) {
                        timeEvents.Add(null);
                        continue;
                    }

                    //create a new delegate expression that calls the Components associated method.
                    ParameterExpression ComponentInstanceParameter =
                            Expression.Parameter(typeof(Component), "__component");
                    UnaryExpression Casting = Expression.Convert(ComponentInstanceParameter, type);
                    MethodCallExpression Call = Expression.Call(Casting, eventMethod);
                    Expression<Action<Component>> Lambda =
                            Expression.Lambda<Action<Component>>(Call, ComponentInstanceParameter);
                    timeEvents.Add(Lambda.Compile());
                }



                Debug.LogAction("Found and Resolved events attached to : " + type.Name + " In " + type.Namespace);

                _newAssociatedTimeEvents.Add(type, timeEvents.ToArray());
                _newAssociatedVirtualEventPrivileges.Add(type, type.GetCustomAttribute<NonVirtual>() != null);
            }
        }
        
        _TypeAssociatedTimeEvents = _newAssociatedTimeEvents.ToFrozenDictionary();
        _TypeAssociatedVirtualEventPrivileges = _newAssociatedVirtualEventPrivileges.ToFrozenDictionary();
    }
    
    
    
}