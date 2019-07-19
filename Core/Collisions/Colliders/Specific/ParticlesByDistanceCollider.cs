using System;
using Core.Utils;

namespace Core.Collisions.Colliders.Specific
{
    public class ParticlesByDistanceCollider : ICollider<float,Particle,Particle>
    {
        private readonly float _minDistance;

        public ParticlesByDistanceCollider(float minDistance)
        {
            if (minDistance <= 0) throw new ArgumentOutOfRangeException(nameof(minDistance));
            this._minDistance = minDistance;
        }

        public bool AreColliding(Particle first, Particle second)
        {
            return first.Position.DistanceTo(second.Position) < _minDistance;
        }
    }
}