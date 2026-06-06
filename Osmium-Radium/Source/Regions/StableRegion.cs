using OsmiumRadium;



/// <summary> The base region where all children immediate elements and region are stored </summary>
public class StableRegion : Region
{
    public void Clear() {
        _children.Clear();
    }
    
    protected internal override void SetClipping() => Backend.UploadClippingUniform(Rect.FullScreen);

}