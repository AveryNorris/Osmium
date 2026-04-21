using System.Collections.Frozen;
using System.Reflection;
using System.Runtime.CompilerServices;



namespace OsmiumNucleus;



/// <summary> Main class of Osmium, allows you to Create() scenes and Initialize() the game </summary>
/// <author> Avery Norris </author>
#nullable enable
public static class Osmium
{
    
    
    
    /// <summary> Current Version of Osmium </summary>
    public const string Version = "1.3";



    /// <summary> Bottom class of Osmium. Contains the OpenTK Instance. </summary>
    public static Context? Context { get; internal set; }

    
    
    /// <summary> List of all scenes currently loaded in the kernel. </summary>
    public static IReadOnlySet<Scene> Scenes => _scenes;
    [MarkerAttributes.UnsafeInternal] internal static readonly HashSet<Scene> _scenes = [];


    
    /// <summary> Displays if Osmium has Started or not. </summary>
    public static bool IsInitialized { get; internal set; }
    /// <summary> Displays if the update loop is active.</summary>
    public static bool IsRunning { get; internal set; }
    /// <summary> Displays if Osmium has closed or not. </summary>
    public static bool IsClosed { get; internal set; }
    
    
    
    /// <summary> The amount of time since the last type of a frame event. For instance if Draw was just called, DeltaTime would currently reflect the time since the
    /// last DRAW call, even though there was an Update call in between. Use this for framerate independent logic!</summary>
    public static double DeltaTime { get; internal set; } = 0;
    
    
    
    
    /// <summary> Starts up Osmium, and resolves all the Components in the App Domain! Must be called before Run() and Osmium is not guaranteed to work until you call this method! </summary>
    /// <errors> Osmium cannot be initialized again! And it cannot be initialized if you have already called Close() </errors>
    public static void Initialize() {
        if (IsClosed) { Debug.LogError("Osmium is already closed!"); return; }
        if (IsRunning) { Debug.LogError("Osmium is already Running!"); return; }
        if (IsInitialized) { Debug.LogError("Osmium has already Started!"); return; }
        
        IsInitialized = true;
        
        //initialize here to allow any required access before Run()
        Context = new Context();

        EventManager.ResolveAllModules();
        
        Debug.LogAction("Successfully Initialized Osmium!");
    }
    
    
    
    /// <summary> Begins the update loop and sends Load() to all Components! </summary>
    /// <errors>Osmium must be initialized before calling this method.The given name cannot be null. And Osmium cannot have a Scene by the same name! </errors>
    public static void Run() {
        if(!IsInitialized) { Debug.LogError("Osmium has not been Initialized yet!"); return; }
        if(IsRunning) { Debug.LogError("Osmium is already Running!"); return; }
        if(IsClosed) { Debug.LogError("Osmium has already been closed!"); return; }
        
        IsRunning = true;
        
        Debug.LogAction("Beginning Update Loop!");
        Context!.Run();
    }
    
    
    
    /// <summary> Closes Osmium and ends the OpenTK instance! </summary>
    /// <errors>Osmium must be initialized and running before calling this method. And Osmium cannot close after it has been closed! </errors>
    public static void Close() {
        if(!IsInitialized) { Debug.LogError("Osmium has not been Initialized yet!"); return; }
        if(!IsRunning) { Debug.LogError("Osmium is already not Running!"); return; }
        if(IsClosed) { Debug.LogError("Osmium has already been closed!"); return; }

        IsClosed = true;
        IsRunning = false;
        IsInitialized = false;
        
        Context!.Close();
    }
    
    
    
    /// <summary> Initializes the Context and marks Osmium as initialized; but does not Resolve types </summary>
    /// <remarks> This is part of the Editor pipeline! It has no error checking, and it is made explicitly for Radium! So don't use it unless you know what you are doing.
    /// These methods are made required in order to use Virtualization! Use Editor Methods instead of normal ones for Virtualization to work.</remarks>
    [MarkerAttributes.EditorPipeline]
    public static void EditorInitialize() {
        if (IsRunning) { Debug.LogError("Osmium is already Running!"); return; }
        if (IsInitialized) { Debug.LogError("Osmium has already Started!"); return; }
        
        IsInitialized = true;
        
        Context = new Context();
        
        Debug.LogAction("Successfully Initialized Osmium!");
    }
    
    
    
    /// <summary> Starts OpenTK but doesn't let the update loop run! </summary>
    /// <remarks> This is part of the Editor pipeline! It has no error checking, and it is made explicitly for Radium! So don't use it unless you know what you are doing.
    /// These methods are made required in order to use Virtualization! Use Editor Methods instead of normal ones for Virtualization to work.</remarks>
    [MarkerAttributes.EditorPipeline]
    public static void EditorRun() {
        if(!IsInitialized) { Debug.LogError("Osmium has not been Initialized yet!"); return; }
        if(IsRunning) { Debug.LogError("Osmium is already Running!"); return; }
        
        Context!.Run();
    }
    
    
    
    /// <summary> Pretends to initialize Osmium, and makes the Components think that the Game has just been initialized. </summary>
    /// <remarks> This is part of the Editor pipeline! It has no error checking, and it is made explicitly for Radium! So don't use it unless you know what you are doing.
    /// If you do want to use it, use the EditorInitialize() EditorRun() and EditorClose() instead of the traditional methods!</remarks>
    [MarkerAttributes.EditorPipeline]
    public static void VirtualInitialize(IEnumerable<Assembly> __assemblies) {
        EventManager._TypeAssociatedTimeEvents = FrozenDictionary<Type, Action<Component>[]>.Empty;
        IsInitialized = true;
        
        EventManager.ResolveAllModules(__assemblies);
    }
    
    
    
    /// <summary> Pretends to run Osmium virtually, and makes the Components think it has Started. </summary>
    /// <remarks> This is part of the Editor pipeline! It has no error checking, and it is made explicitly for Radium! So don't use it unless you know what you are doing.
    /// If you do want to use it, use the EditorInitialize() EditorRun() and EditorClose() instead of the traditional methods!</remarks>
    [MarkerAttributes.EditorPipeline]
    public static void VirtualRun() {
        
        IsRunning = true;
        
        //send a fake load call to simulate loading
        foreach (Scene scene in Scenes) scene.ChainEvent(0); 
    }
    
    
    
    /// <summary> Pretends to close Osmium, and makes the Components think that the Game has ended. </summary>
    /// <remarks> This is part of the Editor pipeline! It has no error checking, and it is made explicitly for Radium! So don't use it unless you know what you are doing.
    /// If you do want to use it, use the EditorInitialize() EditorRun() and EditorClose() instead of the traditional methods!</remarks>
    [MarkerAttributes.EditorPipeline]
    public static void VirtualClose() {
        
        foreach (Scene scene in Scenes) scene.ChainEvent(1); 
        
        IsRunning = false;
    }
    
    
    
    

    /// <summary> Creates a new <see cref="Scene"/> with the given name, returns null if creating it fails </summary>
    /// <param name="__name"> The name of the new Scene and its unique identifier as a <see cref="string"/>; cannot match up with any other names in the scene.</param>
    /// <returns> The <see cref="Scene"/> you created, or null if there is a failure. </returns>
    /// <errors>Osmium must be initialized before calling this method.The given name cannot be null. And Osmium cannot have a Scene by the same name! </errors>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.Low), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.O1)]
    public static Scene? AddScene(string __name) {
        if(!IsInitialized) { Debug.LogError("Osmium has not been Initialized yet!"); return null; }
        if (__name == null) { Debug.LogError("A Scene cannot have a null name!"); return null; }
        if (ContainsScene(__name)) { Debug.LogError("A Scene with the given name already Exists!"); return null; }

        Scene newScene = new (__name);
        _scenes.Add(newScene);
        return newScene;
    }
    
    
    
    /// <summary> Adds a new <see cref="Scene"/> to Osmium! </summary>
    /// <param name="__scene"> The <see cref="Scene"/> you want to add </param>
    /// <errors> Osmium must be initialized before calling this method. The given Scene cannot be null. And Osmium cannot have a Scene by the same name! </errors>
    /// <remarks> Using this is only recommended for custom types of Scene! If you wish to Spawn in a normal scene, use <see cref="AddScene(string)"/> </remarks>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.Low), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.O1)]
    public static void AddScene(Scene __scene) {
        if(!IsInitialized) { Debug.LogError("Osmium has not been Initialized yet!"); return; }
        if (__scene == null) { Debug.LogError("The given Scene is null!"); return; }
        if (ContainsScene(__scene.Name)) { Debug.LogError("A Scene with the given name already Exists!"); return; }
        
        _scenes.Add(__scene);
    }



    /// <summary> Finds a <see cref="Scene"/> from the given name. </summary>
    /// <param name="__name"> The name of the <see cref="Scene"/> you want to find </param>
    /// <returns> The newly found <see cref="Scene"/>! Or null if there currently isn't one </returns>
    /// <errors> Osmium must be initialized before calling this method. The given Scene cannot be null. And Osmium cannot have a Scene by the same name! </errors>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.Medium),
     MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public static Scene? GetScene(string __name) {
        if(!IsInitialized)  { Debug.LogError("Osmium has not been Initialized yet!"); return null; }
        if(__name == null) { Debug.LogError("A Scene cannot have a null name!"); return null; }
        
        return _scenes.FirstOrDefault(scene => scene.Name == __name);
    }



    /// <summary> Tells you whether a <see cref="Scene"/> is currently loaded or not </summary>
    /// <param name="__name"> The name of the <see cref="Scene"/> you are searching for </param>
    /// <returns> A <see cref="bool"/> that is either true or false depending on if a scene with the given name is loaded or not</returns>
    /// <errors> Osmium must be initialized before calling this method. The given name cannot be null. </errors>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.Medium),
     MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public static bool ContainsScene(string __name) {
        if(!IsInitialized)  { Debug.LogError("Osmium has not been Initialized yet!"); return false; }
        if(__name == null) { Debug.LogError("A Scene cannot have a null name!"); return false; }
        
        return _scenes.Any(scene => scene.Name == __name);
    }
    
    
    
    /// <summary> Removes a currently loaded <see cref="Scene"/> from Osmium from a given name</summary>
    /// <param name="__name"> The name of the scene you want to remove </param>
    /// <errors> Osmium must be initialized before calling this method. The given Name cannot be null. And the given Scene must exist </errors>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.Medium), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public static void RemoveScene(string __name) {
        if(!IsInitialized)  { Debug.LogError("Osmium has not been Initialized yet!"); return; }
        if(__name == null) { Debug.LogError("A Scene cannot have a null name!"); return; }
        if(!ContainsScene(__name)) { Debug.LogError("The given scene does not exist!"); return; }

        _scenes.Remove(GetScene(__name)!);
    }



    /// <summary> Removes a currently loaded <see cref="Scene"/> from Osmium </summary>
    /// <param name="__scene"> The scene to remove </param>
    /// <errors> Osmium must be initialized before calling this method. The given Scene cannot be null. And the given Scene must exist </errors>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.Medium), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public static void RemoveScene(Scene __scene) {
        if(!IsInitialized)  { Debug.LogError("Osmium has not been Initialized yet!"); return; }
        if(__scene == null) { Debug.LogError("A given scene cannot be null!"); return; }
        if(!ContainsScene(__scene.Name)) { Debug.LogError("The given scene does not exist!"); return; }
        
        _scenes.Remove(__scene);
    }



}