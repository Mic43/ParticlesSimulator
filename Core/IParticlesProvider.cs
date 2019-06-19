using System.Collections.Generic;

namespace Core
{
    public interface IParticlesProvider
    {
        IEnumerable<ITickReceiver>  GetParticles();
    }
}