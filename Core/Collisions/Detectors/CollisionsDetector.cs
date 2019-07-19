using System.Collections.Generic;
using System.Linq;
using Core.Collisions.Colliders;
using Core.Infrastructure;
using Core.Utils;

namespace Core.Collisions.Detectors
{
    public class CollisionsDetector<T> : ICollisionsDetector<T> where T:struct
    {
        private readonly IEnumerable<ICollidable<T>> _objects;
        private readonly ICollider<T> _collider;

        public CollisionsDetector(IEnumerable<ICollidable<T>> objects,ICollider<T> collider)
        {
            _objects = objects ?? throw new System.ArgumentNullException(nameof(objects));
            _collider = collider;
        }

        public IEnumerable<Collision<T>> Get()
        {
            var collisions = _objects.SelectMany(obj => _objects.Except(Extensions.Single<ICollidable<T>>(obj))
                    .Where(obj2 => _collider.AreColliding(obj, obj2)),
                (obj, obj2) => new Collision<T>(obj, obj2)).Distinct();
            return collisions;
        }
    }
}