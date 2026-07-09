using System.Collections.Frozen;
using System.Reflection;
using System.Runtime.CompilerServices;



namespace OsmiumNucleus;



/// <summary> Main class of Osmium, allows you to Create() scenes and Initialize() the game </summary>
/// <author> Avery Norris </author>
#nullable enable
public static partial class Osmium
{
    
    
    
    /// <summary> Current Version of Osmium </summary>
    public const string Version = "1.3";



    /// <summary> Bottom class of Osmium. Contains the OpenTK Instance. </summary>
    public static Window Window => Bedrock.window;

    
    
    /// <summary> List of all scenes currently loaded in the kernel. </summary>
    public static IReadOnlySet<Scene> Scenes => _scenes;
    [MarkerAttributes.UnsafeInternal] internal static readonly HashSet<Scene> _scenes = [];


    
    /// <summary> Displays if Osmium has Started or not. </summary>
    public static bool IsInitialized { get; private set; }
    /// <summary> Displays if the update loop is active.</summary>
    public static bool IsRunning { get; private set; }
    /// <summary> Displays if Osmium has closed or not. </summary>
    public static bool IsClosed { get; private set; }
    /// <summary> Displays if Osmium is running virtually </summary>
    public static bool IsVirtualized { get; private set; }


    /// <summary> If Osmium crashes, with SafeEscape enabled, it will still allow exiting statements to be run </summary>
    public static bool SafeEscape = true;
    
    
    
    /// <summary> The seconds since the last type of frame event. For instance if Draw was just called, DeltaTime would currently reflect the time since the
    /// last DRAW call, even though there was an Update call in between. Use this for framerate independent logic!</summary>
    public static float DeltaTime => Bedrock.DeltaTime;
    
    
    
    /// <summary> Is called when a new <see cref="Scene"/> is added! </summary>
    public static event Action<Scene>? SceneAdded;
    /// <summary> Is called when a new <see cref="Scene"/> is removed! </summary>
    public static event Action<Scene>? SceneRemoved;
    
    
    
    
    /// <summary> Starts up Osmium, and resolves all the Components in the App Domain! Must be called before Run() and Osmium is not guaranteed to work until you call this method! </summary>
    /// <errors> Osmium cannot be initialized again! And it cannot be initialized if you have already called Close() </errors>
    public static void Initialize() {
        if (IsClosed) { Debug.Error("Osmium is already closed!"); return; }
        if (IsRunning) { Debug.Error("Osmium is already Running!"); return; }
        if (IsInitialized) { Debug.Error("Osmium has already Started!"); return; }
        
        IsInitialized = true;
        
        Bedrock.Run();
        
        EventManager.ResolveAllModules();
        
        foreach (IModule module in EventManager._LeadingModuleReferences) module.Initialize();
        
        Debug.Action("Successfully Initialized Osmium!");
    }
    
    
    
    /// <summary> Begins the update loop and sends Load() to all Components! </summary>
    /// <errors>Osmium must be initialized before calling this method.The given name cannot be null. And Osmium cannot have a Scene by the same name! </errors>
    public static void Run() {
        if(!IsInitialized) { Debug.Error("Osmium has not been Initialized yet!"); return; }
        if(IsRunning) { Debug.Error("Osmium is already Running!"); return; }
        if(IsClosed) { Debug.Error("Osmium has already been closed!"); return; }
        
        IsRunning = true;
        
        Debug.Action("Beginning Update Loop!");

        foreach (IModule module in EventManager._LeadingModuleReferences) module.Run();
        Window!.Run();
    }
    
    
    
    /// <summary> Closes Osmium and ends the OpenTK instance! </summary>
    /// <errors>Osmium must be initialized and running before calling this method. And Osmium cannot close after it has been closed! </errors>
    public static void Close() {
        if(IsClosed) { Debug.Error("Osmium has already been closed!"); return; }

        IsClosed = true;
        IsRunning = false;
        IsInitialized = false;
        
        Debug.Log("Closing Osmium!");
        
        foreach (IModule module in EventManager._LeadingModuleReferences) module.Close();
        Window!.Close();
    }
    
    
    
    /// <summary> Initializes the Context and marks Osmium as initialized; but does not Resolve types </summary>
    /// <remarks> This is part of the Editor pipeline! It has no error checking, and it is made explicitly for Radium! So don't use it unless you know what you are doing.
    /// These methods are made required in order to use Virtualization! Use Editor Methods instead of normal ones for Virtualization to work.</remarks>
    [MarkerAttributes.UnsafePipeline]
    public static void EditorInitialize() {
        if (IsRunning) { Debug.Error("Osmium is already Running!"); return; }
        if (IsInitialized) { Debug.Error("Osmium has already Started!"); return; }
        
        IsInitialized = true;
        
        Debug.Action("Successfully Initialized Osmium!");
    }
    
    
    
    /// <summary> Starts OpenTK but doesn't let the update loop run! </summary>
    /// <remarks> This is part of the Editor pipeline! It has no error checking, and it is made explicitly for Radium! So don't use it unless you know what you are doing.
    /// These methods are made required in order to use Virtualization! Use Editor Methods instead of normal ones for Virtualization to work.</remarks>
    [MarkerAttributes.UnsafePipeline]
    public static void EditorRun() {
        if(!IsInitialized) { Debug.Error("Osmium has not been Initialized yet!"); return; }
        if(IsRunning) { Debug.Error("Osmium is already Running!"); return; }
        
        Window!.Run();
    }
    
    
    
    /// <summary> Pretends to initialize Osmium, and makes the Components think that the Game has just been initialized. </summary>
    /// <remarks> This is part of the Editor pipeline! It has no error checking, and it is made explicitly for Radium! So don't use it unless you know what you are doing.
    /// If you do want to use it, use the EditorInitialize() EditorRun() and EditorClose() instead of the traditional methods!</remarks>
    [MarkerAttributes.UnsafePipeline]
    public static void VirtualInitialize(IEnumerable<Assembly> __assemblies) {
        EventManager._TypeAssociatedTimeEvents = FrozenDictionary<Type, EventManager.EventProfile>.Empty;
        EventManager._LeadingModuleReferences.Clear();
        IsInitialized = true;
        IsVirtualized = true;
        
        EventManager.ResolveAllModules(__assemblies);
        
        foreach (IModule module in EventManager._LeadingModuleReferences) module.Initialize();
    }
    
    
    
    /// <summary> Pretends to run Osmium virtually, and makes the Components think it has Started. </summary>
    /// <remarks> This is part of the Editor pipeline! It has no error checking, and it is made explicitly for Radium! So don't use it unless you know what you are doing.
    /// If you do want to use it, use the EditorInitialize() EditorRun() and EditorClose() instead of the traditional methods!</remarks>
    [MarkerAttributes.UnsafePipeline]
    public static void VirtualRun() {
        
        IsRunning = true;
        
        foreach (IModule module in EventManager._LeadingModuleReferences) module.Run();
        foreach (Scene scene in Scenes) scene.ChainEvent(0); 
    }
    
    
    
    /// <summary> Pretends to close Osmium, and makes the Components think that the Game has ended. </summary>
    /// <remarks> This is part of the Editor pipeline! It has no error checking, and it is made explicitly for Radium! So don't use it unless you know what you are doing.
    /// If you do want to use it, use the EditorInitialize() EditorRun() and EditorClose() instead of the traditional methods!</remarks>
    [MarkerAttributes.UnsafePipeline]
    public static void VirtualClose() {
        
        foreach (Scene scene in Scenes) scene.ChainEvent(Event.Unload); 
        
        IsRunning = false;
        IsVirtualized = false;
        
        foreach (IModule module in EventManager._LeadingModuleReferences) module.Close();
    }
    
    
    
    

    /// <summary> Creates a new <see cref="Scene"/> with the given name, returns null if creating it fails </summary>
    /// <param name="__name"> The name of the new Scene and its unique identifier as a <see cref="string"/>; cannot match up with any other names in the scene.</param>
    /// <returns> The <see cref="Scene"/> you created, or null if there is a failure. </returns>
    /// <errors>Osmium must be initialized before calling this method.The given name cannot be null. And Osmium cannot have a Scene by the same name! </errors>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.Low), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.O1)]
    public static Scene? AddScene(string __name) {
        if(!IsInitialized) { Debug.Error("Osmium has not been Initialized yet!"); return null; }
        if (__name == null) { Debug.Error("A Scene cannot have a null name!"); return null; }
        if (ContainsScene(__name)) { Debug.Error("A Scene with the given name already Exists!"); return null; }

        Scene newScene = new (__name);
        _scenes.Add(newScene);
        
        SceneAdded?.Invoke(newScene);
        return newScene;
    }

    
    
    /// <summary> Adds a new <see cref="Scene"/> to Osmium! </summary>
    /// <param name="__scene"> The <see cref="Scene"/> you want to add </param>
    /// <errors> Osmium must be initialized before calling this method. The given Scene cannot be null. And Osmium cannot have a Scene by the same name! </errors>
    /// <remarks> Using this is only recommended for custom types of Scene! If you wish to Spawn in a normal scene, use <see cref="AddScene(string)"/> </remarks>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.Low), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.O1)]
    public static void AddScene(Scene __scene) {
        if(!IsInitialized) { Debug.Error("Osmium has not been Initialized yet!"); return; }
        
        _scenes.Add(__scene);
        SceneAdded?.Invoke(__scene);
    }



    /// <summary> Finds a <see cref="Scene"/> from the given name. </summary>
    /// <param name="__name"> The name of the <see cref="Scene"/> you want to find </param>
    /// <returns> The newly found <see cref="Scene"/>! Or null if there currently isn't one </returns>
    /// <errors> Osmium must be initialized before calling this method. The given Scene cannot be null. And Osmium cannot have a Scene by the same name! </errors>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.Medium),
     MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public static Scene? GetScene(string __name) {
        if(!IsInitialized)  { Debug.Error("Osmium has not been Initialized yet!"); return null; }
        if(__name == null) { Debug.Error("A Scene cannot have a null name!"); return null; }
        
        return _scenes.FirstOrDefault(scene => scene.Name == __name);
    }



    /// <summary> Tells you whether a <see cref="Scene"/> is currently loaded or not </summary>
    /// <param name="__name"> The name of the <see cref="Scene"/> you are searching for </param>
    /// <returns> A <see cref="bool"/> that is either true or false depending on if a scene with the given name is loaded or not</returns>
    /// <errors> Osmium must be initialized before calling this method. The given name cannot be null. </errors>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.Medium),
     MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public static bool ContainsScene(string __name) {
        if(!IsInitialized)  { Debug.Error("Osmium has not been Initialized yet!"); return false; }
        if(__name == null) { Debug.Error("A Scene cannot have a null name!"); return false; }
        
        return _scenes.Any(scene => scene.Name == __name);
    }



    /// <summary> Tells you whether a <see cref="Scene"/> is currently loaded or not </summary>
    /// <param name="__scene"> A reference to the <see cref="Scene"/> you are looking for </param>
    /// <returns> A <see cref="bool"/> that is either true or false depending on if a scene with the given name is loaded or not</returns>
    /// <errors> Osmium must be initialized before calling this method. The given name cannot be null. </errors>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.Medium),
     MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.O1)]
    public static bool ContainsScene(Scene __scene) {
        if(!IsInitialized)  { Debug.Error("Osmium has not been Initialized yet!"); return false; }
        if(__scene == null) { Debug.Error("A Scene cannot be null!"); return false; }
        
        return _scenes.Contains(__scene);
    }
    
    
    
    /// <summary> Removes a currently loaded <see cref="Scene"/> from Osmium from a given name</summary>
    /// <param name="__name"> The name of the scene you want to remove </param>
    /// <errors> Osmium must be initialized before calling this method. The given Name cannot be null. And the given Scene must exist </errors>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.Medium), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public static void RemoveScene(string __name) {
        if(!IsInitialized)  { Debug.Error("Osmium has not been Initialized yet!"); return; }
        if(__name == null) { Debug.Error("A Scene cannot have a null name!"); return; }
        if(!ContainsScene(__name)) { Debug.Error("The given scene does not exist!"); return; }
        
        Scene scene = GetScene(__name)!;
        _scenes.Remove(scene);
        SceneRemoved?.Invoke(scene);
    }



    /// <summary> Removes a currently loaded <see cref="Scene"/> from Osmium </summary>
    /// <param name="__scene"> The scene to remove </param>
    /// <errors> Osmium must be initialized before calling this method. The given Scene cannot be null. And the given Scene must exist </errors>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.Medium), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.O1)]
    public static void RemoveScene(Scene __scene) {
        if(!IsInitialized)  { Debug.Error("Osmium has not been Initialized yet!"); return; }
        if(__scene == null) { Debug.Error("A given scene cannot be null!"); return; }
        if(!ContainsScene(__scene)) { Debug.Error("The given scene does not exist!"); return; }
        
        _scenes.Remove(__scene);
        SceneRemoved?.Invoke(__scene);
    }



    /// <summary> Starts a coroutine </summary>
    /// <inheritdoc cref="CoroutineRunner.Start"/>
    [MarkerAttributes.MethodLambda]
    public static void StartCoroutine(IEnumerator<ICoroutineAction> __coroutine) => CoroutineRunner.Start(__coroutine);
    
    
    
}