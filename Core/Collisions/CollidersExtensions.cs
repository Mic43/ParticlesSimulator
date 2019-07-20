using System;
using Core.Collisions.Colliders;

namespace Core.Collisions
{
    public static class CollidersExtensions
    {
        //public static WrappingCollider<T, TFirst, TSecond> Wrap<T, TFirst, TSecond>(ICollider<T, TFirst, TSecond> collider)
        //    where T : struct
        //    where TFirst : class, ICollidable<T>
        //    where TSecond : class, ICollidable<T>

        //{
        //    if (collider == null) throw new ArgumentNullException(nameof(collider));
        //    return new WrappingCollider<T, TFirst, TSecond>(collider);
        //}

        public static WrappingCollider<T, TFirst, TSecond> Wrap<T, TFirst, TSecond> (this ICollider<T, TFirst, TSecond>  collider)
            where T : struct
            where TFirst : class, ICollidable<T>
            where TSecond : class, ICollidable<T>
        {
            if (collider == null) throw new ArgumentNullException(nameof(collider));
            return new WrappingCollider<T, TFirst, TSecond>(collider);
        }
    }
}