using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Core.Utils;

namespace Core.ElectricFieldSources
{
    public class CompoundElectricFieldSource<T> : IElectricFieldSource<T> where T : struct
    {
        public IEnumerable<IElectricFieldSource<T>> ElectricFieldSources { get; }
        private readonly IElectricFieldSource<T> _excludedFieldSource;

        public CompoundElectricFieldSource(IEnumerable<IElectricFieldSource<T>> electricFieldSources, 
            IElectricFieldSource<T> excludedFieldSource = null)
        {
            ElectricFieldSources = electricFieldSources ?? throw new ArgumentNullException(nameof(electricFieldSources));
            _excludedFieldSource = excludedFieldSource;
        }
        public CompoundElectricFieldSource(IElectricFieldSource<T> excludedFieldSource = null, params IElectricFieldSource<T>[] electricFieldSources)
            : this(electricFieldSources.AsEnumerable(), excludedFieldSource)
        {

        }

        public Vector<T> GetIntensity(Vector<T> location)
        {
            bool IsExcluded(IElectricFieldSource<T> fieldSource)
            {
                return _excludedFieldSource != null && _excludedFieldSource == fieldSource;
            }

            return ElectricFieldSources.Aggregate(Vector<T>.Zero,
                (vector, fieldSource) => vector +
                                         (IsExcluded(fieldSource) ? Vector<T>.Zero : fieldSource.GetIntensity(location)));
        }
    }
}