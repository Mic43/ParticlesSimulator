using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Core.Collisions
{
    interface ICollider<T> where T:struct
    {
        bool AreColliding(ICollidable<T> first, ICollidable<T> second);
    }
    interface ICollider<T,TFirst,TSecond> 
        where T:struct 
        where TFirst: ICollidable<T> 
        where TSecond: ICollidable<T>
    {
        bool AreColliding(TFirst first,TSecond second);

        //private ICollider collider;
        //public bool AreColliding(ICollider other)
        //{
        //    if (other is T)
        //        return collider.AreColliding(other);
        //    return false;
        //}
    }
    class Collider<T, TFirst, TSecond> : ICollider<T> where T:struct
        where TFirst : class ,ICollidable<T>
        where TSecond : class,ICollidable<T>
    {
        private readonly ICollider<T, TFirst, TSecond> _collider;

        public Collider(ICollider<T, TFirst, TSecond> collider)
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

    //class Collider<T, V> : ICollider<T, V> where T : ICollider where V : ICollider
    //{
    //    Func<T, V, bool> collidingFunc;
    //    public bool IsCollidingWith(T first, V second)
    //    {
    //        return collidingFunc(first, second);
    //    }
    //}
    //abstract class Collider: ICollider
    //{
    //   // public Vector<float> Origin { get; set; }

        
    //    public bool IsCollidingWith(ICollider other)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    //  public abstract bool IsCollidingWith(T other);

    //}
  //  class RectanglesCollider : RectangleCollider<Rec>
}
