using System;
using Core.Collisions.Detectors;

namespace Core.Collisions.Resolvers
{
    public abstract class CollisionsResolverBase<T> : ICollisionsResolver where T : struct
    {
        protected ICollisionsDetector<T> CollisionsDetector;

        protected CollisionsResolverBase(ICollisionsDetector<T> collisionsDetector)
        {
            this.CollisionsDetector = collisionsDetector ?? throw new ArgumentNullException(nameof(collisionsDetector));
        }

        public abstract void ResolveAll();
    }
}