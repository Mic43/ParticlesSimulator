using System;
using Core.Infrastructure;

namespace Core.RunningTasks
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"> type of objects to generate</typeparam>
    public class ByIntervalObjectsGenerator<T> : RunningTask
    {
        private TimeSpan _lastGeneration= TimeSpan.Zero;

        public IObjectsWriter<T> ObjectsWriter { get; }
        public Func<T> ObjectsFactory { get; }
        public TimeSpan Interval { get; }

        public ByIntervalObjectsGenerator(Func<T> objectsFactory, IObjectsWriter<T> objectsWriter,
            TimeSpan interval)
        {
            OnUpdate = GenerateObjects;
            ObjectsFactory = objectsFactory ?? throw new ArgumentNullException(nameof(objectsFactory));
            ObjectsWriter = objectsWriter ?? throw new ArgumentNullException(nameof(objectsWriter));
            Interval = interval;
        }

        private void GenerateObjects(TimeSpan timeSpan)
        {
            _lastGeneration += timeSpan;
            if (_lastGeneration> Interval)
            {
                int count = 1;
                ObjectsWriter.Put(Utils.Extensions.CreateItems<T>(count, ObjectsFactory));
                _lastGeneration -= Interval;
            }
          
        }
    }

}
