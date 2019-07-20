using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Core.Utils
{
    public static class EnumerableExtensions
    {
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
        public static IEnumerable<T> CreateItems<T>(int count) where T : new()
        {
            return CreateItems(count, () => new T());
        }
        public static IEnumerable<T> CreateItems<T>(int count, Func<T> creator)
        {
            for (int i = 0; i < count; i++)
            {
                yield return creator();
            }
        }
        public static IEnumerable<T> Single<T>( this T element)
        {
            return Enumerable.Repeat(element, 1);
        }
    }

}