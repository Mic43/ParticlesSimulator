using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Core.ElectricFieldSources
{
    public class CompoundElectricFieldSource<T> : IElectricFieldSource<T> where T : struct
    {
        public IEnumerable<IElectricFieldSource<T>> ElectricFieldSources { get; }

        public CompoundElectricFieldSource(IEnumerable<IElectricFieldSource<T>> electricFieldSources)
        {
            ElectricFieldSources = electricFieldSources ?? throw new ArgumentNullException(nameof(electricFieldSources));
        }

        public Vector<T> GetIntensity(Vector<T> location)
        {
            return ElectricFieldSources.Aggregate(Vector<T>.Zero,
                (vector, fieldSource) => vector + fieldSource.GetIntensity(location));
        }
    }
}