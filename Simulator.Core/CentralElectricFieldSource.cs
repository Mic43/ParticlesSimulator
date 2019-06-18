using System.Numerics;

namespace Core
{
    public class CentralElectricFieldSource : IElectricFieldSource<float>, IPositionable<float>, IChargeCarrier<float>
    {
        public Vector<float> Position { get; }
        public float CoulombConstant { get; }
        public float Charge { get; }

        public CentralElectricFieldSource(Vector<float> position, float coulombConstant, float charge)
        {

            Position = position;
            CoulombConstant = coulombConstant;
            Charge = charge;
        }

        public Vector<float> GetIntensity(Vector<float> location)
        {
            var distanceVector = (location - Position);
            var distance = Extensions.Length((Vector<float>) distanceVector);
            return CoulombConstant * Charge / (distance * distance * distance) * distanceVector; // wektor kierunkowy ?
        }
    }
}