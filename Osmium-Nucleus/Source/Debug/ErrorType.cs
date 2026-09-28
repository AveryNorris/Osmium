namespace OsmiumNucleus;

[Flags]
public enum MessageType : byte
{
    Info = 1,
    Warning = 2,
    Error = 4,
    Fatal = 8,
}