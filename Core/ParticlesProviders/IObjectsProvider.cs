using System.Collections.Generic;

namespace Core.ParticlesProviders
{
    public interface IObjectsProvider<T>
    {
        IEnumerable<T>  Get();
    }
}