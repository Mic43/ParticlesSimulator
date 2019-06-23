using System;
using System.Collections.Generic;

namespace Core.Infrastructure
{
    public class ObjectsStore<T> : 
        IObjectsProvider<T>, 
        IObjectsWriter<T>, 
        IObjectsRemover<T>
    {
        private readonly List<T> _receivers = new List<T>();

        public IEnumerable<T> Get()
        {
            return _receivers;
        }

        public void Put(IEnumerable<T> objects)
        {
            _receivers.AddRange(objects);
        }

        public void Remove(Predicate<T> removePredicate)
        {
            if (removePredicate == null) throw new ArgumentNullException(nameof(removePredicate));
            _receivers.RemoveAll(removePredicate);
        }
    }
}
