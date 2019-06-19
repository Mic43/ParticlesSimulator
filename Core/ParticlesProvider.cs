using System;
using System.Collections.Generic;

namespace Core
{
    public class ParticlesProvider : IParticlesProvider
    {
        private readonly IEnumerable<ITickReceiver> _receivers;

        public ParticlesProvider(IEnumerable<ITickReceiver> receivers)
        {
            this._receivers = receivers ?? throw new ArgumentNullException(nameof(receivers));
        }

        public IEnumerable<ITickReceiver> GetParticles()
        {
            return _receivers;
        }
    }
}