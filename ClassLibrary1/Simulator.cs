using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ClassLibrary1
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


        public Task StartAsync(CancellationToken cancellationToken)
        {
            return StartAsync(cancellationToken, null);
        }

        public async Task StartAsync(CancellationToken cancellationToken,IProgress<ICollection<IPositionable<float>>> progress)
        {
            await Task.Run(() =>
            {
                _stopwatch.Start();
                while (!cancellationToken.IsCancellationRequested)
                {
                    progress?.Report(Particles.OfType<IPositionable<float>>().ToList());
                    Tick();
                    _stopwatch.Restart();
                }
            }, cancellationToken);

        }
    }
}