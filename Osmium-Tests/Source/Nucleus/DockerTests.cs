using OsmiumNucleus;

namespace OsmiumTests;

[TestFixture]
public class DockerTests
{

    public Scene TestScene;

    public const int MultipleTestCount = 100;
    
    [SetUp]
    public void SetUp() {
        TestScene = Osmium.AddScene("Test");
        
    }

    [TearDown]
    public void TearDown() {
        Osmium.RemoveScene(TestScene);
    }
    
    [Test]
    public void AddingGameObject() {
        TestScene.Add<Empty>();

        Assert.That(TestScene.Contains<Empty>(), Is.True);
    }
    
    [Test]
    public void DestroyingGameObject() {
        TestScene.Add<Empty>();
        TestScene.Destroy<Empty>();

        Assert.That(TestScene.Contains<Empty>(), Is.False);
    }

    [Test]
    public void Enumerating() {
        for (int i = 0; i < MultipleTestCount; i++) TestScene.Add<Empty>();

        int count = 0;
        foreach (GameObject obj in TestScene) count++;
        
        Assert.That(count, Is.EqualTo(MultipleTestCount));
    }

}