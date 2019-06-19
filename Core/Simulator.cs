using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Core
{
    public class Simulator
    {
        private readonly IParticlesProvider _particlesProvider;
        private readonly Stopwatch _stopwatch = new Stopwatch();
        private  TimeSpan _simulationTime = TimeSpan.Zero;
        // public ICollection<ITickReceiver> Particles { get; private set; }
        public bool IsStarted { get; private set; } = false;

        public Simulator(IParticlesProvider particlesProvider)
        {
            _particlesProvider = particlesProvider ?? throw new ArgumentNullException(nameof(particlesProvider));
        }

        public void DoTick()
        {         
            if(!IsStarted)
                throw new InvalidOperationException("You must start simulation first");

            foreach (var particle in _particlesProvider.GetParticles())
            {
                particle.OnTick(_stopwatch.Elapsed);    
            }

            _simulationTime += _stopwatch.Elapsed;
            _stopwatch.Restart();
        }

        public void Start()
        {
            IsStarted = true;
            _stopwatch.Start();
        }

        public void Stop()
        {
            IsStarted = false;
            _stopwatch.Stop();
        }
    }
}