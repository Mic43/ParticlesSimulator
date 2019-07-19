using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Collisions
{
    public class Collision<T> : IEquatable<Collision<T>> where T:struct
    {
        private readonly ISet<ICollidable<T>> _contents = new HashSet<ICollidable<T>>();

        public Collision(ICollidable<T> object1, ICollidable<T> object2)
        {
            if (object1 == null) throw new ArgumentNullException(nameof(object1));
            if (object2 == null) throw new ArgumentNullException(nameof(object2));
            //this.Object1 = object1 ?? throw new System.ArgumentNullException(nameof(object1));
            //this.Object2 = object2 ?? throw new System.ArgumentNullException(nameof(object2));
            _contents.Add(object1);
            _contents.Add(object2);

         
        }

        public ICollidable<T> Object1 => _contents.First();

        public ICollidable<T> Object2 => _contents.Skip(1).Single();

        public override bool Equals(object obj)
        {
            return Equals(obj as Collision<T>);
        }

        public bool Equals(Collision<T> other)
        {
            return _contents.SetEquals(other._contents);
        }
    }
}