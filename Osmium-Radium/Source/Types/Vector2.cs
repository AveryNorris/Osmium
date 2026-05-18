namespace OsmiumRadium;

public struct Vector2
{
    public float x;
    public float y;

    public Vector2(float x, float y) {
        this.x = x;
        this.y = y;
    }

    public Vector2(float v) {
        this.x = v;
        this.y = v;
    }

    public static Vector2 operator /(Vector2 v, float divisor) => new Vector2(v.x / divisor, v.y / divisor);
    
    public static Vector2 operator +(Vector2 v, float value) => new Vector2(v.x + value, v.y + value);
    
    public static Vector2 operator -(Vector2 v, float value) => new Vector2(v.x - value, v.y - value);
    
    public static Vector2 operator *(Vector2 v, float value) => new Vector2(v.x * value, v.y * value);

    
    
    public static Vector2 operator +(Vector2 a, Vector2 b) => new Vector2(a.x + b.x, a.y + b.y);
    
    public static Vector2 operator -(Vector2 a, Vector2 b) => new Vector2(a.x - b.x, a.y - b.y);
    
    public static Vector2 operator *(Vector2 a, Vector2 b) => new Vector2(a.x * b.x, a.y * b.y);
    
    public static Vector2 operator /(Vector2 a, Vector2 b) => new Vector2(a.x / b.x, a.y / b.y);




}