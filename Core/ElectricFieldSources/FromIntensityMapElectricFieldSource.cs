using System;
using System.Collections.Generic;
using System.Numerics;
using System.Windows;
using Core.Interpolation;
using Core.Utils;
using Emgu.Util;

namespace Core.ElectricFieldSources
{
    public  class FromIntensityMapElectricFieldSource : IElectricFieldSource<float> 
    {
        private readonly IInterpolation _interpolation;
        public Vector<float>[,] IntensityMap { get; }
        private readonly float _unitSize;
        private readonly Vector<float> _origin;

        public FromIntensityMapElectricFieldSource(Vector<float>[,] intensityMap,IInterpolation interpolation, float unitSize)
            : this(intensityMap, interpolation, unitSize,Vector2D.Zero)
        {
        }

        public FromIntensityMapElectricFieldSource(Vector<float>[,] intensityMap,IInterpolation interpolation,
            float unitSize, Vector<float> origin)
        {
            _interpolation = interpolation ?? throw new ArgumentNullException(nameof(interpolation));
            _unitSize = unitSize;
            _origin = origin;
            IntensityMap = intensityMap ?? throw new ArgumentNullException(nameof(intensityMap));
        }

        public Vector<float> GetIntensity(Vector<float> location)
        {
            Vector<float> vector = (location - _origin) * (1 / _unitSize);
            (int x, int y) position = ((int)vector.X(), (int)vector.Y());

            if (position.x < 0 || position.x >= IntensityMap.GetLength(0)
                               || position.y < 0 || position.y >= IntensityMap.GetLength(1))
                return Vector2D.Zero;
            return IntensityMap[position.x, position.y];
           // var intensity = _interpolation.CalculateValue(TODO, position.y, IntensityMap);
            //return intensity;
        }

    }
}