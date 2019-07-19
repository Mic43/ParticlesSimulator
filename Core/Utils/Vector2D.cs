using System.Numerics;

namespace Core.Utils
{
    public static class Vector2D
    {
        
        public static Vector<float> Create(float x, float y)
        {
            return new Vector<float>(new float[] {x,y,0,0});
        }

        public static Vector<float> Create(float value)
        {
            return Create(value, value);
        }
        public static float X(this Vector<float> vec)
        {
            return vec[0];
        }
        public static float Y(this Vector<float> vec)
        {
            return vec[1];
        }
        public static Vector<float> Zero { get; } = Create(0, 0);

        public static Vector<float> One { get; } = Create(1, 1);
    }
}