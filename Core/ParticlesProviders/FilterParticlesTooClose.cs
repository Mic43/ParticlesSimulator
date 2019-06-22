using System;
using System.Collections.Generic;
using System.Linq;
using Core.Utils;

namespace Core.ParticlesProviders
{
    public class FilterParticlesTooClose : IObjectsProvider<ITickReceiver>
    {
        public IEnumerable<IPositionable<float>> Positionables { get; private set; }
        public IObjectsProvider<ITickReceiver> ParticlesProvider { get; private set; }
        private readonly float _minimalDistance;

        public FilterParticlesTooClose(IEnumerable<IPositionable<float>> positionables, IObjectsProvider<ITickReceiver> particlesProvider,
            float minimalDistance = 0.05f)
        {
            Positionables = positionables ?? throw new ArgumentNullException(nameof(positionables));
            ParticlesProvider = particlesProvider ?? throw new ArgumentNullException(nameof(particlesProvider));
            this._minimalDistance = minimalDistance;
        }

        public IEnumerable<ITickReceiver> Get()
        {
            return ParticlesProvider.Get().OfType<IPositionable<float>>()
                .Where(part => !Positionables.Any(pos => TooClose(pos, part)))
                .Cast<ITickReceiver>();
        }

        private bool TooClose(IPositionable<float> pos, IPositionable<float> part)
        {
            return (pos.Position - part.Position).Length() <= _minimalDistance;
        }
    }
}