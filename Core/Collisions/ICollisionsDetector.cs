using System.Collections.Generic;

namespace Core.Collisions
{
    public interface ICollisionsDetector
    {
        IEnumerable<Collision> Get();
    }
}