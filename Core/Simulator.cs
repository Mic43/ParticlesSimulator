using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Core
{
    public class Simulator
    {
        private readonly Stopwatch _stopwatch = new Stopwatch();
        public ICollection<ITickReceiver> Particles { get; private set; }
        public event EventHandler<TimeSpan> StepOccured;

        public Simulator() : this(Enumerable.Empty<ITickReceiver>().ToList())
        {
        }

        public Simulator(ICollection<ITickReceiver> particles)
        {
            Particles = particles ?? throw new ArgumentNullException(nameof(particles));
        }

        private void DoTick()
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
            await Task.Run(async () =>
            {
                _stopwatch.Start();
               // long it = 0;
                while (!cancellationToken.IsCancellationRequested)
                {
                    await Task.WhenAll(Task.Delay(1, cancellationToken),
                        Task.Run( () =>
                        {
                            //await Task.Delay(1500);
                            progress?.Report(Particles.OfType<IPositionable<float>>().ToList());
                            DoTick();
                            _stopwatch.Restart();
                         //   it++;
                            //Debug.WriteLine(it);
                        }, cancellationToken));
                    
                }
            }, cancellationToken);
            
        }

        protected virtual void OnStepOccured(TimeSpan timeSpan)
        {
            StepOccured?.Invoke(this, timeSpan);
        }
    }
}