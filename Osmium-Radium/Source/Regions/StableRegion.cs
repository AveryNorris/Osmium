using OsmiumRadium;



/// <summary> The base region where all children immediate elements and region are stored </summary>
public class StableRegion : Region
{
    public void Clear() {
        _children.Clear();
    }
    
    protected internal override void Draw() {
        for (int i = 0; i < _children.Count; i++) {
            Backend.UploadClippingUniform(new Bounds(size: new Vector2(100,100)));
            
            _children[i].Draw();
        }
    }
}