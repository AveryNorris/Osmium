using System.Diagnostics;

namespace OsmiumNucleus;


//todo: struct?
public class DebugMessage(string category, string message, string[] detailInfo, object[] details, MessageType __type)
{
    
    public virtual MessageType MessageType => __type;
    
    public virtual bool IsFatal => MessageType.HasFlag(MessageType.Fatal);
    public virtual bool IsError => MessageType.HasFlag(MessageType.Error);
    public virtual bool IsWarning => MessageType.HasFlag(MessageType.Warning);
    public virtual bool IsInfo => MessageType.HasFlag(MessageType.Info);

    public virtual string[] DetailInfo => detailInfo;
    public virtual object[] Details => details;

    public virtual string Category => category;
    public virtual string Message => message;

    public virtual DateTime Time => DateTime.Now;
    public virtual StackTrace CallStack => new StackTrace(3);

    public virtual string ToFullString() {
        string result = "";
        
        if(IsFatal) result += "FATAL :";
        if(IsError) result += "ERROR :";
        if(IsWarning) result += "WARNING :";
        if(IsInfo) result += "INFO :";
        
        if(category.Length > 0) result += $" {Category} - ";
        
        result += Message;
        
        for (int i = 0; i < Math.Min(Details.Length, DetailInfo.Length); i++) {
            result += $"\n {DetailInfo[i]} - \"{Details[i]}\"";
        }

        result += $"\n :: {Time.ToShortTimeString()} :: {CallStack.ToString()}";
        
        return result;
    }

    public virtual string ToSimpleString() {
        string result = "";
        
        if(IsFatal) result += "FATAL :";
        if(IsError) result += "ERROR :";
        if(IsWarning) result += "WARNING :";
        if(IsInfo) result += "INFO :";
        
        if(category.Length > 0) result += $" {Category} - ";
        
        result += Message;

        for (int i = 0; i < Math.Min(Details.Length, DetailInfo.Length); i++) {
            result += $"\n {DetailInfo[i]} - \"{Details[i]}\"";
        }

        return result;
    }

    public override string ToString() => ToSimpleString();
}