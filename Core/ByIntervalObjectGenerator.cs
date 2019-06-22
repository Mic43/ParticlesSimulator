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
        public ByIntervalObjectsGenerator(Func<T> objectsFactory, IObjectsWriter<T> objectsWriter, 
            TimeSpan interval) 
            : base( timeSpan =>
            {
                // int count = (int)(Math.Round(timeSpan.TotalMilliseconds / interval.TotalMilliseconds));
                int count = 1;

                objectsWriter.Put(Utils.Extensions.CreateItems<T>(count, objectsFactory));
            })
        {
            if (objectsFactory == null)
            {
                throw new ArgumentNullException(nameof(objectsFactory));
            }
            if (objectsWriter == null)
            {
                throw new ArgumentNullException(nameof(objectsWriter));
            }
        }
       // private int z;

    }
}
