using System.Collections.Generic;

namespace Core.Collisions.Detectors
{
    public interface ICollisionsDetector<T> where T:struct
    {
        IEnumerable<Collision<T>> Get();
    }
}