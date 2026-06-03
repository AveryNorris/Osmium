namespace OsmiumNucleus;



/// <summary> Represents a timed delay in a coroutine, which can be used with yield return.</summary>
/// <param name="duration"> The amount of time before the delay ends </param>
public class Delay(TimeSpan duration) : ICoroutineAction {

    
    
    /// <summary> The amount of time remaining in the delay </summary>
    private TimeSpan Duration = duration;
    
    
    
    public bool Continue() {
        Duration -= TimeSpan.FromSeconds(Osmium.DeltaTime);
        
        return Duration > TimeSpan.Zero;
    }
    
    
    
    /// <summary> Creates a new delay from the given time in milliseconds </summary>
    public static Delay FromMilliseconds(float __milliseconds) => new Delay(TimeSpan.FromMilliseconds(__milliseconds));
    
    /// <summary> Creates a new delay from the given time in seconds </summary>
    public static Delay FromSeconds(float __seconds) => new Delay(TimeSpan.FromSeconds(__seconds));
    
    /// <summary> Creates a new delay from the given time in minutes </summary>
    public static Delay FromMinutes(float __minutes) => new Delay(TimeSpan.FromMinutes(__minutes));
    
    /// <summary> Creates a new delay from the given time in hours </summary>
    public static Delay FromHours(float __hours) => new Delay(TimeSpan.FromHours(__hours));

    /// <summary> Creates a new delay from the given time in days </summary>
    public static Delay FromDays(float __days) => new Delay(TimeSpan.FromDays(__days));
    
    
    
}