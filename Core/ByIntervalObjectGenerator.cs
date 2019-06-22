using Core.ParticlesProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"> type of objects to generate</typeparam>
    public class ByIntervalObjectsGenerator<T> : RunningTask
    {
        private TimeSpan lastGeneration= TimeSpan.Zero;

        public ByIntervalObjectsGenerator(Func<T> objectsFactory, IObjectsWriter<T> objectsWriter,
            TimeSpan interval)
        {
            if (objectsFactory == null)
            {
                throw new ArgumentNullException(nameof(objectsFactory));
            }
            if (objectsWriter == null)
            {
                throw new ArgumentNullException(nameof(objectsWriter));
            }
            this.OnUpdate = GenerateObjects;
            ObjectsFactory = objectsFactory;
            ObjectsWriter = objectsWriter;
            Interval = interval;
        }

       

        public Func<T> ObjectsFactory { get; }
        public IObjectsWriter<T> ObjectsWriter { get; }
        public TimeSpan Interval { get; }

        private void GenerateObjects(TimeSpan timeSpan)
        {
            lastGeneration += timeSpan;
            if (lastGeneration> Interval)
            {
                int count = 1;
                ObjectsWriter.Put(Utils.Extensions.CreateItems<T>(count, ObjectsFactory));
                lastGeneration -= Interval;
            }
          
        }
    }

}
