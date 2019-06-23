using System.Collections.Generic;

namespace Core.Infrastructure
{
    public interface IObjectsProvider<out T>
    {
        IEnumerable<T>  Get();
    }
}