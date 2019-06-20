using System.Collections.Generic;

namespace Core.ParticlesProviders
{
    public interface IParticlesProvider
    {
        IEnumerable<ITickReceiver>  GetParticles();
    }

    interface IParticlesWriter
    {
        void PutParticles(IEnumerable<ITickReceiver> particles);
    }
}