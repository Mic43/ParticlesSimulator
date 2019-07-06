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

        public static Vector<float> Zero { get; } = Create(0, 0);

        public static Vector<float> One { get; } = Create(1, 1);
    }
}