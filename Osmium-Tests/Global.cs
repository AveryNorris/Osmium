using OsmiumNucleus;

[assembly: NonParallelizable, LevelOfParallelism(1)]

[SetUpFixture]
public class GlobalSetup
{

    [OneTimeSetUp]
    public void SetUp() {
        Osmium.HeadlessInitialize();
    }

}