using System;
using System.Diagnostics;
using System.Numerics;
using Core.ElectricFieldSources;
using Core.Utils;

namespace Core
{
    public class Particle : CentralElectricFieldSource, ITickReceiver, ICollidable<float>
    {
        private IElectricFieldSource<float> _electricFieldSource;
        public IElectricFieldSource<float> ElectricFieldSource
        {
            get => _electricFieldSource;
            set => _electricFieldSource = value ?? throw new ArgumentNullException(nameof(value));
        }

        public float Mass { get; set; }
        public Vector<float> Velocity { get; private set; }

        public Particle(float mass,
            float charge = 0.0f,
            float coulombConstant = (float)Constants.CoulombConstant,
            Vector<float> position = default(Vector<float>),
            Vector<float> initialVelocity = default(Vector<float>)) 
            : this(new EmptyElectricFieldSource<float>(), mass,charge,
                coulombConstant, position: position, initialVelocity: initialVelocity)
        {
        }

        public Particle(IElectricFieldSource<float> electricFieldSource,
            float mass,
            float charge = 0.0f,
            float coulombConstant = (float)Constants.CoulombConstant,
            Vector<float> position = default(Vector<float>),
            Vector<float> initialVelocity = default(Vector<float>)) : base(position,charge, coulombConstant)
        {
            if (mass <= 0) throw new ArgumentOutOfRangeException(nameof(mass));

            Mass = mass;
            _electricFieldSource = electricFieldSource ?? throw new ArgumentNullException(nameof(electricFieldSource));
            Velocity = initialVelocity;
        }

        public void OnTick(TimeSpan elapsed)
        { 
            var intensity = ElectricFieldSource.GetIntensity(Position);
            var force = intensity * Charge; 
            var acceleration = force * (1 / Mass);

            float elapsedSeconds = (float)elapsed.TotalSeconds;
            var positionChange = acceleration * elapsedSeconds * elapsedSeconds * 0.5f + Velocity* elapsedSeconds;
           // Debug.AutoFlush = true;
          //  Debug.WriteLine(positionChange);
            Velocity+= acceleration * elapsedSeconds;

            Position = Position + positionChange;
        }

        public override string ToString()
        {
            return $"Position: {Position}, Velocity: {Velocity}";
        }
    }
}