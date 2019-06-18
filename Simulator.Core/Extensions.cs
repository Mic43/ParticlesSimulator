using System;
using System.Numerics;

namespace Core
{
    public static class Extensions
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
    }
}