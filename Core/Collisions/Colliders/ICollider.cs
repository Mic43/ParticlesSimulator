namespace Core.Collisions.Colliders
{
    public interface ICollider<T> where T:struct
    {
        bool AreColliding(ICollidable<T> first, ICollidable<T> second);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">type of collidables position representation</typeparam>
    /// <typeparam name="TFirst"></typeparam>
    /// <typeparam name="TSecond"></typeparam>
    public interface ICollider<T, in TFirst, in TSecond> 
        where T:struct 
        where TFirst: ICollidable<T> 
        where TSecond: ICollidable<T>
    {
        bool AreColliding(TFirst first,TSecond second);
    }

    //class WrappingCollider<T, V> : ICollider<T, V> where T : ICollider where V : ICollider
    //{
    //    Func<T, V, bool> collidingFunc;
    //    public bool IsCollidingWith(T first, V second)
    //    {
    //        return collidingFunc(first, second);
    //    }
    //}
    //abstract class WrappingCollider: ICollider
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
