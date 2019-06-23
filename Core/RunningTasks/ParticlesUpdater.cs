using System;
using System.Threading.Tasks;
using Core.Infrastructure;

namespace Core.RunningTasks
{

    public class ParticlesUpdater : RunningTask
    {
        private float _simulationSpeed = 1;

        public float SimulationSpeed
        {
            get => _simulationSpeed;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Simulation speed must be number greater than zero.");
                _simulationSpeed = value;
            }
        }

        public ParticlesUpdater(IObjectsProvider<ITickReceiver> particlesProvider)
        {
            //Parallel.ForEach<ITickReceiver>(particlesProvider.Get(),
            //    (particle) => particle.OnTick(timespan));
            OnUpdate = (timespan =>
            {
                foreach (var particle in particlesProvider.Get())
                {
                    particle.OnTick(TimeSpan.FromMilliseconds(SimulationSpeed * timespan.TotalMilliseconds));
                }
            });

        }
    }
}