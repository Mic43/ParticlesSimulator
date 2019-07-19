using Core.Utils;

namespace Core.Collisions.Colliders
{
    public class CommonCollider : ICollider<float>
    {
        public float MinimalDistance { get; }

        public CommonCollider(float minimalDistance)
        {
            MinimalDistance = minimalDistance;
        }

        public bool AreColliding(ICollidable<float> first, ICollidable<float> second)
        {
            return first.Position.DistanceTo(second.Position) <= MinimalDistance;
        }
    }
}