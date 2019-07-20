using System;
using System.Numerics;

namespace Core.Utils
{
    public static class VectorExtensions
    {
        public static float Length (this Vector<float> vec) 
        {
            float acc = default(float);
            for (int i = 0; i < Vector<float>.Count; i++)
            {
                acc += vec[i]*vec[i];
            }
            return (float) Math.Sqrt(acc);
        }

        public static float DistanceTo(this Vector<float> vec, Vector<float> other)
        {
            return (vec - other).Length();
        }
    }
}