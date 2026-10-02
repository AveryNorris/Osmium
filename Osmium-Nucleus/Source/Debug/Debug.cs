using System.Reflection;

namespace OsmiumNucleus;



public static class Debug
{



    static Debug() {
        RegisterCategory("NUCLEUS");
    }
    
    
        
    //Whether to throw error exceptions
    public static bool ThrowErrors { get; set; } = false;
    //Whether to write to console
    public static bool WriteToConsole { get; set; } = true;

    public static event Action<DebugMessage>? OnMessage;
    
    public static event Action? OnClear;
    
    public const string DefaultCategory = "GENERAL";
    
    private static Dictionary<Assembly, string> CachedCategories = [];
    
    
    
    public static void Log(object Message) => Debug.Message(new DebugMessage(FindCategory(Assembly.GetCallingAssembly()), Message.ToString(), [], [], MessageType.Info));
    
    public static void Log(object Message, string[] DetailInfo, object[] Details) => Debug.Message(new DebugMessage(FindCategory(Assembly.GetCallingAssembly()), Message.ToString(), DetailInfo, Details, MessageType.Info));
    
    public static void Warning(object Message) => Debug.Message(new DebugMessage(FindCategory(Assembly.GetCallingAssembly()), Message.ToString(), [], [], MessageType.Warning));
    
    public static void Warning(object Message, string[] DetailInfo, object[] Details) => Debug.Message(new DebugMessage(FindCategory(Assembly.GetCallingAssembly()), Message.ToString(), DetailInfo, Details, MessageType.Warning));
    
    public static void Error(object Message) => Debug.Message(new DebugMessage(FindCategory(Assembly.GetCallingAssembly()), Message.ToString(), [], [], MessageType.Error));
    
    public static void Error(object Message, string[] DetailInfo, object[] Details) => Debug.Message(new DebugMessage(FindCategory(Assembly.GetCallingAssembly()), Message.ToString(), DetailInfo, Details, MessageType.Error));
    
    public static void Fatal(object Message) => Debug.Message(new DebugMessage(FindCategory(Assembly.GetCallingAssembly()), Message.ToString(), [], [], MessageType.Fatal));
    
    public static void Fatal(object Message, string[] DetailInfo, object[] Details) => Debug.Message(new DebugMessage(FindCategory(Assembly.GetCallingAssembly()), Message.ToString(), DetailInfo, Details, MessageType.Fatal));
    
    public static void RegisterCategory(string __category) {
        if (__category == null) { Debug.Error("Cannot register a null category!"); return; }
        
        CachedCategories[Assembly.GetCallingAssembly()] = __category;
    }

    public static string FindCategory(Assembly __callingAssembly) {
        if (CachedCategories.TryGetValue(__callingAssembly, out string? category))
        {
            return category;
        }
        
        return DefaultCategory;
    }
    


    public static void Clear() {
        OnClear?.Invoke();
        
        if(WriteToConsole) Console.Clear();
    }
    
    public static void Message(DebugMessage message) {
        OnMessage?.Invoke(message);
        
        if(WriteToConsole) Console.WriteLine(message.IsError || message.IsFatal ? message.ToFullString() : message.ToSimpleString());
        if((ThrowErrors && message.IsError) || message.IsFatal) Osmium.Close();
    }

    public static void CollectVirtualization() {
        foreach (Assembly assembly in CachedCategories.Keys.ToArray()) if (assembly.IsCollectible) CachedCategories.Remove(assembly);

        if (OnMessage != null) foreach (Action<DebugMessage> subscriber in OnMessage.GetInvocationList()) { if (subscriber.GetMethodInfo().IsCollectible) OnMessage -= subscriber; }
        if (OnClear != null) foreach (Action subscriber in OnClear.GetInvocationList()) { if (subscriber.GetMethodInfo().IsCollectible) OnClear -= subscriber; }
        
        
    }
    
    
    
}