namespace Core.Collisions.Detectors
{
    //public class SimpleCollisionsDetector : ICollisionsDetector
    //{
    //    private IObjectsProvider<ICollidable<float>> _objects;
    //    public float MinimalDistance { get; }

    //    public SimpleCollisionsDetector(IObjectsProvider<ICollidable<float>> objects, float minimalDistance = 0.01f)
    //    {
    //        _objects = objects ?? throw new System.ArgumentNullException(nameof(objects));
    //        this.MinimalDistance = minimalDistance;
    //    }

    //    private bool AreTooClose(ICollidable<float> object1, ICollidable<float> object2)
    //    {
    //        return (object1.Position - object2.Position).Length() <= MinimalDistance;
    //    }

    //    public IEnumerable<Collision> Get()
    //    {
    //        var collisions = _objects.Get().SelectMany(obj => _objects.Get().Except(Extensions.Single(obj))
    //            .Where(obj2 => AreTooClose(obj, obj2)),
    //                (obj, obj2) => new Collision(obj, obj2)).Distinct();
    //        return collisions;
    //    } 
    //}
}