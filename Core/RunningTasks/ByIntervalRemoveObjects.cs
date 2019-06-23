using System;
using Core.Infrastructure;

namespace Core.RunningTasks
{
    public class ByIntervalRemoveObjects<T> : RunningTask
    {
        private readonly Predicate<T> _removePredicate;
        private TimeSpan _lastGeneration = TimeSpan.Zero;

        public IObjectsProvider<T> ObjectsProvider { get; }
        public IObjectsRemover<T> ObjectsRemover { get; }
        public TimeSpan Interval { get; }

        public ByIntervalRemoveObjects(IObjectsProvider<T> objectsProvider, IObjectsRemover<T> objectsRemover,
            Predicate<T> removePredicate,TimeSpan interval)
        {
            _removePredicate = removePredicate ?? throw new ArgumentNullException(nameof(removePredicate));
            ObjectsProvider = objectsProvider ?? throw new ArgumentNullException(nameof(objectsProvider));
            ObjectsRemover = objectsRemover ?? throw new ArgumentNullException(nameof(objectsRemover));
            Interval = interval;
            OnUpdate = RemoveObjects;
        }

        private void RemoveObjects(TimeSpan timeSpan)
        {
            _lastGeneration += timeSpan;
            if (_lastGeneration > Interval)
            {
                ObjectsRemover.Remove(_removePredicate);
                _lastGeneration -= Interval;
            }

        }
    }
}