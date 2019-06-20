using System;
using System.Collections.Generic;
using System.Numerics;

namespace Core.Utils
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
        public static IEnumerable<Vector<float>> CreatePositionsDistribution(Vector<float> start,
            Vector<float> end, int count)
        {
            float distance = (end - start).Length() / (count + 1);
            var vect = (end - start) * (1.0f / (count + 1));

            for (int i = 0; i < count; i++)
            {
              //  var delta = distance * (i + 1);
                yield return start + vect * (i + 1);
            }
        }
    }

}