using System.Threading.Tasks;
using Core.ParticlesProviders;

namespace Core.RunningTasks
{

    public class Simulator : RunningTask
    {
        public Simulator(IObjectsProvider<ITickReceiver> particlesProvider) 
            : base(timespan =>
           {
               Parallel.ForEach<ITickReceiver>(particlesProvider.Get(),
                   (particle) => particle.OnTick(timespan));
               //foreach (var particle in particlesProvider.Get())
               //{
               //    particle.OnTick(timespan);
               //}
           })
        {
        }
    }
}