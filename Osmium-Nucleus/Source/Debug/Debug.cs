using System.Diagnostics;
using System.Reflection;

namespace OsmiumNucleus;



/// <summary> Handles all Osmium Debugging! Writes to a file in the same directory as the running assembly! </summary>
public static class Debug
{



    /// <summary> True path of the log file Osmium dumps to. </summary>
    public static readonly string LogFilePath;
    
    
    
    /// <summary> Target name of the log file </summary>
    public const string LogFileName = "Log";

    public static OrderedDictionary<DebugMessage, int> Stack = [];
    
        
    //Whether to throw error exceptions
    public static bool ThrowExceptions { get; set; } = false;
    //Whether to debug errors at all
    public static bool DebugErrors { get; set; } = true;
    //Whether to write to console
    public static bool WriteToConsole { get; set; } = true;
    //Whether to write to file
    public static bool WriteToFile { get; set; } = false;
    
    //output of the console
    private static List<string> _debugTable = [];
    
    public static IReadOnlyList<string> Output => _debugTable;
        
        
    /// <summary> The safety level of the Debugger</summary>
    public static SafetyLevel safetyLevel {
        get => _safetyLevel; 
        set {
            ThrowExceptions = value is SafetyLevel.Extreme;
            DebugErrors = value is not SafetyLevel.None;
            _safetyLevel = value;
        }
    } private static SafetyLevel _safetyLevel;

    
    
    /// <summary> Represents the safety level of Osmium! Set to normal by default. </summary>
    public enum SafetyLevel {
        Extreme, //Throw exceptions and stop the whole program
        Normal, //Just debug it to the file, and try to exit safely. 
        Low, //Push through tasks but debug error
        None, //Ignore most/all errors and do not debug it,
    }
        
        
        
    /// <summary> Finds the target debug file and saves it's path.</summary>
    static Debug() {
        string directoryPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? AppContext.BaseDirectory;
        LogFilePath = Path.Join(directoryPath, LogFileName + ".awlf");
        
        File.WriteAllText(LogFilePath, "");
        
        //always write to console if WriteToConsole is true, but don't set it to false if there isn't a direct console.
        if(Console.IsOutputRedirected)
            WriteToConsole = true;
    }




    /// <summary> Sends an action message! </summary>
    public static void Action(string __message) => LogGeneric(new DebugMessage(__message, "ACT", [], [], false));
    /// <summary> Sends an action message! Also sends extra information in the form of objects; the first list is the names of objects to send, and the second
    /// is the actual information at each index.</summary>
    public static void Action(string __message, string[] __params, string[] __values) => LogGeneric(new DebugMessage(__message, "ACT", __params, __values, false));
        
        
    /// <summary> Sends a warning message! </summary>
    public static void Warning(string __message) => LogGeneric(new DebugMessage(__message, "WRN", [], [], false));
    /// <summary> Sends a warning message! Also sends extra information in the form of objects; the first list is the names of objects to send, and the second
    /// is the actual information at each index.</summary>
    public static void Warning(string __message, string[] __params, string[] __values) => LogGeneric(new DebugMessage(__message, "WRN", __params, __values, false));
        
        
    
    /// <summary> Sends an error message! </summary>
    public static void Error(string __message) => LogGeneric(new DebugMessage(__message, "ERR", [], [], true));
    /// <summary> Sends an error message! Also sends extra information in the form of objects; the first list is the names of objects to send, and the second
    /// is the actual information at each index.</summary>
    public static void Error(string __message, string[] __params, string[] __values) => LogGeneric(new DebugMessage(__message, "ERR", __params, __values, true));
    
    
    /// <summary> Logs an error! </summary>
    public static void Log(string __message, string __callSign = "LOG") => LogGeneric(new DebugMessage(__message, __callSign, [],[], false));
    
    public static void Log(string __message, string[] __params, string[] __values, string __callSign = "LOG") => LogGeneric(new DebugMessage(__message, __callSign, __params,__values, false));

        
    /// <summary>Writes the current message to the log file. With any given call sign.</summary>
    public static void LogGeneric(DebugMessage __message) {
        if (__message.Values.Length != __message.Parameters.Length) throw new Exception("Debug Parameters does not match the amount of values!");

        if (!Stack.TryAdd(__message, 1)) {
            Stack[__message]++;
        }

        string output = "\n\n" + __message.CallSign + "- \"" + __message.Message + '\"';
        if (__message.Error) output += "\n         STK-" + new StackTrace();

        for (int i = 0; i < __message.Parameters.Length; i++)
            output += "\n         " + __message.Parameters[i] + "- " + __message.Values.ElementAt(i);

        if (__message.Error && ThrowExceptions) throw new Exception(output);
            
        if(WriteToFile) File.AppendAllText(LogFilePath, output);
        if(WriteToConsole) Console.WriteLine(output);
        
        _debugTable.Add(output);
    }



    public static void Clear() {
        Stack.Clear();
        
        if(WriteToFile) File.WriteAllText(LogFilePath, "");
        if(WriteToConsole) Console.Clear();
        
        _debugTable.Clear();
    }
}

public struct DebugMessage(string __message, string __callSign, string[] __parameters, string[] __values, bool __error)
{
    public string Message = __message;
    public string CallSign = __callSign;
    
    public string[] Parameters = __parameters;
    public string[] Values = __values;
    
    public bool Error = __error;
}