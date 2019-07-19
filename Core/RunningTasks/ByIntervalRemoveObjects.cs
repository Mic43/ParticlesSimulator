using System;
using Core.Infrastructure;

namespace Core.RunningTasks
{
    public class ByIntervalRemoveObjects<T> : ByIntervalPerform
    {

        public IObjectsRemover<T> ObjectsRemover { get; }

        public ByIntervalRemoveObjects( IObjectsRemover<T> objectsRemover,
            Predicate<T> removePredicate,TimeSpan interval) 
            : base(interval)
        {
            ObjectsRemover = objectsRemover ?? throw new ArgumentNullException(nameof(objectsRemover));

            ActionToPerform = () => ObjectsRemover.Remove(removePredicate);
        }
    }
}