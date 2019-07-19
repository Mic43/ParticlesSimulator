using Core.Collisions.Detectors;

namespace Core.Collisions.Resolvers
{
    public class ForceStopIfParticleCollisionResolver<T> : CollisionsResolverBase<T> where T : struct
    {
        public ForceStopIfParticleCollisionResolver(ICollisionsDetector<T> collisionsDetector) : base(collisionsDetector)
        {
        }

        public override void ResolveAll()
        {
            foreach (var collision in CollisionsDetector.Get())
            {
                if (collision.Object1 is Particle particle)
                {
                    particle.ForceStop = true;
                }
                if (collision.Object2 is Particle particle2)
                {
                    particle2.ForceStop = true;
                }
            }
        }
    }
}