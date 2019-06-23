using System.Collections.Generic;

namespace Core.Infrastructure
{
    public interface IObjectsWriter<in T>
    {
        void Put(IEnumerable<T> objects);
    }
}