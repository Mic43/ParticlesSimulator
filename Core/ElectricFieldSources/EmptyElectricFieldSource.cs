﻿using System.Numerics;

namespace Core.ElectricFieldSources
{
    public class EmptyElectricFieldSource<T> : IElectricFieldSource<T> where T : struct
    {
        public Vector<T> GetIntensity(Vector<T> location)
        {
            return Vector<T>.Zero;
        }
    }
}