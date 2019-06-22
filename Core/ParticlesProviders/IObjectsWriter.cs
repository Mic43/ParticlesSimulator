using System.Collections.Generic;

namespace Core.ParticlesProviders
{
    public interface IObjectsWriter<T>
    {
        void Put(IEnumerable<T> objects);
    }
}