using System;
using System.Diagnostics;
using Core.Infrastructure;

namespace Core.Collisions
{
    public class DebugCollisionsResolver : ICollisionsResolver
    {
        private readonly ICollisionsDetector _collisionsDetector;

        public DebugCollisionsResolver(ICollisionsDetector collisionsDetector)
        {
            this._collisionsDetector = collisionsDetector ?? throw new ArgumentNullException(nameof(collisionsDetector));
        }

        public void ResolveAll()
        {
            foreach (var collision in _collisionsDetector.Get())
            {
                Debug.WriteLine($"Collision detected: {collision.Object1} with {collision.Object2}");
            }
        }
    }
}