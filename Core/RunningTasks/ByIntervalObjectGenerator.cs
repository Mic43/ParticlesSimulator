using System;
using Core.Infrastructure;

namespace Core.RunningTasks
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"> type of objects to generate</typeparam>
    public class ByIntervalObjectsGenerator<T> : ByIntervalPerform
    {

        public IObjectsWriter<T> ObjectsWriter { get; }
        public Func<T> ObjectsFactory { get; }

        public ByIntervalObjectsGenerator(Func<T> objectsFactory, IObjectsWriter<T> objectsWriter,
            TimeSpan interval) : base(interval)
        {
            ObjectsFactory = objectsFactory ?? throw new ArgumentNullException(nameof(objectsFactory));
            ObjectsWriter = objectsWriter ?? throw new ArgumentNullException(nameof(objectsWriter));
            ActionToPerform = GenerateObjects;
        }

        private void GenerateObjects()
        {
            //TODO: proper count calucaltion
            int count = 1;
            ObjectsWriter.Put(Utils.Extensions.CreateItems<T>(count, ObjectsFactory));
           
        }
    }

}
