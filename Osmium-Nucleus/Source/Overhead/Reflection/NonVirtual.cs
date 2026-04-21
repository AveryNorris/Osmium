namespace OsmiumNucleus;



/// <summary> Marks and allows a Component to receive events even if Osmium is virtually running and is currently turned off</summary>
[AttributeUsage(AttributeTargets.Class)] 
public class NonVirtual : Attribute;