namespace Simulator.Core
{
    public class Particle : IChargeCarrier<float>, ITickReceiver, IPositionable<float>
    { 
        public float Charge { get;  }
        public float Mass { get; }
        public Vector<float> Position { get; private set; }
        public IElectricFieldSource<float> ElectricFieldSource { get;}
        public Vector<float> Velocity { get; private set; }

        public Particle(float mass,
            float charge = 0.0f,
            Vector<float> position = default(Vector<float>),
            Vector<float> initialVelocity = default(Vector<float>)) 
            : this(new Empty<float>(), mass, charge, position, initialVelocity)
        {
        }

        public Particle(IElectricFieldSource<float> electricFieldSource,
            float mass,
            float charge = 0.0f,
            Vector<float> position = default(Vector<float>),
            Vector<float> initialVelocity = default(Vector<float>))
        {
            if (mass <= 0) throw new ArgumentOutOfRangeException(nameof(mass));

            Position = position;
            Charge = charge;
            Mass = mass;
            ElectricFieldSource = electricFieldSource ?? throw new ArgumentNullException(nameof(electricFieldSource));
            Velocity = initialVelocity;
        }

        public void OnTick(TimeSpan elapsed)
        { 
            var intensity = ElectricFieldSource.GetIntensity(Position);
            var force = intensity * Charge; 
            var acceleration = force * (1 / Mass);

            float elapsedSeconds = (float)elapsed.TotalSeconds;
            var positionChange = acceleration * elapsedSeconds * elapsedSeconds * 0.5f + Velocity* elapsedSeconds;
            Velocity = acceleration * elapsedSeconds;

            Position = Position + positionChange;
        }

        public override string ToString()
        {
            return $"Position: {Position}, Velocity: {Velocity}";
        }
    }
}