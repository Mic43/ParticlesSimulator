using System.Collections.Generic;
using Core.Infrastructure;

namespace Core.Collisions
{
    public interface ICollisionsResolver
    {
        void ResolveAll();
    }

    class CollisionsResolver : ICollisionsResolver
    {
        private IObjectsProvider<Collision> collisions;
        public void ResolveAll()
        {

        }
    }

    internal class Collision
    {
    }

    interface ICollisionsProvider
    {
        IEnumerable<Collision> Get();
    }

    class CollisionsProvider : ICollisionsProvider
    {
        private IEnumerable<IPositionable<float>> objects;
        public IEnumerable<Collision> Get()
        {
            throw new System.NotImplementedException();
        }
    }
}