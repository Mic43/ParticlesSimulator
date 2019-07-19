using System;
using System.Diagnostics;
using Core.Collisions.Detectors;

namespace Core.Collisions.Resolvers
{
    public class DebugCollisionsResolver<T> : CollisionsResolverBase<T> where T:struct
    {
        public DebugCollisionsResolver(ICollisionsDetector<T> collisionsDetector) : base(collisionsDetector)
        {
        }

        public override void ResolveAll()
        {
            foreach (var collision in CollisionsDetector.Get())
            {
                Debug.WriteLine($"Collision detected: {collision.Object1} with {collision.Object2}");
            }
        }
    }
}