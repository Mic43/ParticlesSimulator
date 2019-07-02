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
            OnUpdate = (timespan =>
            {
                foreach (var tickReceiver in particlesProvider.Get())
                {
                    tickReceiver.OnTick(TimeSpan.FromMilliseconds(SimulationSpeed * timespan.TotalMilliseconds));
                }

                //Parallel.ForEach(particlesProvider.Get(),
                //    (particle) => particle.OnTick(TimeSpan.FromMilliseconds(SimulationSpeed * timespan.TotalMilliseconds)));
            });

        }
    }
}