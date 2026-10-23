using OsmiumNucleus;

public class GlobalSetup
{
    [OneTimeSetUp]
    public void OneTimeSetup() {
        Osmium.Initialize();
        
    }
}