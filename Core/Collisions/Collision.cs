using System;
using System.Collections.Generic;

namespace Core.Collisions
{
    public class Collision : IEquatable<Collision>
    {
        public Collision(ICollidable<float> object1, ICollidable<float> object2)
        {
           // HashSet<> s;
           // s.
            this.Object1 = object1 ?? throw new System.ArgumentNullException(nameof(object1));
            this.Object2 = object2 ?? throw new System.ArgumentNullException(nameof(object2));
        }

        public ICollidable<float> Object1 { get; }
        public ICollidable<float> Object2 { get; }

        public override bool Equals(object obj)
        {
            return Equals(obj as Collision);
        }

        public bool Equals(Collision other)
        {
            return other != null &&
                   EqualityComparer<ICollidable<float>>.Default.Equals(Object1, other.Object1) &&
                   EqualityComparer<ICollidable<float>>.Default.Equals(Object2, other.Object2);
        }
    }
}