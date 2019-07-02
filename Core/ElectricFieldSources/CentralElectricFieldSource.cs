using System;
using System.Numerics;
using Core.Utils;

namespace Core.ElectricFieldSources
{
    public class CentralElectricFieldSource : Positionable<float>, IElectricFieldSource<float>, IChargeCarrier<float>
    {
        public float CoulombConstant { get; }
        public float Charge { get; }

        public CentralElectricFieldSource(Vector<float> position, float charge, 
            float coulombConstant =(float) Constants.CoulombConstant)
            : base(position)
        {
            if (coulombConstant <= 0) throw new ArgumentOutOfRangeException(nameof(coulombConstant));

            CoulombConstant = coulombConstant;
            Charge = charge;
        }

        public Vector<float> GetIntensity(Vector<float> location)
        {
            var distanceVector = (location - Position);
            if(distanceVector.Equals(Vector<float>.Zero))
                return new Vector<float>(float.MaxValue);
            var distance = distanceVector.Length();

            return CoulombConstant * Charge / (distance * distance * distance) * distanceVector; // wektor kierunkowy ?
        }
    }
}