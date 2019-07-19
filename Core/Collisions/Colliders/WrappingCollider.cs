using System;

namespace Core.Collisions.Colliders
{
    public class WrappingCollider<T, TFirst, TSecond> : ICollider<T> where T:struct
        where TFirst : class ,ICollidable<T>
        where TSecond : class,ICollidable<T>
    {
        private readonly ICollider<T, TFirst, TSecond> _collider;

        public WrappingCollider(ICollider<T, TFirst, TSecond> collider)
        {
            _collider = collider ?? throw new ArgumentNullException(nameof(collider));
        }

        public bool AreColliding(ICollidable<T> first, ICollidable<T> second)
        {
            if (first is TFirst && second is TSecond)
                return _collider.AreColliding(first as TFirst, second as TSecond);
            return false;
        }

    }
}