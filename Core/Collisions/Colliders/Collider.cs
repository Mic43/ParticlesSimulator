using Core.Utils;

namespace Core.Collisions.Colliders
{
    public class Collider : ICollider<float>
    {
        public float MinimalDistance { get; }

        public Collider(float minimalDistance)
        {
            MinimalDistance = minimalDistance;
        }

        public bool AreColliding(ICollidable<float> first, ICollidable<float> second)
        {
            return first.Position.DistanceTo(second.Position) <= MinimalDistance;
        }
    }
}