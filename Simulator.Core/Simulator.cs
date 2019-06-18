using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Core
{
    public class Simulator
    {
        private readonly Stopwatch _stopwatch = new Stopwatch();
        public ICollection<ITickReceiver> Particles { get; private set; }

        public Simulator() : this(Enumerable.Empty<ITickReceiver>().ToList())
        {
        }

        public Simulator(ICollection<ITickReceiver> particles)
        {
            Particles = particles ?? throw new ArgumentNullException(nameof(particles));
        }

        private void Tick()
        {
            foreach (var particle in Particles)
            {
                particle.OnTick(_stopwatch.Elapsed);    
            }   
        }

        public async Task Start()
        {
            await Task.Run(() =>
            {
                _stopwatch.Start();
                while (true)
                {
                    Tick();
                    _stopwatch.Restart();
                   // Debug.WriteLine(Particles.Single().ToString());
                }
            });

        }
    }
}