namespace OsmiumRadium;

public struct Vector4
{
    public float x;
    public float y;
    public float z;
    public float w;

    public Vector4(float x, float y, float z, float w) {
        this.x = x;
        this.y = y;
        this.z = z;
        this.w = w;
    }

    public Vector4(float v) {
        this.x = v;
        this.y = v;
    }

    public static Vector4 operator /(Vector4 v, float divisor) => new Vector4(v.x / divisor, v.y / divisor, v.z / divisor, v.w / divisor);
    
    public static Vector4 operator +(Vector4 v, float value) => new Vector4(v.x + value, v.y + value, v.z + value, v.w + value);
    
    public static Vector4 operator -(Vector4 v, float value) => new Vector4(v.x - value, v.y - value, v.z - value, v.w - value);
    
    public static Vector4 operator *(Vector4 v, float value) => new Vector4(v.x * value, v.y * value, v.z * value, v.w * value);

    
    
    public static Vector4 operator +(Vector4 a, Vector4 b) => new Vector4(a.x + b.x, a.y + b.y, a.z + b.z, a.w + b.w);
    
    public static Vector4 operator -(Vector4 a, Vector4 b) => new Vector4(a.x - b.x, a.y - b.y, a.z - b.z, a.w - b.w);
    
    public static Vector4 operator *(Vector4 a, Vector4 b) => new Vector4(a.x * b.x, a.y * b.y, a.z * b.z, a.w * b.z);
    
    public static Vector4 operator /(Vector4 a, Vector4 b) => new Vector4(a.x / b.x, a.y / b.y, a.z / b.z, a.w / b.w);




}